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
    public Vector2 direction0;
    public Vector2 direction1;

    public Vector2 target0;
    public Vector2 target1;

    // Args
    public float distanceTest;
    public float distanceTrigger = 4.0f;

    void Fire( string userIndex )
    {
        var gamepad = InputSystem.GetDevice<Gamepad>();
        if ( gamepad != null )
        {
            gamepad.SetMotorSpeeds(0.05f, 0.10f);
        }

        Debug.Log("Fire callback " + userIndex );

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
        inputMap.GetAction("Fire_0").performed += ctx => Fire( "0" );
        inputMap.GetAction("Fire_1").performed += ctx => Fire( "1" );

        inputMap.GetAction("Movement_0").performed += ctx => direction0 = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Movement_0").cancelled += ctx => direction0 = new Vector2();

        inputMap.GetAction("Movement_1").performed += ctx => direction1 = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Movement_1").cancelled += ctx => direction1 = new Vector2();

        inputMap.GetAction("Target_0").performed += ctx => target0 = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Target_0").cancelled += ctx => target0 = new Vector2();

        inputMap.GetAction("Target_1").performed += ctx => target1 = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Target_1").cancelled += ctx => target1 = new Vector2();

        inputMap.Enable();
    }

    void OnDisable()
    {
        inputMap.Disable();
    }
}
