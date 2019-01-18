using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        GameScore.Reset();
    }
}
