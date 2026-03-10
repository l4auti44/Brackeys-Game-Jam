using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        EventManager.Login.OnLoginLoading?.Invoke();

        // WebGL builds don’t have a device id, and the PlayerPrefs storage
        // can be cleared by the browser between sessions. Instead of
        // generating a GUID we simply reuse the string that the player
        // types in – this makes the "user name" act as the login key.
        //
        // For native platforms you may still want a persistent id (device
        // id or generated GUID) so players aren’t forced to remember one.
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            customID = customNameInput?.text?.Trim();
        }
        else
        {
            customID = GetOrCreateCustomId();
        }

        // fall back if input was empty for whatever reason
        if (string.IsNullOrEmpty(customID))
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
                if (SceneManager.GetActiveScene().name == "Main Menu")
                {
                    EventManager.Login.OnLoginSuccess?.Invoke();
                    SceneController.SceneLoader("GameScene", 1.5f);
                }
                    
            }
        }else
        {
            SetDisplayNameFromInput();
        }
            

        
    }

    private void OnLoginFailure(PlayFabError error)
    {
        EventManager.Login.OnLoginFailure?.Invoke(error.GenerateErrorReport());
        Debug.LogError("Login failed 😭 " + error.GenerateErrorReport());
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
            EventManager.Login.OnLoginFailure?.Invoke("Display name cannot be empty.");
            return;
        }


        var req = new UpdateUserTitleDisplayNameRequest { DisplayName = desiredName };

        PlayFabClientAPI.UpdateUserTitleDisplayName(req,
            r =>
            {
                Debug.Log("Name set 😎: " + desiredName);
                PlayerPrefs.SetString(LastUsernameKey, desiredName);
                PlayerPrefs.Save();
                if (SceneManager.GetActiveScene().name == "Main Menu")
                {
                    EventManager.Login.OnLoginSuccess?.Invoke();
                    SceneController.SceneLoader("GameScene", 1.5f);
                }
                    
            },
            e =>
            {
                EventManager.Login.OnLoginFailure?.Invoke("Failed to set display name: " + e.GenerateErrorReport());
                Debug.LogError("Cannot set name 💀: " + e.GenerateErrorReport());
            }
        );
    }

}
