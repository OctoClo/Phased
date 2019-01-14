﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : Singleton<LifeCounter>
{
    public int LifeCount = 5;
    public Animator ScreenglowAnimator;

    int previousFrameLifeCount;
    float flickerCounter;

    int initialLifeCount;
    
    public bool IsInvulnerable
    {
        get
        {
            return (flickerCounter > 0.0f);
        }
    }
    
    public Spaceship DamageSource { get; private set; }

    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);

        initialLifeCount = LifeCount;
    }

    void Initialize()
    {
        LifeCount = initialLifeCount;
        flickerCounter = 0.0f;
        previousFrameLifeCount = LifeCount;
    }

    public void TriggerInvulnerability( float durationInSeconds )
    {
        flickerCounter = durationInSeconds;
    }

    public void RemoveLife( Spaceship other )
    {
        DamageSource = other;

        LifeCount--;

        if(LifeCount == 1){
            ScreenglowAnimator.SetBool("Low Health", true);
        } else{
            ScreenglowAnimator.SetTrigger("Hit");
        }

        if (LifeCount == 0)
        {
            EventManager.Instance.Raise(new GameEndEvent() { Victorious = false });
        }
    }

    void Update()
    {
        if (IsInvulnerable && (Time.frameCount % WorldConstants.Instance.PlayerFlickerFrequency) == 0)
        {
            flickerCounter -= Time.deltaTime;
        }

        // Check if the player collided with an obstacle in the current frame
        if (previousFrameLifeCount != LifeCount)
        {
            flickerCounter = WorldConstants.Instance.PlayerInvulnerabilityDuration;
        }

        previousFrameLifeCount = LifeCount;
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        Initialize();
    }
}
