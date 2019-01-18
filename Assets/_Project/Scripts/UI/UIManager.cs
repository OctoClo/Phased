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
    public GameObject QuitOverlay;
    public GameObject LeaderboardOverlay;
    public GameObject CreditsOverlay;
    public GameObject OptionsOverlay;

    public Animator IntroAnimation;
    public Leaderboard Leaderboard;

    [Header("HUD Elements")]
    public GameObject HUDLifeCounterContainer;
    public GameObject ScoreContainer;
    public GameObject MultiplicatorContainer;

    [Header("Spaceships")]
    public List<Spaceship> Spaceships = new List<Spaceship>();
    public List<Image> PhaseZones = new List<Image>();

    [Header("Leaderboard")]
    public Leaderboard leaderboard;


    TextMeshProUGUI HUDLifeCounter;
    TextMeshProUGUI score;
    TextMeshProUGUI multiplicator;

    GameObject currentOverlay;
    GameObject beforeQuitOverlay;
    GameObject previousOverlay;

    bool gameActive = false;

    void Start()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);
        EventManager.Instance.AddListener<GameBackEvent>(OnGameBackEvent);
        EventManager.Instance.AddListener<GameQuitAskEvent>(OnGameQuitAskEvent);
        EventManager.Instance.AddListener<GameQuitConfirmEvent>(OnGameQuitConfirmEvent);
        EventManager.Instance.AddListener<GameQuitCancelEvent>(OnGameQuitCancelEvent);
        EventManager.Instance.AddListener<GameLeaderboardEvent>(OnGameLeaderboardEvent);
        EventManager.Instance.AddListener<GameCreditsEvent>(OnGameCreditsEvent);
        EventManager.Instance.AddListener<GameOptionsEvent>(OnGameOptionsEvent);
        EventManager.Instance.AddListener<GameMainMenuEvent>(OnGameMainMenuEvent);
        
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

            score.SetText(FillScoreWithZeros(GameScore.Score.ToString()));

            multiplicator.SetText(GameScore.Multiplicator.ToString());

            /*for (int i = 0; i < 2; i++)
            {
                PhaseZones[i].transform.position = Spaceships[i].transform.position;
            }*/
        }
    }

    string FillScoreWithZeros(string scoreTxt)
    {

        string scoreFill = "";

        for (int i = 0; i < (8 - scoreTxt.Length); i++)
        {
            scoreFill += "0";
        }

        return scoreFill + scoreTxt;

    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        gameActive = true;
        currentOverlay.SetActive(false);
        HUDOverlay.SetActive(true);
        currentOverlay = HUDOverlay;
        IntroAnimation.Play("Intro");
    }

    void OnGameBackEvent(GameBackEvent e)
    {
        Debug.Log("OnGameBackEvent");
        previousOverlay.SetActive(true);
        currentOverlay.SetActive(false);
        currentOverlay = previousOverlay;
        previousOverlay = null;
    }

    void OnGameLeaderboardEvent(GameLeaderboardEvent e)
    {
        gameActive = false;

        currentOverlay.SetActive(false);

        previousOverlay = currentOverlay;

        currentOverlay = LeaderboardOverlay;
        LeaderboardOverlay.SetActive(true);

        StartCoroutine(DelayWriteLeaderboard());
    }

    IEnumerator DelayWriteLeaderboard()
    {
        yield return new WaitForSeconds(0.05f);
        leaderboard.WriteScoresToUI();
    }

    void OnGameCreditsEvent(GameCreditsEvent e)
    {
        gameActive = false;

        currentOverlay.SetActive(false);

        currentOverlay = CreditsOverlay;
        CreditsOverlay.SetActive(true);
    }

    void OnGameOptionsEvent(GameOptionsEvent e)
    {
        gameActive = false;

        currentOverlay.SetActive(false);

        currentOverlay = OptionsOverlay;
        OptionsOverlay.SetActive(true);
    }

    void OnGameMainMenuEvent(GameMainMenuEvent e)
    {
        gameActive = false;

        currentOverlay.SetActive(false);

        currentOverlay = MenuOverlay;
        MenuOverlay.SetActive(true);
    }
    
    void OnGameEndEvent(GameEndEvent e)
    {
        gameActive = false;
        /*for (int i = 0; i < 2; i++)
        {
            PhaseZones[i].gameObject.SetActive(false);
        }*/

        currentOverlay.SetActive(false);

        if (e.Victorious)
        {
            VictoryOverlay.SetActive(true);

            var score = VictoryOverlay.transform.Find("ScoreLine/ScoreCount").GetComponent<TextMeshProUGUI>();
            score.text = GameScore.Score.ToString();

            var rank = VictoryOverlay.transform.Find("RankLine/RankCount").GetComponent<TextMeshProUGUI>();
            rank.text = Leaderboard.GetRankByScore(GameScore.Score).ToString();

            currentOverlay = VictoryOverlay;
        }
        else
        {
            GameOverOverlay.SetActive(true);

            var score = GameOverOverlay.transform.Find("Score").GetComponent<TextMeshProUGUI>();
            score.text = FillScoreWithZeros(GameScore.Score.ToString());

            currentOverlay = GameOverOverlay;
        }
    }

    void OnGameQuitAskEvent(GameQuitAskEvent e)
    {
        beforeQuitOverlay = currentOverlay;
        QuitOverlay.SetActive(true);
    }

    void OnGameQuitConfirmEvent(GameQuitConfirmEvent e)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnGameQuitCancelEvent(GameQuitCancelEvent e)
    {
        QuitOverlay.SetActive(false);
        beforeQuitOverlay.SetActive(false);
        beforeQuitOverlay.SetActive(true);
        currentOverlay = beforeQuitOverlay;
    }
}
