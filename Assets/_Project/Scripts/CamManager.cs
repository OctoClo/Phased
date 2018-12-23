using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class CamManager : MonoBehaviour
{
    public int CurrentMode;
    public List<Camera> Cameras;
    public List<GameObject> PlayersBounds;

    void Start()
    {
        ActivateCam();
    }
    
    void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().cKey.wasReleasedThisFrame)
        {
            CurrentMode = (CurrentMode + 1) % Cameras.Count;
            Debug.Log("C pressed ! New mode : " + CurrentMode);
            ActivateCam();
        }
    }

    void ActivateCam()
    {
        foreach (Camera cam in Cameras)
        {
            cam.gameObject.SetActive(false);
        }

        foreach (GameObject playersBounds in PlayersBounds)
        {
            playersBounds.gameObject.SetActive(false);
        }

        Cameras[CurrentMode].gameObject.SetActive(true);
        PlayersBounds[CurrentMode].gameObject.SetActive(true);
    }
}
