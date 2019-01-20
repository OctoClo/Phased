using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

public class InputField : MonoBehaviour
{
    public Leaderboard Leaderboard;

    bool needEventRegister;
    UnityEngine.UI.InputField inputField;
    float inputWait;

    static string[] TeamNames =
    {
        "aaaaaa",
        "mlgpro",
        "omgwtf",
        "assass",
        "weaboo",
        "poopoo",
        "pwned1",
        "haxxor",
        "newbie",
        "foobar"
    };

    static string RandomizeTeamName()
    {
        return TeamNames[Random.Range(0, TeamNames.Length)];
    }

    public void Initialize()
    {
        if (!inputField)
        {
            inputField = GetComponent<UnityEngine.UI.InputField>();
        }
        inputField.text = "";
        inputField.placeholder.GetComponent<Text>().text = RandomizeTeamName();
        inputWait = 0.1f;
        needEventRegister = true;
    }

    void Update()
    {
        inputWait -= Time.deltaTime;

        // The InputSystem is initialized at a late stage of the engine (I guess)
        // Which is why this crappy code is required...
        if ( needEventRegister )
        {
            Keyboard keyboardInstance = InputSystem.GetDevice<Keyboard>();
            if ( keyboardInstance != null )
            {
                keyboardInstance.onTextInput += KeyboardInstance_onTextInput;
                needEventRegister = false;

                Debug.Log("Keyboard registered for leaderboard");
            }
        }
    }

    private void KeyboardInstance_onTextInput(char obj)
    {
        if ( inputWait > 0.0f )
        {
            return;
        }

        //Debug.Log((int)obj);

        if (obj == 0x08 && inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
        }
        else if (obj == 0x0d && inputField.text.Length > 0)
        {
            InputSystem.GetDevice<Keyboard>().onTextInput -= KeyboardInstance_onTextInput;
            Leaderboard.SubmitScore();
        }
        else if (inputField.text.Length < 6)
        {
            inputField.text += obj;
        }
        
        inputWait = 0.1f;
    }
}
