
using System.Collections;
using TMPro;
using UnityEngine;

public class UpdateScoreText : MonoBehaviour
{
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
            scoreText.text = ScoreManager.GetScore().ToString("D12");
            yield return new WaitForSeconds(0.1f);
        }
       
    }
}
