﻿using UnityEngine;
using System.Collections;

public class OrbitalPlatformController : MonoBehaviour {

    public float orbitSpeed = 5f;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
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
                    Debug.Log("Fixed Lock");
                }

            } else if (playerRb.velocity.magnitude > 1.5) {
                if (rb)
                {
                    other.gameObject.SendMessage("HitDamage", (playerRb.velocity.sqrMagnitude * playerRb.mass) * rb.mass);
                }
            }
        }

    }
}
