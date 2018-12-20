using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    public int LifeCount = 5;

    int previousFrameLifeCount;
    float flickerCounter;
    
    public bool IsInvulnerable
    {
        get
        {
            return (flickerCounter != 0.0f);
        }
    }

    public bool HasNoLifeLeft
    {
        get
        {
            return ( LifeCount <= 0 );
        }
    }
    public Spaceship DamageSource { get; private set; }

    void Start()
    {
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
    }

    void Update()
    {
        if (LifeCount <= 0)
        {
            // TODO GameOver Screen
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

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
}
