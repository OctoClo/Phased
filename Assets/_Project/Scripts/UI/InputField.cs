using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

public class InputField : MonoBehaviour
{
    UnityEngine.UI.InputField inputField;
    float inputWait;
    bool needEventRegister;

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

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<UnityEngine.UI.InputField>();
        inputField.text = RandomizeTeamName();

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

                Debug.Log("registered1");
            }
        }
    }

    private void KeyboardInstance_onTextInput(char obj)
    {
        if ( inputWait > 0.0f )
        {
            Debug.Log(inputWait);
            return;
        }

        Debug.Log((int)obj);

        if (obj == 0x08 && inputField.text.Length > 0)
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
        else if ( inputField.text.Length < 6)
            inputField.text += obj;

        inputWait = 0.1f;
    }
}
