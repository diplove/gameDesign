using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SunController : MonoBehaviour {

    private CircleCollider2D physicalCollider;

    void Awake() {
        physicalCollider = gameObject.AddComponent<CircleCollider2D>();
        physicalCollider.radius *= 0.85f;

    }

    void OnCollisionEnter2D(Collision2D other) {
        other.gameObject.SendMessage("DestroySelf");
        
    }

    public float getColliderRadius() {
        return physicalCollider.radius;
    }

}
