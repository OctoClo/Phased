using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EStatePhase { NO_PHASE, PRE_PHASE, PHASE };

public class PhasingManager : MonoBehaviour
{
    public List<Spaceship> Spaceships = new List<Spaceship>();
    public List<GameObject> Weapons = new List<GameObject>();
    public List<Material> Materials = new List<Material>();

    public GameObject PlayersLink;

    public OutputManager outputManager;

    public float PrePhaseTriggerDist = 18.0f;
    public float PhaseTriggerDist = 8.0f;

    public float PrePhaseVibration = 0.05f;
    public float PhaseVibration = 0.1f;

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
        switch(state)
        {
            case EStatePhase.NO_PHASE:
                PlayersLink.SetActive(false);
                break;
            case EStatePhase.PRE_PHASE:
                PlayersLink.SetActive(true);
                outputManager.VibrateAll(PrePhaseVibration, PrePhaseVibration);
                break;
            case EStatePhase.PHASE:
                PlayersLink.SetActive(true);
                outputManager.VibrateAll(PhaseVibration, PhaseVibration);
                //Set input manager to average players input to control the two ships as one

                // Next proto:
                //Activate shield on ships
                //Invulnerability on phasing during 2 seconds
                break;
        }

        PlayersLink.GetComponent<MeshRenderer>().material = Materials[(int)state];
        SetSpaceshipsWeapon(Weapons[(int)state]);
    }

    void SetSpaceshipsWeapon(GameObject weapon)
    {
        Spaceships[0].SetWeapon(weapon);
        Spaceships[1].SetWeapon(weapon);
    }
}
