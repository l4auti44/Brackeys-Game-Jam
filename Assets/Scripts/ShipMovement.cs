using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float speedForce = 100f;
    [HideInInspector] public int movementControler = 1;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Add new Input System
        if (Input.GetKey(KeyCode.A))
        {
            _rb.AddForce(Vector2.left * speedForce * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rb.AddForce(Vector2.right * speedForce * Time.deltaTime * movementControler);
        }
    }

    public void IncreaseMovement()
    {
        if (movementControler < 3)
        {
            movementControler++;
        }
        
    }
    public void DecreaseMovement()
    {
        if (movementControler > 0)
        {
            movementControler--;
        }
    }

}
