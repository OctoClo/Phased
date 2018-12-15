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
    public Vector2[] direction = new Vector2[2];
    public Vector2[] target = new Vector2[2];

    public List<Spaceship> spaceships = new List<Spaceship>();

    // Args
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
        float normalizedFactor = Mathf.Clamp01( 1.0f - ( Mathf.Abs( playerDistance ) / ( distanceTrigger * distanceTrigger ) ) ) - 0.8f;
        
        var gamepad = InputSystem.GetDevice<Gamepad>();
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(normalizedFactor, 0.0f);
        }
    }

    void Update()
    {
        var distance = Vector3.Distance( spaceships[0].transform.position, spaceships[1].transform.position );
 
        SetPlayerPhaseFeedback(distance);
    }

    void OnEnable()
    {
        inputMap = inputAsset.GetActionMap("DefaultControls");
    
        // Bind Action
        inputMap.GetAction("Fire_0").performed += ctx => Fire( "0" );
        inputMap.GetAction("Fire_1").performed += ctx => Fire( "1" );

        inputMap.GetAction("Movement_0").performed += ctx => direction[0] = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Movement_0").cancelled += ctx => direction[0] = new Vector2();

        inputMap.GetAction("Movement_1").performed += ctx => direction[1] = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Movement_1").cancelled += ctx => direction[1] = new Vector2();

        inputMap.GetAction("Target_0").performed += ctx => target[0] = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Target_0").cancelled += ctx => target[0] = new Vector2();

        inputMap.GetAction("Target_1").performed += ctx => target[1] = ctx.ReadValue<Vector2>();
        inputMap.GetAction("Target_1").cancelled += ctx => target[1] = new Vector2();

        inputMap.Enable();
    }

    void OnDisable()
    {
        inputMap.Disable();
    }
}
