using System.Collections;
using UnityEngine;

public static class ScoreManager
{
    private static int score = 0;

    private static ScoreFeedback scoreFeedback;



    public static void AddScore(int points)
    {
        score += points;
    }

    public static void AddScore(int points, Vector2 position)
    {
        if (scoreFeedback == null)
        {
            scoreFeedback = Object.FindObjectOfType<ScoreFeedback>();
        }
        score += points;
        scoreFeedback.ShowFloatingPoints(points, position);
    }
    public static void SubtractScore(int points)
    {
        score -= points;
    }

    public static int GetScore()
    {
        return score;
    }
    
    public static void ResetScore()
    {
        score = 0;
    }
}

