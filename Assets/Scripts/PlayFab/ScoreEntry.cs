using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEntry : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI rankText;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;


    public void SetData(int rank, string name, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
