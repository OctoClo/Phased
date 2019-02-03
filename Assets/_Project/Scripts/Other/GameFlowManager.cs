using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using System.Linq;

public class GameFlowManager : MonoBehaviour
{
    bool paused = false;

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GamePausedEvent>(OnGamePausedEvent);
        EventManager.Instance.AddListener<GameResumedEvent>(OnGameResumedEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.RemoveListener<GamePausedEvent>(OnGamePausedEvent);
        EventManager.Instance.RemoveListener<GameResumedEvent>(OnGameResumedEvent);
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        GameScore.Reset();
    }

    void Update()
    {
        if (Gamepad.all.Any(x => x.startButton.wasReleasedThisFrame))
        {
            if (!paused)
            {
                EventManager.Instance.Raise(new GamePausedEvent() { });
            }
            else
            {
                EventManager.Instance.Raise(new GameResumedEvent() { });
            }
        }
    }

    void OnGamePausedEvent(GamePausedEvent e)
    {
        Time.timeScale = 0;
        paused = !paused;
    }

    void OnGameResumedEvent(GameResumedEvent e)
    {
        Time.timeScale = 1;
        paused = !paused;
    }
}
