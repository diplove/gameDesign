using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetGravity : MonoBehaviour {

    //public CircleCollider2D planetPhysicalCollider;
    public float mass;
    public float gravConstant;
    public float gravityFieldSizeMultiplier = 10f;

    private CircleCollider2D planetPhysicalCollider;
    private CircleCollider2D gravitationalField;

    private List<Transform> objectsInGravity = new List<Transform>();

    void Start() {
        planetPhysicalCollider = transform.GetChild(0).GetComponent<PlanetController>().getCollider();
        gravitationalField = gameObject.AddComponent<CircleCollider2D>();
        gravitationalField.radius = planetPhysicalCollider.radius * gravityFieldSizeMultiplier;
        gravitationalField.isTrigger = true;
    }

    void FixedUpdate() {
        ApplyGravity();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "player") {
            if (gravitationalField.IsTouching(other)) {
                objectsInGravity.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "player") {
            if (!gravitationalField.IsTouching(other)) {
                objectsInGravity.Remove(other.transform);
            }
        }
    }

    void ApplyGravity() {
        foreach (Transform t in objectsInGravity) {
            Vector3 diff = transform.position - t.position;


            if (diff.magnitude > planetPhysicalCollider.radius + 2.5) { // Safe zone. Planet collider radius plus 2.5

                Vector3 direction = diff.normalized;
                Rigidbody2D tRb = t.GetComponent<Rigidbody2D>();

                float gravForce = (mass * tRb.mass * gravConstant) / diff.sqrMagnitude;

                tRb.AddForce(direction * gravForce);
            }
        }
    }
    
}
