using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Spaceship> Spaceships = new List<Spaceship>();

    public LifeCounter lifeCounter;

    public List<Image> phaseZones = new List<Image>();

    public GameObject gameOverOverlay;
    public GameObject hudOverlay;

    public Text HUDLifeCounter;
    
    void Update()
    {
        if ( lifeCounter.LifeCount <= 0 )
        {
            foreach (Spaceship spaceship in Spaceships)
            {
                spaceship.GetComponentInChildren<Renderer>().enabled = false;
            }

            hudOverlay.SetActive(false);
            gameOverOverlay.SetActive(true);
        }

        HUDLifeCounter.text = "x" + lifeCounter.LifeCount;

        for (int i = 0; i < 2; i++)
        {
            phaseZones[i].transform.position = Spaceships[i].transform.position;
        }
    }
}
