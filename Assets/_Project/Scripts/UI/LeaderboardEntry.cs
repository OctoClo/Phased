using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    public GameObject rankContainer;
    public GameObject nameContainer;
    public GameObject scoreContainer;

    TextMeshProUGUI rankText;
    TextMeshProUGUI nameText;
    TextMeshProUGUI scoreText;

    void Start()
    {
        rankText = rankContainer.GetComponent<TextMeshProUGUI>();
        nameText = nameContainer.GetComponent<TextMeshProUGUI>();
        scoreText = scoreContainer.GetComponent<TextMeshProUGUI>();
    }
    
    public void Fill(int rank, string name, string score)
    {
        rankText.SetText(rank.ToString());
        nameText.SetText(name);
        scoreText.SetText(FillScoreWithZeros(score));
    }

    string FillScoreWithZeros(string scoreTxt)
    {

        string scoreFill = "";

        for (int i = 0; i < (7 - scoreTxt.Length); i++)
        {
            scoreFill += "0";
        }

        return scoreFill + scoreTxt;

    }
}
