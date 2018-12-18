using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    public int LifeCount = 5;

    int previousFrameLifeCount;
    int flickerCounter;
    
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
            return ( LifeCount <= 0 );
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

    void Start()
    {
        flickerCounter = 0;
        previousFrameLifeCount = LifeCount;
    }

    public void RemoveLife( Spaceship other )
    {
        damageSource = other;

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
            flickerCounter--;
        }

        // Check if the player collided with an obstacle in the current frame
        if (previousFrameLifeCount != LifeCount)
        {
            flickerCounter = WorldConstants.Instance.PlayerInvulnerableFrameCount;
        }

        previousFrameLifeCount = LifeCount;
    }
}
