using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public static class OutputManager
{

    public static void Vibrate(Player RewiredPlayer, float vibrationLeft, float vibrationRight, float duration)
    {
        if (vibrationLeft > 0) RewiredPlayer.SetVibration(0, vibrationLeft, duration);
        if (vibrationRight > 0) RewiredPlayer.SetVibration(1, vibrationRight, duration);
    }

    public static void VibrateAll(float duration)
    {
        foreach (Joystick joystick in ReInput.controllers.Joysticks)
        {
            joystick.SetVibration(0, 0.5f, duration);
            joystick.SetVibration(1, 0.5f, duration);
        }
    }

    public static void ResetVibrationAll()
    {
        foreach (Joystick joystick in ReInput.controllers.Joysticks)
        {
            joystick.SetVibration(0.0f, 0.0f);
            joystick.SetVibration(0.0f, 0.0f);
        }
    }
}
