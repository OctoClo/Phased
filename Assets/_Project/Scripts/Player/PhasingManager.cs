using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatePhase { NO_PHASE, PRE_PHASE, PHASE };

public class PhasingManager : MonoBehaviour
{
    public List<Spaceship> Spaceships = new List<Spaceship>();

    public List<Material> MaterialsPlayersLink = new List<Material>();
    public GameObject PlayersLink;
    public InputManager inputManager;
    public float PrePhaseTriggerDist = 18.0f;
    public float PhaseTriggerDist = 8.0f;

    public float PrePhaseVibration = 0.001f;
    public float PhaseVibration = 0.0005f;

    [SerializeField]
    EStatePhase state = EStatePhase.NO_PHASE;

    float distBetweenShips = 0.0f;

    void Update()
    {
        distBetweenShips = Vector3.Distance(Spaceships[0].transform.position, Spaceships[1].transform.position);

        UpdatePlayerLink();

        CheckForStateChange();
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
        EStatePhase newStateId = EStatePhase.NO_PHASE;

        if (distBetweenShips < PhaseTriggerDist)
            newStateId = EStatePhase.PHASE;
        else if (distBetweenShips < PrePhaseTriggerDist)
            newStateId = EStatePhase.PRE_PHASE;

        if (!state.Equals(newStateId))
        {
            state = newStateId;
            HandleStateChange();
        }
    }

    void HandleStateChange()
    {
        switch (state)
        {
            case EStatePhase.NO_PHASE:
                PlayersLink.SetActive(false);

                //Reset ?
                OutputManager.VibrateAll(0, 0);
                
                break;
            case EStatePhase.PRE_PHASE:
                PlayersLink.SetActive(true);
                OutputManager.VibrateAll(PrePhaseVibration, PrePhaseVibration);
                break;
            case EStatePhase.PHASE:
                PlayersLink.SetActive(true);
                OutputManager.VibrateAll(PhaseVibration, PhaseVibration);
                //Set input manager to average players input to control the two ships as one
                inputManager.setPlayersInputAverageMode(true);

                // Next proto:
                //Activate shield on ships
                //Invulnerability on phasing during 2 seconds
                break;
        }

        PlayersLink.GetComponent<MeshRenderer>().material = MaterialsPlayersLink[(int)state];
        SetSpaceshipsWeapon(state);
    }

    void SetSpaceshipsWeapon(EStatePhase state)
    {
        Spaceships[0].SetWeapon(state);
        Spaceships[1].SetWeapon(state);
    }
}
