using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Controls;

public class PlayerInputManager : MonoBehaviour
{
    const int MAX_PLAYER_COUNT = 2;

    class ConnectedGamepad
    {
        public bool IsInUse;
        public InputDevice DeviceInstance;
    };

    class ConnectedPlayer
    {
        public ConnectedGamepad Gamepad;
        public Spaceship Spaceship;
    };

    public InputActionAsset inputAsset;

    InputActionMap inputMap;

    private int playerCount = 0;
    
    private List<ConnectedGamepad> connectedGamepads;
    private bool[] connectedPlayers;
    private List<ConnectedPlayer> connectedPlayersInstance;

    public List<Spaceship> spaceships = new List<Spaceship>();
    
    // Args
    public float distanceTrigger = 4.0f;

    void SetPlayerPhaseFeedback(float playerDistance)
    {
        float normalizedFactor = Mathf.Clamp01( 1.0f - ( Mathf.Abs( playerDistance ) / ( distanceTrigger * distanceTrigger ) ) ) - 0.8f;

        foreach (var connectedPlayer in connectedPlayersInstance)
        {
            if (connectedPlayer.Gamepad == null)
            {
                continue;
            }

            var gamepad = connectedPlayer.Gamepad.DeviceInstance as Gamepad;

            if (gamepad != null)
            {
                gamepad.SetMotorSpeeds(normalizedFactor, 0.0f);
            }
        }
    }

    void Update()
    {
        //if (true)
        //{
            //Debug.Log("needGamepadRescan");
            for (int j = 0; j < connectedGamepads.Count; j++)
            {
                var gamepad = connectedGamepads[j].DeviceInstance as Gamepad;
                if (!connectedGamepads[j].IsInUse)
                {
                    for (int i = 0; i < MAX_PLAYER_COUNT; i++)
                    {
                        if (!connectedPlayers[i] || connectedPlayersInstance[i].Gamepad == null)
                        {
                            connectedGamepads[j].IsInUse = true;
                            connectedPlayers[i] = true;

                            ConnectedPlayer player = new ConnectedPlayer
                            {
                                Spaceship = spaceships[i],
                                Gamepad = connectedGamepads[j]
                            };

                            connectedPlayersInstance.Add(player);
                            break;
                        }
                    }

                    playerCount++;
                }
            }

            for (int i = 0; i < MAX_PLAYER_COUNT; i++)
            {
                if (!connectedPlayers[i])
                {
                    Debug.Log("Added keyboard input");

                    InputDevice keyboard = InputSystem.GetDevice<Keyboard>();
                
                    connectedPlayers[i] = true;

                    ConnectedPlayer player = new ConnectedPlayer
                    {
                        Spaceship = spaceships[i],
                        Gamepad = null
                    };

                    connectedPlayersInstance.Add(player);
                }
            }
        //}

        foreach (var connectedPlayer in connectedPlayersInstance)
        {
            if (connectedPlayer.Gamepad != null)
            {
                var gamepad = connectedPlayer.Gamepad.DeviceInstance as Gamepad;
                if (gamepad != null)
                {
                    connectedPlayer.Spaceship.direction = gamepad.leftStick.ReadValue();
                    connectedPlayer.Spaceship.target = gamepad.rightStick.ReadValue();

                    if (gamepad.rightTrigger.isPressed)
                    {
                        gamepad.SetMotorSpeeds(0.05f, 0.10f);
                    }
                    else
                    {
                        gamepad.SetMotorSpeeds(0.0f, 0.0f);
                    }
                }
            }
            else
            {
                // TODO Assume a keyboard and mouse are plugged in...
                var keyboardInstance = InputSystem.GetDevice<Keyboard>();
                var mouseInstance = InputSystem.GetDevice<Mouse>();

                float x = 0.0f, y = 0.0f;
                if (keyboardInstance.wKey.isPressed) y += +1.0f;
                if (keyboardInstance.sKey.isPressed) y += -1.0f;

                if (keyboardInstance.aKey.isPressed) x += -1.0f;
                if (keyboardInstance.dKey.isPressed) x += +1.0f;

                connectedPlayer.Spaceship.direction.x = x;
                connectedPlayer.Spaceship.direction.y = y;

                // Normalize cursor coordinates (-1..1 range)
                var screenDimensions = new Vector2(Screen.width, Screen.height);
                var mouseAbsoluteCoords = mouseInstance.position.ReadValue();
                               
                connectedPlayer.Spaceship.target = mouseAbsoluteCoords / screenDimensions * new Vector2( 2.0f, 2.0f ) - new Vector2( 1.0f, 1.0f );
                
                if (mouseInstance.leftButton.isPressed)
                {
                    // Fire
                }
            }
        }

        var distance = Vector3.Distance(spaceships[0].transform.position, spaceships[1].transform.position);
        SetPlayerPhaseFeedback(distance);
    }

    private void Awake()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (!(device is Gamepad))
            {
                return;
            }

            if (change == InputDeviceChange.Disconnected || change == InputDeviceChange.Removed)
            {
                int index = connectedGamepads.FindIndex( x => ( x.DeviceInstance == ( device as Gamepad ) ) );
                if (index >= 0)
                {
                    playerCount--;
                    connectedPlayers[index] = false;
                    connectedGamepads.RemoveAt(index);

                    connectedPlayersInstance.RemoveAt( connectedPlayersInstance.FindIndex( x => x.Gamepad.DeviceInstance == device ) );
                }
            }
            else if ( change == InputDeviceChange.Added )
            {
                Debug.Log("New Gamepad Detected!");

                ConnectedGamepad connectedGamepad = new ConnectedGamepad
                {
                    IsInUse = false,
                    DeviceInstance = (device as Gamepad)
                };

                connectedGamepads.Add(connectedGamepad);
            }
        };
    }

    void OnEnable()
    {
        connectedGamepads = new List<ConnectedGamepad>();
        connectedPlayersInstance = new List<ConnectedPlayer>();
        playerCount = 0;
        connectedPlayers = new bool[MAX_PLAYER_COUNT];
    }

    void OnDisable()
    {
        // Stop vibrations
        foreach (Gamepad g in Gamepad.all)
        {
            g.SetMotorSpeeds(0.0F, 0.0F);
        }
    }
}
