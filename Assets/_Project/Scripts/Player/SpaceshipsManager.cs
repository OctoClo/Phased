using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsManager : Singleton<SpaceshipsManager>
{
    public List<GameObject> Spaceships;

    List<Spaceship> spaceshipsScripts = new List<Spaceship>();

    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);

        spaceshipsScripts.Add(Spaceships[0].GetComponent<Spaceship>());
        spaceshipsScripts.Add(Spaceships[1].GetComponent<Spaceship>());
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        ActivateSpaceships(true);
        InitializeSpaceships();
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

    void InitializeSpaceships()
    {
        spaceshipsScripts[0].Initialize();
        spaceshipsScripts[1].Initialize();
    }
}
