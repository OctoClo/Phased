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
    public UnityEngine.UI.Text UIElement;

    void Start()
    {
        Scores = new List<Entry>();

        StreamReader reader = new StreamReader(Application.dataPath + "\\Leaderboard.txt");

        // Create file on first run
        if ( reader == null )
        {
            File.CreateText(Application.dataPath + "\\Leaderboard.txt");
            return;
        }

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

        int rank = 1;
        //UIElement.text = " rank\t name\t score\t max hit\n";
        foreach ( var entry in Scores )
        {
            UIElement.text += " " + rank++ + "\t " + entry.name + "\t " + entry.score.ToString().PadLeft(16, '0') + "\t " + entry.maxHitCount.ToString().PadLeft(8, '0') + '\n';
        }

        Debug.Log(GetRankByScore(0));
        Debug.Log(GetRankByScore(668));
    }

    public int GetRankByScore( ulong score )
    {
        var rank = Scores.FindIndex(x => x.score < score) + 1;

        return ( rank == 0 ) ? ( Scores.Count + 1 ) : rank;
    }

    public void AddScore(Leaderboard.Entry entry)
    {
        Scores.Add(entry);

        UpdateLeaderboardDb();
    }

    void UpdateLeaderboardDb()
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "\\Leaderboard.txt", true);

        var latestEntry = Scores[Scores.Count - 1];
        writer.WriteLine(latestEntry.name + ' ' + latestEntry.score.ToString() + ' ' + latestEntry.maxHitCount.ToString());

        writer.Close();
    }
}
