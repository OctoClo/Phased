using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour
{
    // Handling gamepads connection
    const int MAX_PLAYER_COUNT = 2;

    class ConnectedPlayer
    {
        //public ConnectedGamepad Gamepad;
        public Player RewiredPlayer;
        public Spaceship Spaceship;
    };

    int playerCount = 0;
    bool[] connectedPlayers;
    List<ConnectedPlayer> connectedPlayersInstance;
    // List<ConnectedGamepad> connectedGamepads;
    
    public List<Spaceship> Spaceships = new List<Spaceship>();

    void Update()
    {
        // Check connected gamepads
        //TODO: Remove to clean
        // for (int j = 0; j < connectedGamepads.Count; j++)
        // {
        //     Gamepad gamepad = connectedGamepads[j].DeviceInstance as Gamepad;
        //     if (!connectedGamepads[j].IsInUse)
        //     {
        //         for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        //         {
        //             if (!connectedPlayers[i] || connectedPlayersInstance[i].Gamepad == null)
        //             {
        //                 connectedGamepads[j].IsInUse = true;
        //                 connectedPlayers[i] = true;

        //                 ConnectedPlayer player = new ConnectedPlayer
        //                 {
        //                     Spaceship = Spaceships[i],
        //                     Gamepad = connectedGamepads[j]
        //                 };

        //                 connectedPlayersInstance.Add(player);
        //                 break;
        //             }
        //         }

        //         playerCount++;
        //     }
        // }

        // Check if a keyboard if needed
        // for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        // {
        //     if (!connectedPlayers[i])
        //     {
        //         Debug.Log("Added keyboard input");

        //         InputDevice keyboard = InputSystem.GetDevice<Keyboard>();

        //         connectedPlayers[i] = true;

        //         ConnectedPlayer player = new ConnectedPlayer
        //         {
        //             Spaceship = Spaceships[i],
        //             Gamepad = null
        //         };

        //         connectedPlayersInstance.Add(player);
        //     }
        // }

        // Check inputs
        CheckInput();
    }

    private void Awake()
    {

        //Joystick events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

        connectedPlayersInstance = new List<ConnectedPlayer>();

        //Assign RewiredPlayers
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        {
            ConnectedPlayer player = new ConnectedPlayer
            {
                Spaceship = Spaceships[i],
                RewiredPlayer = ReInput.players.GetPlayer(i)
            };

            connectedPlayersInstance.Add(player);
            playerCount++;
        }

        //TODO: Remove to clean
        // InputSystem.onDeviceChange += (device, change) =>
        // {
        //     if (!(device is Gamepad))
        //     {
        //         return;
        //     }

        //     if (change == InputDeviceChange.Disconnected || change == InputDeviceChange.Removed)
        //     {
        //         Debug.Log("Lost Gamepad!");

        //         int index = connectedGamepads.FindIndex(x => (x.DeviceInstance == (device as Gamepad)));
        //         if (index >= 0)
        //         {
        //             playerCount--;
        //             connectedPlayers[index] = false;
        //             connectedGamepads.RemoveAt(index);

        //             connectedPlayersInstance.RemoveAt(connectedPlayersInstance.FindIndex(x => x.Gamepad.DeviceInstance == device));
        //         }
        //     }
        //     else if (change == InputDeviceChange.Added)
        //     {
        //         Debug.Log("New Gamepad Detected!");

        //         ConnectedGamepad connectedGamepad = new ConnectedGamepad
        //         {
        //             IsInUse = false,
        //             DeviceInstance = (device as Gamepad)
        //         };

        //         connectedGamepads.Add(connectedGamepad);
        //     }
        // };
    }

    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        
        //If controller is joystick and one player doesn't have a joystick
        //
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    void CheckInput()
    {
        //TODO: Remove to clean
        // List<Vector2> gamepadsDirections = new List<Vector2>();

        // foreach (ConnectedPlayer connectedPlayer in connectedPlayersInstance)
        // {
        //     if (connectedPlayer.Gamepad != null)
        //     {
        //         var gamepad = connectedPlayer.Gamepad.DeviceInstance as Gamepad;
        //         if (gamepad != null)
        //         {
        //             connectedPlayer.Spaceship.Direction = gamepad.leftStick.ReadValue();

        //             connectedPlayer.Spaceship.Target = gamepad.rightStick.ReadValue();
        //             connectedPlayer.Spaceship.IsFiring = gamepad.rightTrigger.isPressed;
        //         }
        //     }
        //     else
        //     {
        //         // TODO Assume a keyboard and a mouse are plugged in...
        //         Keyboard keyboardInstance = InputSystem.GetDevice<Keyboard>();
        //         Mouse mouseInstance = InputSystem.GetDevice<Mouse>();

        //         float x = 0.0f, y = 0.0f;
        //         if (keyboardInstance.wKey.isPressed) y += +1.0f;
        //         if (keyboardInstance.sKey.isPressed) y += -1.0f;

        //         if (keyboardInstance.aKey.isPressed) x += -1.0f;
        //         if (keyboardInstance.dKey.isPressed) x += +1.0f;

        //         connectedPlayer.Spaceship.Direction.x = x;
        //         connectedPlayer.Spaceship.Direction.y = y;

        //         // Normalize cursor coordinates (-1..1 range)
        //         Vector2 screenDimensions = new Vector2(Screen.width, Screen.height);
        //         Vector2 mouseAbsoluteCoords = mouseInstance.position.ReadValue();

        //         connectedPlayer.Spaceship.Target = mouseAbsoluteCoords / screenDimensions * new Vector2(2.0f, 2.0f) - new Vector2(1.0f, 1.0f);
        //         connectedPlayer.Spaceship.IsFiring = mouseInstance.leftButton.isPressed;
        //     }
        // }

        foreach (ConnectedPlayer connectedPlayer in connectedPlayersInstance)
        {
            if (connectedPlayer.RewiredPlayer != null)
            {

                connectedPlayer.Spaceship.Direction = new Vector2(
                    connectedPlayer.RewiredPlayer.GetAxis("Move Horizontal"), 
                    connectedPlayer.RewiredPlayer.GetAxis("Move Vertical")
                    );

                connectedPlayer.Spaceship.Target = new Vector2(
                    connectedPlayer.RewiredPlayer.GetAxis("Aim Horizontal"),
                    connectedPlayer.RewiredPlayer.GetAxis("Aim Vertical")
                    );

                connectedPlayer.Spaceship.IsFiring = connectedPlayer.RewiredPlayer.GetButton("Fire");

                if(connectedPlayer.RewiredPlayer.controllers.hasMouse && connectedPlayer.RewiredPlayer.controllers.joystickCount < 1){
                    Vector2 screenDimensions = new Vector2(Screen.width, Screen.height);
                    connectedPlayer.Spaceship.Target = connectedPlayer.RewiredPlayer.controllers.Mouse.screenPosition / screenDimensions * new Vector2(2.0f, 2.0f) - new Vector2(1.0f, 1.0f);
                }
                
                
            }
        }
    }

    void OnEnable()
    {
        // connectedGamepads = new List<ConnectedGamepad>();
        //connectedPlayersInstance = new List<ConnectedPlayer>();
        //playerCount = 0;
        connectedPlayers = new bool[MAX_PLAYER_COUNT];
    }

    void OnDisable()
    {
        OutputManager.ResetVibrationAll();
    }
}
