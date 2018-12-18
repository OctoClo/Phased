using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasingManager : MonoBehaviour
{
    //0:Default, 1:Pre-phasing, 2:Phased
    [SerializeField]
    int state = 0;

    float distBetweenShips = 0.0f;

    public float prePhaseTriggerDist = 18.0f;
    public float phaseTriggerDist = 8.0f;

    public List<Spaceship> Spaceships = new List<Spaceship>();
    public GameObject playersLink;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distBetweenShips = Vector3.Distance(Spaceships[0].transform.position, Spaceships[1].transform.position);

        updatePlayerLink();

        checkForStateChange();
    }

    void updatePlayerLink(){

        playersLink.transform.position = (Spaceships[0].transform.position + Spaceships[1].transform.position) / 2;

        float angle = Mathf.Atan2(Spaceships[0].transform.position.z - Spaceships[1].transform.position.z, Spaceships[0].transform.position.x - Spaceships[1].transform.position.x) * Mathf.Rad2Deg;

        playersLink.transform.rotation = Quaternion.Euler(0, angle * -1, 0);

        playersLink.transform.localScale = new Vector3(distBetweenShips - 4, 0.25f, 0.25f);

    }

    void checkForStateChange(){

        int newStateId = 0;

        if (distBetweenShips < phaseTriggerDist)
            newStateId = 2;
        else if (distBetweenShips < prePhaseTriggerDist)
            newStateId = 1;

        if (!state.Equals(newStateId))
            setState(newStateId);
            handleStateChange();

    }

    void setState(int id){
        state = id;
    }

    void handleStateChange(){
        
        switch(state){
            case 0:
                playersLink.SetActive(false);
                //Switch to weapon 0
                break;
            case 1:
                playersLink.SetActive(true);
                //Switch to weapon 1
                //Subtle vibration
                break;
            case 2:
                playersLink.SetActive(true);
                //Switch to weapon 2
                //Vibration on phasing
                //Invulnerability on phasing during 2 seconds
                //Activate shield on ships
                //Set input manager to average players input to control the two ships as one
                break;
        }
        

    }
}
