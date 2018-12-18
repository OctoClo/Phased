using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class OutputManager : MonoBehaviour
{
    [HideInInspector]
    public List<Gamepad> Gamepads = new List<Gamepad>();

    public void Vibrate(Gamepad gamepad, float vibrationLeft, float vibrationRight)
    {
        gamepad.SetMotorSpeeds(vibrationLeft, vibrationRight);
    }

    public void VibrateAll(float vibrationLeft, float vibrationRight)
    {
        foreach (Gamepad gamepad in Gamepads)
        {
            Vibrate(gamepad, vibrationLeft, vibrationRight);
        }
    }
}
