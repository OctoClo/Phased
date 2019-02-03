using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : Singleton<ScreenShake>
{
    private Vector3 startPosition;
    float shakeIntensity;

    bool shaking = false;

    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {
        if (shaking)
        {
            Vector2 offset = Random.insideUnitCircle;
            transform.position = startPosition + new Vector3 (offset.x, offset.y, 0) * shakeIntensity;
        }
    }

    IEnumerator ShakeDuration(float time)
    {
        shaking = true;
        yield return new WaitForSecondsRealtime(time);
        shaking = false;
        transform.position = startPosition;
    }

    public void Shake(float time, float intensity)
    {
        shakeIntensity = intensity;
        StartCoroutine(ShakeDuration(time));
    }
}