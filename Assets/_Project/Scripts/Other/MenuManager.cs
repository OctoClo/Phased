﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class GameStartedEvent : GameEvent { }

public class GameResumedEvent : GameEvent { }

public class GameQuitAskEvent : GameEvent { }

public class GameQuitConfirmEvent : GameEvent { }

public class GameQuitCancelEvent : GameEvent { }

public class GameEndEvent : GameEvent { public bool Victorious; }

public class MenuManager : MonoBehaviour
{
    public void StartButton()
    {
        EventManager.Instance.Raise(new GameStartedEvent());
    }

    public void ResumeButton()
    {
        EventManager.Instance.Raise(new GameResumedEvent());
    }

    public void QuitButton()
    {
        EventManager.Instance.Raise(new GameQuitAskEvent());
    }

    public void QuitYesButton()
    {
        EventManager.Instance.Raise(new GameQuitConfirmEvent());
    }

    public void QuitNoButton()
    {
        EventManager.Instance.Raise(new GameQuitCancelEvent());
    }

    public void RetryButton()
    {
        EventManager.Instance.Raise(new GameStartedEvent());
    }

    void Update()
    {
        Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

        if (keyboard != null && keyboard.pKey.wasReleasedThisFrame)
        {
            StartButton();
        }
    }
}
