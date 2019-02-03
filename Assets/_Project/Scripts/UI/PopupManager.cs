using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public Popup PopupScore;
    
    GameObject PopupsFolder;

    private void Start()
    {
        GameScore.PopupManager = this;
        PopupsFolder = FolderManager.Instance.PopupsFolder;
    }

    public void CreatePopupScore(string score, Vector3 position)
    {
        Popup instance = Instantiate(PopupScore, position, Quaternion.identity, PopupsFolder.transform);
        instance.SetText("+" + score);
    }
}
