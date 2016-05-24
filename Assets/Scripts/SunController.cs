using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SunController : MonoBehaviour {

    public float starMaxHeat;
    private List<Transform> heatedObjects = new List<Transform>();

    private float heatSum;

    void FixedUpdate() {
        calculateDistance();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "player") {
            heatedObjects.Add(other.transform);
            Debug.Log("Added player to heated objects");
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
            t.SendMessage("ApplyHeat", (int)(starMaxHeat / diff.magnitude));
        }
    }

}
