using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class SubmitScore : MonoBehaviour
{
    public void SubmitNewScore(int newScore)
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogError("Player not logged in, cannot submit score.");
            return;
        }

        var getReq = new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { "HighScore" }
        };

        PlayFabClientAPI.GetPlayerStatistics(getReq,
            getRes =>
            {
                int currentBest = 0;
                var stat = getRes.Statistics?.Find(s => s.StatisticName == "HighScore");
                if (stat != null) currentBest = stat.Value;

                if (newScore <= currentBest)
                {
                    Debug.Log("Score not higher than current best, not submitting. Current Best: " + currentBest + ", New Score: " + newScore);
                    return;
                }

                var updateReq = new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = "HighScore", Value = newScore }
                    }
                };

                PlayFabClientAPI.UpdatePlayerStatistics(updateReq,
                    _ => Debug.Log("New BEST uploaded 🚀: " + newScore),
                    e => Debug.LogError("cannot upload score 💀: " + e.GenerateErrorReport())
                );
            },
            e => Debug.LogError("cannot read current score 💀: " + e.GenerateErrorReport())
        );
    }

    private void SubmitNewScoreWithGameManager(Component comp)
    {
        int score = ScoreManager.GetScore();
        SubmitNewScore(score);
    }

    void Awake()
    {
        EventManager.Game.OnDie += SubmitNewScoreWithGameManager;
    }

    void OnDestroy()
    {
        EventManager.Game.OnDie -= SubmitNewScoreWithGameManager;
    }
}
