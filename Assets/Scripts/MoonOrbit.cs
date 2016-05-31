using UnityEngine;
using System.Collections;

public class MoonOrbit : MonoBehaviour {

    public float orbitSpeed;
    public float moonMass;

    private Rigidbody2D rb;

    void Start() {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.mass = moonMass;
        rb.isKinematic = true;
        gameObject.AddComponent<CircleCollider2D>();
    }
    
    void FixedUpdate() {
        transform.RotateAround(transform.root.transform.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "player") {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.velocity.sqrMagnitude < 1) {
                if (Vector3.Angle(other.transform.up, transform.position - other.transform.position) > 170) {
                    other.gameObject.SendMessage("createFixedLock", gameObject);
                }

            } else if (playerRb.velocity.magnitude > 1.5) {
                other.gameObject.SendMessage("HitDamage", (playerRb.velocity.sqrMagnitude * playerRb.mass) * rb.mass);
            }
        }

    }
}
