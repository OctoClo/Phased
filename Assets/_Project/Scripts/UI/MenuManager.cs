﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartedEvent : GameEvent { }

public class GamePausedEvent : GameEvent { }

public class GameResumedEvent : GameEvent { }

public class GameBackEvent : GameEvent { }

public class GameLeaderboardEvent : GameEvent { }

public class GameCreditsEvent : GameEvent { }

public class GameControlsEvent : GameEvent { }

public class GameQuitAskEvent : GameEvent { }

public class GameQuitConfirmEvent : GameEvent { }

public class GameQuitCancelEvent : GameEvent { }

public class GameMainMenuEvent : GameEvent { }

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

    public void BackButton()
    {
        EventManager.Instance.Raise(new GameBackEvent());
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

    public void LeaderboardButton()
    {
        EventManager.Instance.Raise(new GameLeaderboardEvent());
    }

    public void CreditsButton()
    {
        EventManager.Instance.Raise(new GameCreditsEvent());
    }

    public void ControlsButton()
    {
        EventManager.Instance.Raise(new GameControlsEvent());
    }

    public void ReturnToMainMenuButton()
    {
        EventManager.Instance.Raise(new GameMainMenuEvent());
    }
}
