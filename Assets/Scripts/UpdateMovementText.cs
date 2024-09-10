using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMovementText : MonoBehaviour
{
    private ShipMovement ship;
    private TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        //TODO: Temporal find use, should use events
        ship = GameObject.Find("Ship").GetComponent<ShipMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(ship.movementControler.ToString());
    }
}
