using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderManager : Singleton<FolderManager>
{
    public GameObject SpawnFolder;
    public GameObject BulletsFolder;

    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);    
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        foreach (Transform child in SpawnFolder.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in BulletsFolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
