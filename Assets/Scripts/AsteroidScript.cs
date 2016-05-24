using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour
{
    public float MinTorque = -100f;
    public float MaxTorque = 100f;
    public float MinForce = 20f;
    public float MaxForce = 40f;

	void Start ()
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        float magnitude = Random.Range(MinForce, MaxForce);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        rb2d.AddForce(magnitude * new Vector2(x, y));

        float torque = Random.Range(MinTorque, MaxTorque);
        rb2d.AddTorque(torque);
	}
	
	void Update ()
    {

	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player")
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.velocity.sqrMagnitude < 1)
            {
                if (Vector3.Angle(other.transform.up, transform.position - other.transform.position) > 170)
                {
                    other.gameObject.SendMessage("createFixedLock", gameObject);
                }

            }
            else if (playerRb.velocity.magnitude > 1.5)
            {
                other.gameObject.SendMessage("HitDamage", (playerRb.velocity.sqrMagnitude * playerRb.mass) * GetComponent<Rigidbody2D>().mass);
            }
        }

    }
}
