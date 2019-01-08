using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public static class OutputManager
{
    public static IEnumerator Vibrate(Gamepad gamepad, float vibrationLeft, float vibrationRight, float duration)
    {
        gamepad.SetMotorSpeeds(vibrationLeft, vibrationRight);
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0.0f, 0.0f);
    }

    public static IEnumerator VibrateAll(float duration)
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            gamepad.SetMotorSpeeds(0.5f, 0.5f);
            yield return new WaitForSeconds(duration);
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    public static void ResetVibrationAll()
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            gamepad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }
}
