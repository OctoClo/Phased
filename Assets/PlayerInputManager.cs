using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Controls;

public class PlayerInputManager : MonoBehaviour
{
    public InputActionAsset inputAsset;

    InputActionMap inputMap;

    // Output
    public Vector2 direction;
    public Vector2 target;

    public float distanceTest;
    public float distanceTrigger = 4.0f;

    void Fire()
    {
        var gamepad = InputSystem.GetDevice<Gamepad>();
        if ( gamepad != null )
        {
            gamepad.SetMotorSpeeds(0.05f, 0.10f);
        }

        Debug.Log("Fire callback");

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0.00f, 0.00f);
        }
    }

    void SetPlayerPhaseFeedback(float playerDistance)
    {
        var gamepad = InputSystem.GetDevice<Gamepad>();
        if (gamepad != null)
        {
            float normalizedFactor = Mathf.Clamp01( 1.0f - ( Mathf.Abs( playerDistance ) / ( distanceTrigger * distanceTrigger ) ) ) - 0.8f;

            Debug.Log(normalizedFactor);
            gamepad.SetMotorSpeeds(normalizedFactor, 0.0f);
        }
    }

    void Update()
    {
        SetPlayerPhaseFeedback(distanceTest);
    }

    void OnEnable()
    {
        inputMap = inputAsset.GetActionMap("DefaultControls");
    
        // Bind Action
        inputMap.GetAction("Fire").performed += ctx => Fire();

        inputMap.GetAction("Movement").performed += ctx => direction = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Movement").cancelled += ctx => direction = new Vector2();

        inputMap.GetAction("Target").performed += ctx => target = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Target").cancelled += ctx => target = new Vector2();

        inputMap.Enable();
    }

    void OnDisable()
    {
        inputMap.Disable();
    }
}
