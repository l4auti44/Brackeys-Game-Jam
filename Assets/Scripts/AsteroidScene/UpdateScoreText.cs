
using System.Collections;
using TMPro;
using UnityEngine;

public class UpdateScoreText : MonoBehaviour
{
    [SerializeField] private float mspace = 0.67f; // Adjust this value to change the space between characters
    private TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        StartCoroutine("UpdateTextCoroutine");
    }

    IEnumerator UpdateTextCoroutine()
    {
        while (true)
        {
            scoreText.text = $"<mspace={mspace}em>{ScoreManager.GetScore().ToString("D12")}</mspace>";
            yield return new WaitForSeconds(0.1f);
        }
       
    }
}
