using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAlmostOverEvent : GameEvent { }

public class LifeCounter : Singleton<LifeCounter>
{
    public int LifeCount = 5;
    public Animator ScreenglowAnimator;
    public InputManager inputManager;
    

    int initialLifeCount;

    public bool IsInvulnerable = false;
    
    public Spaceship DamageSource { get; private set; }

    void Start()
    {
        initialLifeCount = LifeCount;
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStartedEvent>(OnGameStartedEvent);
    }

    void Initialize()
    {
        LifeCount = initialLifeCount;
    }

    public void RemoveLife(Spaceship other)
    {
        DamageSource = other;

        LifeCount--;

        if (LifeCount == 0)
        {

            StartCoroutine(GameOver());
        }
        else
        {
            StartCoroutine(WaitBeforeEndInvulnerability(WorldConstants.Instance.PlayerInvulnerabilityDuration));
            other.PlayLoseLifeVFX();
            ScreenShake.Instance.Shake(WorldConstants.Instance.ScreenShakeHitDuration, WorldConstants.Instance.ScreenShakeHitIntensity);

            if (LifeCount == 1)
            {
                ScreenglowAnimator.SetBool("Low Health", true);
            }
            else
            {
                ScreenglowAnimator.SetTrigger("Hit");
            }
        }
    }

    IEnumerator GameOver()
    {
        EventManager.Instance.Raise(new GameAlmostOverEvent());
        ScreenShake.Instance.Shake(WorldConstants.Instance.ScreenShakeDeathDuration, WorldConstants.Instance.ScreenShakeDeathIntensity);

        yield return new WaitUntil(SpaceshipsManager.Instance.HasDeathFXFinished);
        
        EventManager.Instance.Raise(new GameEndEvent() { Victorious = false });
    }

    IEnumerator WaitBeforeEndInvulnerability(float duration)
    {
        IsInvulnerable = true;
        yield return new WaitForSecondsRealtime(duration);
        IsInvulnerable = false;
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        Initialize();
    }
}
