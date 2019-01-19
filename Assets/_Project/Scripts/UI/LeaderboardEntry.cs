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

    }
    
    public void Fill(int rank, string name, string score)
    {
        rankText = rankContainer.GetComponent<TextMeshProUGUI>();
        nameText = nameContainer.GetComponent<TextMeshProUGUI>();
        scoreText = scoreContainer.GetComponent<TextMeshProUGUI>();

        rankText.SetText(rank.ToString());
        nameText.SetText(name);
        scoreText.SetText(score.PadLeft(7, '0'));
    }
}
