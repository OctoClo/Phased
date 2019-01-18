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
    public uint PrePhaseScoreMultiplicator = 2;
    public uint PhaseScoreMultiplicator = 4;

    [Header("Players Link")]
    public GameObject PlayersLink;
    public List<Material> MaterialsPlayersLink = new List<Material>();

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
        phaseState = EStatePhase.NO_PHASE;
        AkSoundEngine.SetState("sync_level", "layer_1");
        PlayersLink.SetActive(false);
        PhasingBar.Value = 0;
    }

    void Update()
    {
        if (gameActive)
        {
            distBetweenShips = Vector3.Distance(Spaceships[0].transform.position, Spaceships[1].transform.position);

            UpdatePhasingValue();
            //UpdatePlayerLink();
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

    void UpdatePlayerLink()
    {
        PlayersLink.transform.position = (Spaceships[0].transform.position + Spaceships[1].transform.position) / 2;

        float angle = Mathf.Atan2(Spaceships[0].transform.position.z - Spaceships[1].transform.position.z, Spaceships[0].transform.position.x - Spaceships[1].transform.position.x) * Mathf.Rad2Deg;

        PlayersLink.transform.rotation = Quaternion.Euler(0, angle * -1, 0);

        PlayersLink.transform.localScale = new Vector3(distBetweenShips - 4, 0.25f, 0.25f);

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
        uint scoreMultiplicator = 1;

        switch (phaseState)
        {
            case EStatePhase.NO_PHASE:
                //PlayersLink.SetActive(false);
                PhasingBar.SetPhasingState(false);
                SetSpaceshipsGlow(1f, false);
                PhasingVFX.SetActive(false);
                PhasingExplosionVFX.SetActive(false);
                AkSoundEngine.SetState("sync_level", "layer_1");
                break;

            case EStatePhase.PRE_PHASE:
                //PlayersLink.SetActive(true);
                PhasingBar.SetPhasingState(true);
                PhasingBar.SetPhasedState(false);
                SetSpaceshipsGlow(2.5f, true);
                scoreMultiplicator = PrePhaseScoreMultiplicator;
                PhasingVFX.SetActive(true);
                PhasingVFX.GetComponent<ParticleSystem>().Play();
                PhasingExplosionVFX.SetActive(false);
                AkSoundEngine.SetState("sync_level", "layer_2");
                break;

            case EStatePhase.PHASE:
                //PlayersLink.SetActive(true);
                PhasingBar.SetPhasedState(true);
                SetSpaceshipsGlow(3.5f, true);
                scoreMultiplicator = PhaseScoreMultiplicator;
                PhasingVFX.SetActive(true);
                PhasingExplosionVFX.SetActive(true);
                PhasingVFX.GetComponentInChildren<ParticleSystem>().Play();
                AkSoundEngine.SetState("sync_level", "layer_3");
                break;
        }

        GameScore.Multiplicator = scoreMultiplicator;

        PlayersLink.GetComponent<MeshRenderer>().material = MaterialsPlayersLink[(int)phaseState];
        SetSpaceshipsWeapon(phaseState);
    }

    void UpdateVFXPositions()
    {
        Vector3 middlePos = Spaceships[0].transform.position + (Spaceships[1].transform.position - Spaceships[0].transform.position) / 2;
        PhasingVFX.transform.position = middlePos + new Vector3(0.5f, 3, -3);
        PhasingExplosionVFX.transform.position = middlePos + new Vector3(0, 0, 1000);
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
        Spaceships[1].SetGlowIntensity(value + 1f, instantly);
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
