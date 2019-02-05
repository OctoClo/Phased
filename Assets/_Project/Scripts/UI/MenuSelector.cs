using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
// using UnityEngine.Experimental.Input;
using UnityEngine.UI;
using Rewired;

public class MenuSelector : MonoBehaviour
{
	[SerializeField]
    private Button[] buttons;

    [SerializeField]
    private float blockInputTime = 0.2f;

    [SerializeField]
    private bool horizontalMenu = false;

    private int lastSelectedButton;
	private int selectedButton;
    
    private float timeBetweenButtons = 0.2f;

    EventSystem eventSystem;
    //Keyboard keyboard;

    bool waiting = true;

    private void Start()
    {
        Debug.Log("Start menu selector");
        eventSystem = EventSystem.current;
        buttons[0].Select();
        eventSystem.SetSelectedGameObject(buttons[selectedButton].gameObject);
    }

    private void OnEnable()
    {
        if (!eventSystem)
            eventSystem = EventSystem.current;

        selectedButton = 0;
        lastSelectedButton = -1;

        StartCoroutine(WaitBeforeInput(blockInputTime));
    }

    private void OnDisable()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    IEnumerator WaitBeforeInput(float time)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(time);
        waiting = false;
    }

    private void Update()
	{
        if (!waiting)
        {
            //keyboard = InputSystem.GetDevice<Keyboard>();

            // if (lastSelectedButton != selectedButton)
            // {
            //     buttons[selectedButton].Select();
            //     eventSystem.SetSelectedGameObject(buttons[selectedButton].gameObject);
            //     lastSelectedButton = selectedButton;
            // }

            // if (Gamepad.all.Any(x => x.buttonSouth.wasReleasedThisFrame)
            //     || (keyboard != null && keyboard.enterKey.wasReleasedThisFrame))
            // {
            //     AkSoundEngine.PostEvent("menu_ok", gameObject);
            //     buttons[selectedButton].onClick.Invoke();
            // }

            // if (NextSelection())
            // {
            //     AkSoundEngine.PostEvent("menu_navigation", gameObject);
            //     selectedButton = (selectedButton + 1) % buttons.Length;
            //     StartCoroutine(WaitBeforeInput(timeBetweenButtons));
            // }
            // else if (PreviousSelection())
            // {
            //     AkSoundEngine.PostEvent("menu_navigation", gameObject);
            //     selectedButton--;
            //     if (selectedButton < 0)
            //     {
            //         selectedButton = buttons.Length - 1;
            //     }
            //     StartCoroutine(WaitBeforeInput(timeBetweenButtons));
            // }
        }
	}

    // private bool NextSelection()
    // {
    //     if (horizontalMenu)
    //     {
    //         return (Gamepad.all.Any(x => x.leftStick.ReadValue().x > 0.75F) || (keyboard != null && keyboard.dKey.wasReleasedThisFrame));
    //     }
    //     else
    //     {
    //         return (Gamepad.all.Any(x => x.leftStick.ReadValue().y < -0.75F) || (keyboard != null && keyboard.sKey.wasReleasedThisFrame));
    //     }
    // }

    // private bool PreviousSelection()
    // {
    //     if (horizontalMenu)
    //     {
    //         return (Gamepad.all.Any(x => x.leftStick.ReadValue().x < -0.75F) || (keyboard != null && keyboard.aKey.wasReleasedThisFrame));
    //     }
    //     else
    //     {
    //         return (Gamepad.all.Any(x => x.leftStick.ReadValue().y > 0.75F) || (keyboard != null && keyboard.wKey.wasReleasedThisFrame));
    //     }
    // }    
}
