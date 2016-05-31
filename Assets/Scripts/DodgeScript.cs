using UnityEngine;
using System.Collections;

public class DodgeScript : MonoBehaviour
{
    public GameObject EnemyShip;
    public float DodgeCoolDown = 0.5f;

    private float force;
    private float dodgeCoolDown;
	// Use this for initialization
	void Start ()
    {
        dodgeCoolDown = -1f;
        force = EnemyShip.GetComponent<EnemyShipScript>().MaxForce;
	}
	
	// Update is called once per frame
	void Update ()
    {
        dodgeCoolDown -= Time.deltaTime;
	}

    void OnTriggerEnter2d(Collider2D collider)
    {
        if (dodgeCoolDown < 0f)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity.normalized * force);

            dodgeCoolDown = DodgeCoolDown;
        }
    }
}
