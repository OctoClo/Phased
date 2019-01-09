using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public LifeCounter lifeCounter;
    public Text HUDLifeCounter;

    public Text score;
    public Text multiplicator;

    public List<Spaceship> Spaceships = new List<Spaceship>();
    public List<Image> phaseZones = new List<Image>();

    public GameObject gameOverOverlay;
    public GameObject hudOverlay;    
    
    void Update()
    {
        if ( lifeCounter.LifeCount <= 0 )
        {
            GameOver();
        }

        HUDLifeCounter.text = "x" + lifeCounter.LifeCount;
        score.text = GameScore.Score.ToString();
        multiplicator.text = "x" + GameScore.Multiplicator.ToString();

        for (int i = 0; i < 2; i++)
        {
            phaseZones[i].transform.position = Spaceships[i].transform.position;
        }
    }

    void GameOver()
    {
        foreach (Spaceship spaceship in Spaceships)
        {
            spaceship.GetComponentInChildren<Renderer>().enabled = false;
        }

        hudOverlay.SetActive(false);
        gameOverOverlay.SetActive(true);
    }
}
