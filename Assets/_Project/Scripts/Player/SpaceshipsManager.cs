using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsManager : Singleton<SpaceshipsManager>
{
    public List<GameObject> Spaceships;

    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);

        ActivateSpaceships(false);
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        ActivateSpaceships(true);
    }

    void OnGameEndEvent(GameEndEvent e)
    {
        ActivateSpaceships(false);
    }

    void ActivateSpaceships(bool activated)
    {
        Spaceships[0].SetActive(activated);
        Spaceships[1].SetActive(activated);
    }
}
