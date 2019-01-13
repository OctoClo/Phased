using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public LifeCounter lifeCounter;

    public GameObject HUDLifeCounterContainer;
    TextMeshProUGUI HUDLifeCounter;

    public GameObject scoreContainer;
    TextMeshProUGUI score;
    public GameObject multiplicatorContainer;
    TextMeshProUGUI multiplicator;

    public List<Spaceship> Spaceships = new List<Spaceship>();
    public List<Image> phaseZones = new List<Image>();

    public GameObject gameOverOverlay;
    public GameObject hudOverlay;    

    void Start()
    {
        HUDLifeCounter = HUDLifeCounterContainer.GetComponent<TextMeshProUGUI>();
        score = scoreContainer.GetComponent<TextMeshProUGUI>();
        multiplicator = multiplicatorContainer.GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if ( lifeCounter.LifeCount <= 0 )
        {
            GameOver();
        }

        HUDLifeCounter.SetText(lifeCounter.LifeCount.ToString());

        string scoreFill = "";
        string scoreTxt = GameScore.Score.ToString();

        for (int i = 0; i < (8 - scoreTxt.Length); i++){
            scoreFill += "0";
        }

        score.SetText(scoreFill + scoreTxt);

        multiplicator.SetText(GameScore.Multiplicator.ToString());

        // for (int i = 0; i < 2; i++)
        // {
        //     phaseZones[i].transform.position = Spaceships[i].transform.position;
        // }
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
