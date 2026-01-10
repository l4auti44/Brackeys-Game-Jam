using PlayFab;
using UnityEngine;

public class HideIfLogged : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayFabClientAPI.IsClientLoggedIn() && gameObject.activeSelf)
        {
            
            gameObject.SetActive(false);
        }
    }
}
