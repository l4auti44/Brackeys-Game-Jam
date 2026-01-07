using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    void Awake()
    {
        LoginAsPlayer();
    }



    private void LoginAsPlayer(string customId = "LautiCrack")
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest { CustomId = customId, CreateAccount = false };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login OK 🔥 PlayerId: " + result.PlayFabId);

        var nameReq = new UpdateUserTitleDisplayNameRequest { DisplayName = "LautiCrack" };

        PlayFabClientAPI.UpdateUserTitleDisplayName(nameReq,
            r =>
            {
                Debug.Log("Nombre listo 😎");
            },
            e =>
            {
                Debug.LogError("No pude setear nombre 💀: " + e.GenerateErrorReport());
            }
        );
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failed 😭 " + error.GenerateErrorReport());
    }


    public void SubmitScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "HighScore", Value = score }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, 
            result =>   
            {
                Debug.Log("Score subido 🚀: " + score);
            },
            error => Debug.LogError("No subió el score 💀: " + error.GenerateErrorReport())
        );
    }



}
