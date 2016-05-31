using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetController : MonoBehaviour {

    private CircleCollider2D planetCollider;

    void Awake() {
        planetCollider = gameObject.AddComponent<CircleCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "player") {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb.velocity.sqrMagnitude < 1) {
                if (Vector3.Angle(other.transform.up, transform.position - other.transform.position) > 170) {
                    playerRb.velocity = new Vector2(0, 0);
                }
            } else if (playerRb.velocity.magnitude > 1.5) {
                other.gameObject.SendMessage("HitDamage", (playerRb.velocity.sqrMagnitude * playerRb.mass) * GetComponentInParent<PlanetGravity>().mass);
            }

        }
    }

    public CircleCollider2D getCollider() {
        return planetCollider;
    }

}
