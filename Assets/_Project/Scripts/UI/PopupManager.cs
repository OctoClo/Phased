using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public Popup PopupScore;

    [HideInInspector]
    public Color PopupColor;
    
    GameObject popupsFolder;

    private void Start()
    {
        GameScore.PopupManager = this;
        popupsFolder = FolderManager.Instance.PopupsFolder;
    }

    public void CreatePopupScore(string score, Vector3 position)
    {
        Popup instance = Instantiate(PopupScore, popupsFolder.transform);
        instance.SetText("+" + score);
        instance.SetColor(PopupColor);
    }
}
