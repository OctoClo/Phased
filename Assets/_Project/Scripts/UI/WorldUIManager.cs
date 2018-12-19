using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{

    public List<Spaceship> Spaceships = new List<Spaceship>();

    public List<Image> phaseZones = new List<Image>(); 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < 2; i++){
            phaseZones[i].transform.position = Spaceships[i].transform.position;
        }

    }
}
