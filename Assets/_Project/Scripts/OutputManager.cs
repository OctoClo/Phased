using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public static class OutputManager
{
    public static void Vibrate(Gamepad gamepad, float vibrationLeft, float vibrationRight)
    {
        gamepad.SetMotorSpeeds(vibrationLeft, vibrationRight);
    }

    public static void VibrateAll(float vibrationLeft, float vibrationRight)
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            Vibrate(gamepad, vibrationLeft, vibrationRight);
        }
    }
}
