using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Overlays")]
    public GameObject MenuOverlay;
    public GameObject HUDOverlay;
    public GameObject VictoryOverlay;
    public GameObject GameOverOverlay;

    [Header("HUD Elements")]
    public GameObject HUDLifeCounterContainer;
    public GameObject ScoreContainer;
    public GameObject MultiplicatorContainer;

    [Header("Spaceships")]
    public List<Spaceship> Spaceships = new List<Spaceship>();
    public List<Image> PhaseZones = new List<Image>();

    TextMeshProUGUI HUDLifeCounter;
    TextMeshProUGUI score;
    TextMeshProUGUI multiplicator;

    GameObject currentOverlay;

    bool gameActive = false;

    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);

        HUDLifeCounter = HUDLifeCounterContainer.GetComponent<TextMeshProUGUI>();
        score = ScoreContainer.GetComponent<TextMeshProUGUI>();
        multiplicator = MultiplicatorContainer.GetComponent<TextMeshProUGUI>();

        currentOverlay = MenuOverlay;
    }
    
    void Update()
    {
        if (gameActive)
        {
            HUDLifeCounter.SetText(LifeCounter.Instance.LifeCount.ToString());

            string scoreFill = "";
            string scoreTxt = GameScore.Score.ToString();

            for (int i = 0; i < (8 - scoreTxt.Length); i++)
            {
                scoreFill += "0";
            }

            score.SetText(scoreFill + scoreTxt);

            multiplicator.SetText(GameScore.Multiplicator.ToString());

            for (int i = 0; i < 2; i++)
            {
                PhaseZones[i].transform.position = Spaceships[i].transform.position;
            }
        }
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        gameActive = true;

        currentOverlay.SetActive(false);
        HUDOverlay.SetActive(true);
        currentOverlay = HUDOverlay;
    }

    void OnGameEndEvent(GameEndEvent e)
    {
        gameActive = false;
        for (int i = 0; i < 2; i++)
        {
            PhaseZones[i].gameObject.SetActive(false);
        }

        currentOverlay.SetActive(false);

        if (e.Victorious)
        {
            VictoryOverlay.SetActive(true);
            currentOverlay = VictoryOverlay;
        }
        else
        {
            GameOverOverlay.SetActive(true);
            currentOverlay = GameOverOverlay;
        }
    }
}
