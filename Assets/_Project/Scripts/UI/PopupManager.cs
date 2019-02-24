using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public bool Popups = true;
    public Popup PopupScore;

    [HideInInspector]
    public Color PopupColor = new Color(1, 1, 1, 1);
    
    GameObject popupsFolder;

    private void Start()
    {
        GameScore.PopupManager = this;
        popupsFolder = FolderManager.Instance.PopupsFolder;
    }

    public void CreatePopupScore(string score, Vector3 position)
    {
        if (Popups)
        {
            Popup instance = Instantiate(PopupScore, popupsFolder.transform);
            instance.SetText("+" + score);
            instance.SetColor(PopupColor);
        }
    }
}
