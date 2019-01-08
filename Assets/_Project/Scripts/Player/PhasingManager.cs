using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatePhase { NO_PHASE, PRE_PHASE, PHASE };

public class PhasingManager : MonoBehaviour
{
    [Header("Phasing Bar")]
    public Bar PhasingBar;
    public float TotalPhasingThreshold = 80f;
    public float PhasingIncreaseSpeed = 5f;
    public float PhasingBoostKill = 1;

    [Header("PrePhase")]
    public float PrePhaseTriggerDist = 18.0f;

    [Header("Score Multiplicator")]
    public uint PrePhaseScoreMultiplicator = 2;
    public uint PhaseScoreMultiplicator = 4;

    [Header("Vibrations")]
    public float PrePhaseVibration = 0.05f;
    public float PrePhaseVibrationDuration = 0.1f;
    public float PhaseVibration = 0.1f;
    public float PhaseVibrationDuration = 0.1f;

    [Header("Players Link")]
    public GameObject PlayersLink;
    public List<Material> MaterialsPlayersLink = new List<Material>();

    [Header("Misc")]
    public List<Spaceship> Spaceships = new List<Spaceship>();

    EStatePhase phaseState = EStatePhase.NO_PHASE;

    float distBetweenShips = 0.0f;

    void Start()
    {
        GameScore.PhasingManager = this;
        PhasingBar.SetSeparator(TotalPhasingThreshold);
    }

    void Update()
    {
        distBetweenShips = Vector3.Distance(Spaceships[0].transform.position, Spaceships[1].transform.position);

        UpdatePhasingValue();
        UpdatePlayerLink();
        CheckForStateChange();
    }

    void UpdatePhasingValue()
    {
        if (phaseState == EStatePhase.NO_PHASE)
        {
            PhasingBar.Value -= PhasingIncreaseSpeed * Time.deltaTime;
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
                PlayersLink.SetActive(false);
                //StartCoroutine(OutputManager.VibrateAll(PrePhaseVibration, PrePhaseVibration, PrePhaseVibrationDuration));
                break;

            case EStatePhase.PRE_PHASE:
                PlayersLink.SetActive(true);
                scoreMultiplicator = PrePhaseScoreMultiplicator;
                //StartCoroutine(OutputManager.VibrateAll(PrePhaseVibration, PrePhaseVibration, PrePhaseVibrationDuration));
                break;

            case EStatePhase.PHASE:
                PlayersLink.SetActive(true);
                scoreMultiplicator = PhaseScoreMultiplicator;
                //StartCoroutine(OutputManager.VibrateAll(PhaseVibration, PhaseVibration, PhaseVibrationDuration));
                break;
        }

        GameScore.Multiplicator = scoreMultiplicator;

        PlayersLink.GetComponent<MeshRenderer>().material = MaterialsPlayersLink[(int)phaseState];
        SetSpaceshipsWeapon(phaseState);
    }

    void SetSpaceshipsWeapon(EStatePhase state)
    {
        Spaceships[0].SetWeapon(state);
        Spaceships[1].SetWeapon(state);
    }
}
