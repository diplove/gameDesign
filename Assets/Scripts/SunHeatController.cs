using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SunHeatController : MonoBehaviour {

    public float starMaxHeat;
    private List<Transform> heatedObjects = new List<Transform>();

    private float heatSum;

    void Start() {        
        CircleCollider2D heatField = gameObject.AddComponent<CircleCollider2D>();
        heatField.isTrigger = true;
        heatField.radius = transform.GetChild(0).GetComponent<SunController>().getColliderRadius() * 3f;

    }

    void FixedUpdate() {
        calculateDistance();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "player") {
            heatedObjects.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "player") {
            heatedObjects.Remove(other.transform);
        }
    }

    void calculateDistance() {
        
        foreach (Transform t in heatedObjects) {
            Vector3 diff = transform.position - t.position;
            Debug.Log(starMaxHeat / diff.magnitude);
            t.SendMessage("ApplyHeat", (int)(starMaxHeat / diff.magnitude));
        }
    }

}
