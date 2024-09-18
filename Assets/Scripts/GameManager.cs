using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameProgress, gameProgressSpeed;

    public GameObject gameProgressSlider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Increae the game progress over time
        gameProgress += gameProgressSpeed * 0.01f;

        //Update the slider filler
        gameProgressSlider.GetComponent<Slider>().value = gameProgress * 0.01f;
    }
}
