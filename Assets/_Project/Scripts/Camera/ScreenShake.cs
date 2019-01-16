using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : Singleton<ScreenShake>
{
    private Vector3 startPosition;
    float shakeTimer, shakeIntensity;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            Vector2 offset = Random.insideUnitCircle;
            transform.position = startPosition + new Vector3 (offset.x, offset.y, 0) * shakeIntensity;
        }
        else
        {
            transform.position = startPosition;
        }
    }

    public void Shake(float time, float intensity)
    {
        shakeTimer = time;
        shakeIntensity = intensity;
    }
}