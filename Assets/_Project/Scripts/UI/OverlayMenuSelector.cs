using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Rewired;

public class OverlayMenuSelector : MonoBehaviour
{
    public Selectable firstSelected;
    public EventSystem eventSystem;

    [SerializeField]
    GameObject currentObj;

    void OnEnable() 
    {
        SelectFirstInput();
    }

    void Update()
    {
        currentObj = eventSystem.currentSelectedGameObject;
        //if first input not selected, and any key from joysticks
        //select first input
        if(ReInput.controllers.Joysticks.Any(x => x.GetAnyButtonDown()) && eventSystem.currentSelectedGameObject == null)
        {
            SelectFirstInput();
        }
    }

    void SelectFirstInput()
    {
        firstSelected.Select();
        eventSystem.SetSelectedGameObject(firstSelected.gameObject);
    }
}
