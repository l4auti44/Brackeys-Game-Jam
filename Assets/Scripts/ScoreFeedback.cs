using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreFeedback : MonoBehaviour
{
    [SerializeField] private GameObject floatingPointPrefab;
    [SerializeField] private Transform canvasPopUps;
    [SerializeField] private AnimationClip floatingPointAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowFloatingPoints(int points,Vector2 position)
    {
        GameObject floatingPoint = GameObject.Instantiate(floatingPointPrefab, position, Quaternion.identity, canvasPopUps);
        floatingPoint.GetComponentInChildren<TextMeshProUGUI>().text = points.ToString();
        GameObject.Destroy(floatingPoint, floatingPointAnimation.length);
    }
}
