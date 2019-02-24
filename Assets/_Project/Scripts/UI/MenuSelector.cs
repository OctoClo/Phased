using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Rewired;

public class MenuSelector : MonoBehaviour
{
    public Selectable FirstSelectable;
    public bool VerticalLayout = true;

    EventSystem eventSystem;

    void Awake()
    {
        eventSystem = EventSystem.current;
    }

    void OnEnable() 
    {
        StartCoroutine(SelectFirstButtonLater());
    }

    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null && ReInput.players.AllPlayers.Any(player => player.GetAxis(VerticalLayout ? "UIVertical" : "UIHorizontal") != 0f))
        {
            StartCoroutine(SelectFirstButtonLater());
        }
    }

    private IEnumerator SelectFirstButtonLater()
    {
        yield return null;
        eventSystem.SetSelectedGameObject(FirstSelectable.gameObject);
    }
}
