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
        public Player RewiredPlayer;
        public Spaceship Spaceship;
    };

    int playerCount = 0;
    bool[] connectedPlayers;
    List<ConnectedPlayer> connectedPlayersInstance;
    
    public List<Spaceship> Spaceships = new List<Spaceship>();

    void Update()
    {

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
        connectedPlayers = new bool[MAX_PLAYER_COUNT];
    }

    void OnDisable()
    {
        OutputManager.ResetVibrationAll();
    }
}
