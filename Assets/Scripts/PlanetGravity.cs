using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetGravity : MonoBehaviour {

    public float mass;
    public float gravConstant;

    [SerializeField]
    private CircleCollider2D planetCollider;
    [SerializeField]
    private Collider2D gravDeadZone;

    private List<Transform> deadZoneObjects = new List<Transform>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        calculateDeadZone();
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "player") {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.velocity.sqrMagnitude < 1) {
                if (Vector3.Angle(other.transform.up, transform.position - other.transform.position) > 170) {
                    playerRb.velocity = new Vector2(0, 0);
                }
            } else {
                other.gameObject.SendMessage("HitDamage", (playerRb.velocity.sqrMagnitude * playerRb.mass) * mass);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            if (gravDeadZone.IsTouching(other)) {
                deadZoneObjects.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "player") {
            if (!gravDeadZone.IsTouching(other)) {
                deadZoneObjects.Remove(other.transform);
            }
        }
    }

    void calculateDeadZone()
    {
        foreach (Transform t in deadZoneObjects) {
            Vector3 diff = transform.position - t.position;


            if (diff.magnitude > planetCollider.radius + 2.5) { // Safe zone. Planet collider radius plus 2

                Vector3 direction = diff.normalized;
                Rigidbody2D tRb = t.GetComponent<Rigidbody2D>();

                float gravForce = (mass * tRb.mass * gravConstant) / diff.sqrMagnitude;

                tRb.AddForce(direction * gravForce);
            }
        }
    }
}
