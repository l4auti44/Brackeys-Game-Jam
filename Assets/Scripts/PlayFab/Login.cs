using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField customNameInput;
    private string customID;

    private const string CustomIdKey = "PF_CUSTOM_ID";

    private const string LastUsernameKey = "LAST_USERNAME";


    void Awake()
{
    if (PlayerPrefs.HasKey(LastUsernameKey))
    {
        customNameInput.text = PlayerPrefs.GetString(LastUsernameKey);
        customNameInput.MoveTextEnd(false);
    }
}


    public void LoginAndEnsureDisplayName()
    {
        customID = GetOrCreateCustomId();

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
         { 
            CustomId = customID,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
         };

        

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessWithPlayerInput, OnLoginFailure);
    }


    private void OnLoginSuccessWithPlayerInput(LoginResult result)
    {
        Debug.Log("Login OK 🔥 PlayerId: " + result.PlayFabId);

        string currentName = result.InfoResultPayload?.PlayerProfile?.DisplayName;
        string desiredName = customNameInput?.text?.Trim();
        if (!string.IsNullOrEmpty(currentName))
        {
            if (currentName != desiredName)
            {
                SetDisplayNameFromInput();
            }else
            {
                if (!PlayerPrefs.HasKey(LastUsernameKey))
                {
                    PlayerPrefs.SetString(LastUsernameKey, desiredName);
                    PlayerPrefs.Save();
                }
                Debug.Log("Welcome back, " + currentName);
                SceneController.SceneLoader("GameScene");
            }
        }else
        {
            SetDisplayNameFromInput();
        }
            

        
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
                Debug.Log("Score upload 🚀: " + score);
            },
            error => Debug.LogError("cannot upload score 💀: " + error.GenerateErrorReport())
        );
    }


    private string GetOrCreateCustomId()
    {
        if (PlayerPrefs.HasKey(CustomIdKey))
            return PlayerPrefs.GetString(CustomIdKey);

        string newId = System.Guid.NewGuid().ToString("N");
        PlayerPrefs.SetString(CustomIdKey, newId);
        PlayerPrefs.Save();
        return newId;
    }


    public void SetDisplayNameFromInput()
    {
        string desiredName = customNameInput?.text?.Trim();

        if (string.IsNullOrEmpty(desiredName))
        {
            Debug.LogWarning("Display name is empty 💀");
            return;
        }


        var req = new UpdateUserTitleDisplayNameRequest { DisplayName = desiredName };

        PlayFabClientAPI.UpdateUserTitleDisplayName(req,
            r =>
            {
                Debug.Log("Name set 😎: " + desiredName);
                PlayerPrefs.SetString(LastUsernameKey, desiredName);
                PlayerPrefs.Save();
                SceneController.SceneLoader("GameScene");
            },
            e =>
            {
                Debug.LogError("Cannot set name 💀: " + e.GenerateErrorReport());
            }
        );
    }

}
