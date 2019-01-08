using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Controls;

public class InputManager : MonoBehaviour
{
    // Handling gamepads connection
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

    int playerCount = 0;
    bool[] connectedPlayers;
    List<ConnectedPlayer> connectedPlayersInstance;
    List<ConnectedGamepad> connectedGamepads;
    
    public List<Spaceship> Spaceships = new List<Spaceship>();

    void Update()
    {
        // Check connected gamepads
        for (int j = 0; j < connectedGamepads.Count; j++)
        {
            Gamepad gamepad = connectedGamepads[j].DeviceInstance as Gamepad;
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
                            Spaceship = Spaceships[i],
                            Gamepad = connectedGamepads[j]
                        };

                        connectedPlayersInstance.Add(player);
                        break;
                    }
                }

                playerCount++;
            }
        }

        // Check if a keyboard if needed
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        {
            if (!connectedPlayers[i])
            {
                Debug.Log("Added keyboard input");

                InputDevice keyboard = InputSystem.GetDevice<Keyboard>();

                connectedPlayers[i] = true;

                ConnectedPlayer player = new ConnectedPlayer
                {
                    Spaceship = Spaceships[i],
                    Gamepad = null
                };

                connectedPlayersInstance.Add(player);
            }
        }

        // Check inputs
        CheckInput();
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
                Debug.Log("Lost Gamepad!");

                int index = connectedGamepads.FindIndex(x => (x.DeviceInstance == (device as Gamepad)));
                if (index >= 0)
                {
                    playerCount--;
                    connectedPlayers[index] = false;
                    connectedGamepads.RemoveAt(index);

                    connectedPlayersInstance.RemoveAt(connectedPlayersInstance.FindIndex(x => x.Gamepad.DeviceInstance == device));
                }
            }
            else if (change == InputDeviceChange.Added)
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

    void CheckInput()
    {
        List<Vector2> gamepadsDirections = new List<Vector2>();

        foreach (ConnectedPlayer connectedPlayer in connectedPlayersInstance)
        {
            if (connectedPlayer.Gamepad != null)
            {
                var gamepad = connectedPlayer.Gamepad.DeviceInstance as Gamepad;
                if (gamepad != null)
                {
                    connectedPlayer.Spaceship.Direction = gamepad.leftStick.ReadValue();

                    connectedPlayer.Spaceship.Target = gamepad.rightStick.ReadValue();
                    connectedPlayer.Spaceship.IsFiring = gamepad.rightTrigger.isPressed;
                }
            }
            else
            {
                // TODO Assume a keyboard and a mouse are plugged in...
                Keyboard keyboardInstance = InputSystem.GetDevice<Keyboard>();
                Mouse mouseInstance = InputSystem.GetDevice<Mouse>();

                float x = 0.0f, y = 0.0f;
                if (keyboardInstance.wKey.isPressed) y += +1.0f;
                if (keyboardInstance.sKey.isPressed) y += -1.0f;

                if (keyboardInstance.aKey.isPressed) x += -1.0f;
                if (keyboardInstance.dKey.isPressed) x += +1.0f;
                
                connectedPlayer.Spaceship.Direction.x = x;
                connectedPlayer.Spaceship.Direction.y = y;

                // Normalize cursor coordinates (-1..1 range)
                Vector2 screenDimensions = new Vector2(Screen.width, Screen.height);
                Vector2 mouseAbsoluteCoords = mouseInstance.position.ReadValue();

                connectedPlayer.Spaceship.Target = mouseAbsoluteCoords / screenDimensions * new Vector2(2.0f, 2.0f) - new Vector2(1.0f, 1.0f);
                connectedPlayer.Spaceship.IsFiring = mouseInstance.leftButton.isPressed;
            }
        }
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
        OutputManager.ResetVibrationAll();
    }
}
