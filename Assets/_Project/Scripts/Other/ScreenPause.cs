using System.Collections;
using UnityEngine;

public class ScreenPause : MonoBehaviour
{
    // Singleton
    private static ScreenPause instance;
    public static ScreenPause Instance { get { return instance; } }

    private Coroutine co = null;
    private float previousPauseTime = 0f;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Pause(float pauseTime)
    {
        if (co != null)
        {
            if (pauseTime > previousPauseTime)
            {
                StopCoroutine(co);
                co = StartCoroutine(PauseCoroutine(pauseTime));
                previousPauseTime = pauseTime;
            }
        }
        else
        {
            co = StartCoroutine(PauseCoroutine(pauseTime));
            previousPauseTime = pauseTime;
        }
    }

    private IEnumerator PauseCoroutine(float pauseTime)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
        previousPauseTime = 0f;
        yield return null;
    }
}
