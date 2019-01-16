using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}

public class WorldConstants : Singleton<WorldConstants>
{
    [Header("General Speeds")]
    public float WorldScrollSpeed = 2.0f;
    public float ObstacleMultiplier = 3.0f;
    public float SpawnableMultiplier = 1f;
    public float EnemyVoidSpeedMultiplier = 5f;

    [Header("Enemy Speeds")]
    public float EnemyMultiplier = 1.50f;
    public float TankMultiplier = 10.0f;
    public float TankLateralMultiplier = 10.0f;

    [Header("Sinusoid Pattern")]
    public float EnemySinusPatternLateralMultiplier = 4.0f;
    public float EnemySinusPatternLateralWidth = 20.0f;
    
    [Header("Spaceships")]
    public float PlayerInvulnerabilityDuration = 0.500f; // In seconds    
    public int PlayerFlickerFrequency = 2; // In frame frequency (frameNumber % PlayerFlickerFrequency)
    public float PlayerHitVibrationDuration = 0.5f;

    [Header("Critical Hits")]
    public float MarkBulletTime = 1f;
    public int MarkBulletDamageMultiplicator = 2;

    [Header("Shake it")]
    public float ScreenShakeHitDuration = 0.5f;
    public float ScreenShakeHitIntensity = 1f;
    public float ScreenShakeDeathDuration = 3f;
    public float ScreenShakeDeathIntensity = 2f;
}
