using UnityEngine;
using System.Collections;

public class SupportSystemInertialDampener : MonoBehaviour
{

    private Rigidbody2D vesselRb;
    private PrototypePlayer vessel;

    // Use this for initialization
    void Start()
    {
        vessel = GetComponent<PrototypePlayer>();
        vesselRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            vesselRb.drag = 5;
            vessel.ApplyHeat(100);

        }
        Debug.Log(vesselRb.velocity);

        if (Input.GetKeyUp(KeyCode.A))
        {
            vesselRb.drag = 0;
        }
    }
}
