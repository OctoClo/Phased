using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatePhase { NO_PHASE, PRE_PHASE, PHASE };

public class PhasingManager : MonoBehaviour
{
    [Header("Phasing Bar")]
    public Bar PhasingBar;
    public float TotalPhasingThreshold = 80f;
    public float PhasingDecreaseSpeed = 5f;
    public float PhasingIncreaseSpeed = 5f;
    public float PhasingBoostKill = 1;

    [Header("PrePhase")]
    public float PrePhaseTriggerDist = 18.0f;

    [Header("Score Multiplicator")]
    public uint[] MultiplicatorValues = { 1, 2, 4 };
    public Color[] MultiplicatorColors = new Color[3];
    public UIManager UIManager;
    public PopupManager PopupManager;

    [Header("VFX")]
    public GameObject PhasingVFX;
    public GameObject PhasingExplosionVFX;

    [Header("Misc")]
    public List<Spaceship> Spaceships = new List<Spaceship>();

    EStatePhase phaseState;

    float distBetweenShips;

    bool gameActive = false;

    void Start()
    {
        GameScore.PhasingManager = this;
        PhasingBar.SetSeparator(TotalPhasingThreshold);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.RemoveListener<GameEndEvent>(OnGameEndEvent);
    }

    void Initialize()
    {
        AkSoundEngine.SetState("sync_level", "layer_1");
        PhasingBar.Value = 0;
        phaseState = EStatePhase.NO_PHASE;
        HandleStateChange();
    }

    void Update()
    {
        if (gameActive)
        {
            distBetweenShips = Vector3.Distance(Spaceships[0].transform.position, Spaceships[1].transform.position);

            UpdatePhasingValue();
            CheckForStateChange();
            UpdateVFXPositions();
        }
    }

    void UpdatePhasingValue()
    {
        if (phaseState == EStatePhase.NO_PHASE)
        {
            PhasingBar.Value -= PhasingDecreaseSpeed * Time.deltaTime;
        }
        else
        {
            PhasingBar.Value += PhasingIncreaseSpeed * Time.deltaTime;
        }
    }

    public void AddBoostKill()
    {
        if (phaseState != EStatePhase.NO_PHASE)
        {
            PhasingBar.Value += PhasingBoostKill;
        }
    }

    void CheckForStateChange()
    {
        EStatePhase newStateId;

        if (distBetweenShips > PrePhaseTriggerDist)
        {
            newStateId = EStatePhase.NO_PHASE;
        }
        else if (PhasingBar.Value >= TotalPhasingThreshold)
        {
            newStateId = EStatePhase.PHASE;
        }
        else
        {
            newStateId = EStatePhase.PRE_PHASE;
        }

        if (!phaseState.Equals(newStateId))
        {
            if (phaseState == EStatePhase.NO_PHASE)
            {
                NotifySpaceships(true);
            }
            else if (newStateId == EStatePhase.NO_PHASE)
            {
                NotifySpaceships(false);
            }

            phaseState = newStateId;
            HandleStateChange();
        }
    }

    void HandleStateChange()
    {
        switch (phaseState)
        {
            case EStatePhase.NO_PHASE:
                PhasingBar.SetPhasingState(false);
                SetSpaceshipsGlow(1f, false);
                PhasingVFX.SetActive(false);
                PhasingExplosionVFX.SetActive(false);
                AkSoundEngine.SetState("sync_level", "layer_1");
                break;

            case EStatePhase.PRE_PHASE:
                PhasingBar.SetPhasingState(true);
                PhasingBar.SetPhasedState(false);
                SetSpaceshipsGlow(2.5f, true);
                PhasingVFX.SetActive(true);
                PhasingVFX.GetComponent<ParticleSystem>().Play();
                PhasingExplosionVFX.SetActive(false);
                AkSoundEngine.SetState("sync_level", "layer_2");
                break;

            case EStatePhase.PHASE:
                PhasingBar.SetPhasedState(true);
                SetSpaceshipsGlow(3.5f, true);
                PhasingVFX.SetActive(true);
                PhasingExplosionVFX.SetActive(true);
                PhasingVFX.GetComponentInChildren<ParticleSystem>().Play();
                AkSoundEngine.SetState("sync_level", "layer_3");
                break;
        }

        GameScore.Multiplicator = MultiplicatorValues[(int)phaseState];

        if (PopupManager.Popups)
        {
            PopupManager.PopupColor = MultiplicatorColors[(int)phaseState];
            UIManager.UpdateColor(MultiplicatorColors[(int)phaseState]);
        }
        
        SetSpaceshipsWeapon(phaseState);
    }

    void UpdateVFXPositions()
    {
        Vector3 middlePos = Spaceships[0].transform.position + (Spaceships[1].transform.position - Spaceships[0].transform.position) / 2;
        PhasingVFX.transform.position = middlePos + new Vector3(0.5f, 3, -3);
    }

    void NotifySpaceships(bool phased)
    {
        Spaceships[0].SetPhased(phased);
        Spaceships[1].SetPhased(phased);
    }

    void SetSpaceshipsWeapon(EStatePhase state)
    {
        Spaceships[0].SetWeapon(state);
        Spaceships[1].SetWeapon(state);
    }

    void SetSpaceshipsGlow(float value, bool instantly)
    {
        Spaceships[0].SetGlowIntensity(value, instantly);
        Spaceships[1].SetGlowIntensity(value - 0.5f, instantly);
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        gameActive = true;
        Initialize();
    }

    void OnGameEndEvent(GameEndEvent e)
    {
        gameActive = false;
        PhasingVFX.SetActive(false);
        PhasingExplosionVFX.SetActive(false);
    }
}
