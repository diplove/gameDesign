using UnityEngine;
using System.Collections;

public class ObjectiveArrow : MonoBehaviour {

    public Transform target;
    //public Camera camera;

    void Update() {
        LookAtTarget();
        /*
        float dist = Vector3.Distance(target.position, transform.position);
        if (dist < 500) {

        } else {

        }
        */

        /*
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        //screenPos.x -= Screen.width/2;
        //screenPos.y -= Screen.height/2;
        */
    }

    void LookAtTarget() {
        Vector3 relative = transform.InverseTransformPoint(target.position);
        float angle = Mathf.Atan2(relative.y, relative.x);
        transform.Rotate(0, 0, angle);
    }
}