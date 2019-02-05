using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Rewired;

public class GameFlowManager : MonoBehaviour
{
    public UIManager UIManager;
    public Leaderboard leaderboard;
    bool paused = false;

    IEnumerator WaitForAllSetup()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        EventManager.Instance.Raise(new GameStartedEvent() { });
    }

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
        leaderboard.entryAddedForSession = false;
    }

    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (ReInput.players.GetPlayer(i).GetButtonDown("Pause") && UIManager.IsGameActive())
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
