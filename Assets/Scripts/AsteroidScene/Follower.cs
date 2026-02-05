using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    [SerializeField]
    Rigidbody2D body;

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "MagneticField")
        {
            Debug.Log("DoingSomething");
            var f = collision.gameObject.GetComponent<MagnetismField>();

            float s = f.strength;
            MMoveTowards(collision.gameObject.transform.position, s);


        }
    }

    void MMoveTowards(Vector2 location,float strength)
    {
        Debug.Log("MovingTowards");
        //location *= location * strength;

        transform.parent.position = Vector2.MoveTowards(transform.parent.position, location, strength * Time.deltaTime);
    }





}
