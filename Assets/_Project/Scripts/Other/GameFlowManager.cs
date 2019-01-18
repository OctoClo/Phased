using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStartedEvent>(OnGameStartedEvent);
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        GameScore.Reset();
    }
}
