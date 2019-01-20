using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public struct Entry
    {
        public string  name;
        public ulong   score;
        public ulong   maxHitCount;
    };

    public List<Entry> Scores { get; private set; } = null;
    public GameObject TeamNameInput;
    public List<LeaderboardEntry> UIEntries;
    public MenuSelector LeaderboardMenuSelector;

    bool firstEntryAdded = false;

    public void SubmitScore()
    {
        AddScore( new Entry
        {
            name = TeamNameInput.GetComponent<UnityEngine.UI.InputField>().text,
            score = GameScore.Score,
            maxHitCount = 0 // TODO: Implement me!
        } );

        LeaderboardMenuSelector.enabled = true;

        EventManager.Instance.Raise(new GameLeaderboardEvent());
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameEndEvent>(OnGameEndEvent);
    }

    void Start()
    {
        string path = Application.dataPath + "\\Leaderboard.txt";

        Scores = new List<Entry>();

        if (!File.Exists(path))
        {
            File.CreateText(path);
            return;
        }

        StreamReader reader = new StreamReader(path);

        string line = reader.ReadLine();
        while ( line != null )
        {
            var values = line.Split(' ');
            if ( values.Length == 3 )
            {
                Entry e = new Entry
                {
                    name = values[0],
                    score = ulong.Parse(values[1]),
                    maxHitCount = ulong.Parse(values[2])
                };

                Scores.Add( e );
            }

            line = reader.ReadLine();
        }
        reader.Close();

        Debug.Log("Loaded " + Scores.Count + " entries");
        Scores.Sort((x, y) => y.score.CompareTo(x.score));

        WriteScoresToUI();
    }
    
    public int GetRankByScore( ulong score )
    {
        var rank = Scores.FindIndex(x => x.score < score) + 1;

        return ( rank == 0 ) ? ( Scores.Count + 1 ) : rank;
    }

    public void AddScore( Leaderboard.Entry entry )
    {
        Scores.Add(entry);

        UpdateLeaderboardDb();

        //Sort
        Scores.Sort((x, y) => y.score.CompareTo(x.score));

        WriteScoresToUI();
    }

    void UpdateLeaderboardDb()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "\\Leaderboard.txt", true);

        var latestEntry = Scores[Scores.Count - 1];

        if (firstEntryAdded)
        {
            writer.WriteLine();
            firstEntryAdded = false;
        }
        writer.WriteLine(latestEntry.name + ' ' + latestEntry.score.ToString() + ' ' + latestEntry.maxHitCount.ToString());

        writer.Close();
    }

    public void WriteScoresToUI()
    {
        int rank = 1;
        for (int i = 0; i < Mathf.Min(UIEntries.Count, Scores.Count); i++)
        {
            UIEntries[i].Fill(rank, Scores[i].name, Scores[i].score.ToString());
            rank++;
        }
    }

    void OnGameEndEvent(GameEndEvent e)
    {
        if (e.Victorious)
        {
            TeamNameInput.GetComponent<InputField>().Initialize();
            LeaderboardMenuSelector.enabled = false;
        }
    }
}
