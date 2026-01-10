using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PoblateLeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject scoreTemplate;
    [SerializeField] private Transform scoreParent;

    [SerializeField] private Color backgroundColorAlt;

    private bool fetched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayFabClientAPI.IsClientLoggedIn() && !fetched)
        {
            
            GetTop10();
            ClearLeaderboard();
        }
    }

    public void GetTop10()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        fetched = true;
        PlayFabClientAPI.GetLeaderboard(request,
            result =>
            {
                Debug.Log("TOP 10 🏆");
                foreach (var entry in result.Leaderboard)
                {
                    var scoreObject = Instantiate(scoreTemplate, scoreParent);
                    if (entry.Position % 2 == 1)
                    {
                        scoreObject.GetComponentInChildren<UnityEngine.UI.Image>().color = backgroundColorAlt;
                    }
                    var name = string.IsNullOrEmpty(entry.DisplayName) ? entry.PlayFabId : entry.DisplayName;
                    scoreObject.GetComponentInChildren<ScoreEntry>().SetData(entry.Position + 1, name, entry.StatValue);
                    Debug.Log($"{entry.Position + 1}) {name} - {entry.StatValue}");
                };
            },
            error => Debug.LogError("No pude leer leaderboard 💀: " + error.GenerateErrorReport())
        );
    }

    private void ClearLeaderboard()
    {
        foreach (Transform child in scoreParent)
        {
            Destroy(child.gameObject);
        }
    }
}
