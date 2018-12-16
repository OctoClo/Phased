using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    public int lifeCount = 5;

    private int previousFrameLifeCount;
    private int flickerCounter;

    
    public bool IsInvulnerable
    {
        get
        {
            return (flickerCounter > 0);
        }
    }

    public bool HasNoLifeLeft
    {
        get
        {
            return ( lifeCount <= 0 );
        }
    }

    private Spaceship damageSource;
    public Spaceship DamageSource
    {
        get
        {
            return damageSource;
        }
        private set
        {
            damageSource = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        flickerCounter = 0;
        previousFrameLifeCount = lifeCount;
    }

    public void RemoveLife( Spaceship other )
    {
        damageSource = other;

        lifeCount--;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeCount <= 0)
        {
            // TODO GameOver Screen
            Application.Quit();
        }

        if (IsInvulnerable && (Time.frameCount % WorldConstants.Instance.PlayerFlickerFrequency) == 0)
        {
            flickerCounter--;
        }

        // Check if the player collided with an obstacle in the current frame
        if (previousFrameLifeCount != lifeCount)
        {
            flickerCounter = WorldConstants.Instance.PlayerInvulnerableFrameCount;
        }

        previousFrameLifeCount = lifeCount;
    }
}
