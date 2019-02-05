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
    public GameObject ControlsOverlay;
    public GameObject PauseOverlay;

    [Header("HUD Elements")]
    public GameObject HUDLifeCounterContainer;
    public GameObject ScoreContainer;
    public GameObject MultiplicatorContainer;
    public GameObject MultiplicatorXContainer;
    public GameObject PauseScoreContainer;

    [Header("Intro")]
    public Animator IntroAnimator;
    public AnimationClip IntroAnimation;

    [Header("Spaceships")]
    public List<Spaceship> Spaceships = new List<Spaceship>();
    public List<Image> PhaseZones = new List<Image>();

    [Header("Other")]
    public Leaderboard Leaderboard;

    TextMeshProUGUI HUDLifeCounter;
    TextMeshProUGUI score;
    TextMeshProUGUI multiplicator;
    TextMeshProUGUI multiplicatorX;
    TextMeshProUGUI pauseScore;

    GameObject currentOverlay;
    GameObject beforeQuitOverlay;
    GameObject previousOverlay;

    bool gameActive = false;

    void Start()
    {
        HUDLifeCounter = HUDLifeCounterContainer.GetComponent<TextMeshProUGUI>();
        score = ScoreContainer.GetComponent<TextMeshProUGUI>();
        multiplicator = MultiplicatorContainer.GetComponent<TextMeshProUGUI>();
        multiplicatorX = MultiplicatorXContainer.GetComponent<TextMeshProUGUI>();
        pauseScore = PauseScoreContainer.GetComponent<TextMeshProUGUI>();

        currentOverlay = MenuOverlay;
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);
        EventManager.Instance.AddListener<GameBackEvent>(OnGameBackEvent);
        EventManager.Instance.AddListener<GameQuitAskEvent>(OnGameQuitAskEvent);
        EventManager.Instance.AddListener<GameQuitConfirmEvent>(OnGameQuitConfirmEvent);
        EventManager.Instance.AddListener<GameQuitCancelEvent>(OnGameQuitCancelEvent);
        EventManager.Instance.AddListener<GameLeaderboardEvent>(OnGameLeaderboardEvent);
        EventManager.Instance.AddListener<GameCreditsEvent>(OnGameCreditsEvent);
        EventManager.Instance.AddListener<GameControlsEvent>(OnGameControlsEvent);
        EventManager.Instance.AddListener<GameMainMenuEvent>(OnGameMainMenuEvent);
        EventManager.Instance.AddListener<GamePausedEvent>(OnGamePausedEvent);
        EventManager.Instance.AddListener<GameResumedEvent>(OnGameResumedEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.RemoveListener<GameEndEvent>(OnGameEndEvent);
        EventManager.Instance.RemoveListener<GameBackEvent>(OnGameBackEvent);
        EventManager.Instance.RemoveListener<GameQuitAskEvent>(OnGameQuitAskEvent);
        EventManager.Instance.RemoveListener<GameQuitConfirmEvent>(OnGameQuitConfirmEvent);
        EventManager.Instance.RemoveListener<GameQuitCancelEvent>(OnGameQuitCancelEvent);
        EventManager.Instance.RemoveListener<GameLeaderboardEvent>(OnGameLeaderboardEvent);
        EventManager.Instance.RemoveListener<GameCreditsEvent>(OnGameCreditsEvent);
        EventManager.Instance.RemoveListener<GameControlsEvent>(OnGameControlsEvent);
        EventManager.Instance.RemoveListener<GameMainMenuEvent>(OnGameMainMenuEvent);
        EventManager.Instance.RemoveListener<GamePausedEvent>(OnGamePausedEvent);
        EventManager.Instance.RemoveListener<GameResumedEvent>(OnGameResumedEvent);
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    void Update()
    {
        if (gameActive)
        {
            UpdateHUD();

            /*for (int i = 0; i < 2; i++)
            {
                PhaseZones[i].transform.position = Spaceships[i].transform.position;
            }*/
        }
    }

    void UpdateHUD()
    {
        HUDLifeCounter.SetText(LifeCounter.Instance.LifeCount.ToString());

        score.SetText(FillScoreWithZeros(GameScore.Score.ToString()));

        multiplicator.SetText(GameScore.Multiplicator.ToString());
    }

    public void UpdateColor(Color color)
    {
        multiplicator.faceColor = color;
        multiplicatorX.faceColor = color;
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
        StartCoroutine(WaitForIntroEnd());
        currentOverlay.SetActive(false);
        HUDOverlay.SetActive(true);
        currentOverlay = HUDOverlay;
        UpdateHUD();
        IntroAnimator.Play("Intro");
        AkSoundEngine.PostEvent("play_music_game", gameObject);
    }

    IEnumerator WaitForIntroEnd()
    {
        yield return new WaitForSecondsRealtime(IntroAnimation.length);
        gameActive = true;
    }

    void OnGamePausedEvent(GamePausedEvent e)
    {
        PauseOverlay.SetActive(true);
        currentOverlay = PauseOverlay;
        pauseScore.SetText(FillScoreWithZeros(GameScore.Score.ToString()));
    }

    void OnGameResumedEvent(GameResumedEvent e)
    {
        PauseOverlay.SetActive(false);
        currentOverlay = HUDOverlay;
    }

    void OnGameBackEvent(GameBackEvent e)
    {
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
        yield return new WaitForSecondsRealtime(0.05f);
        Leaderboard.WriteScoresToUI();
    }

    void OnGameCreditsEvent(GameCreditsEvent e)
    {
        gameActive = false;

        currentOverlay.SetActive(false);

        previousOverlay = currentOverlay;

        currentOverlay = CreditsOverlay;
        CreditsOverlay.SetActive(true);
    }

    void OnGameControlsEvent(GameControlsEvent e)
    {
        gameActive = false;

        currentOverlay.SetActive(false);

        previousOverlay = currentOverlay;

        currentOverlay = ControlsOverlay;
        ControlsOverlay.SetActive(true);
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
        AkSoundEngine.PostEvent("play_music_menu", gameObject);
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
        gameActive = false;
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
        currentOverlay.SetActive(false);
        currentOverlay.SetActive(true);

        if (currentOverlay == PauseOverlay)
        {
            gameActive = true;
        }
    }
}
