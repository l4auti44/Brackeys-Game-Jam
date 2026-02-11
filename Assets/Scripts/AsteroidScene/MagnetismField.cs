using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismField : MonoBehaviour
{

    public float strength;

    




    // Update is called once per frame
    void Update()
    {
        
    }


    public void RecieveMod(float s, float new_size)
    {

        strength = s;

        float n = new_size;

        Debug.Log(s);

        Debug.Log(n);

        
        

        Vector2 current_size = new Vector2(n + transform.localScale.x, n + transform.localScale.y);


        transform.localScale = current_size;


    }

}
