using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

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

	private float inputTimer;
	private float initialTimer;
    private float timeBetweenButtons = 0.2f;

    EventSystem eventSystem;
    Keyboard keyboard;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    private void OnEnable()
    {
        if (!eventSystem)
            eventSystem = EventSystem.current;

        selectedButton = 0;
        lastSelectedButton = -1;
        initialTimer = blockInputTime;
    }

    private void OnDisable()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    private void Update()
	{
        keyboard = InputSystem.GetDevice<Keyboard>();

        if (initialTimer > 0)
		{
			initialTimer -= Time.deltaTime;
			return;
		}

		inputTimer -= Time.deltaTime;

		if (lastSelectedButton != selectedButton)
		{
            buttons[selectedButton].Select();
            lastSelectedButton = selectedButton;
        }

		if (Gamepad.all.Any(x => x.buttonSouth.wasReleasedThisFrame)
            || (keyboard != null && keyboard.enterKey.wasReleasedThisFrame))
        {
            AkSoundEngine.PostEvent("menu_ok", gameObject);
            buttons[selectedButton].onClick.Invoke();
		}

        if (NextSelection() && inputTimer <= 0)
		{
            AkSoundEngine.PostEvent("menu_navigation", gameObject);

			inputTimer = timeBetweenButtons;
			selectedButton = (selectedButton + 1) % buttons.Length;
        }

		if (PreviousSelection() && inputTimer <= 0)
		{
            AkSoundEngine.PostEvent("menu_navigation", gameObject);

			inputTimer = timeBetweenButtons;
			selectedButton--;

			if (selectedButton < 0)
			{
				selectedButton = buttons.Length - 1;
			}
        }
	}

    private bool NextSelection()
    {
        if (horizontalMenu)
        {
            return (Gamepad.all.Any(x => x.leftStick.ReadValue().x > 0.75F) || (keyboard != null && keyboard.dKey.wasReleasedThisFrame));
        }
        else
        {
            return (Gamepad.all.Any(x => x.leftStick.ReadValue().y < -0.75F) || (keyboard != null && keyboard.sKey.wasReleasedThisFrame));
        }
    }

    private bool PreviousSelection()
    {
        if (horizontalMenu)
        {
            return (Gamepad.all.Any(x => x.leftStick.ReadValue().x < -0.75F) || (keyboard != null && keyboard.aKey.wasReleasedThisFrame));
        }
        else
        {
            return (Gamepad.all.Any(x => x.leftStick.ReadValue().y > 0.75F) || (keyboard != null && keyboard.wKey.wasReleasedThisFrame));
        }
    }    
}
