using UnityEngine;
using System.Collections;

public class LockOnTargetController : MonoBehaviour {


    GameObject target;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("LockOnTarget");
        
    }
	
	// Update is called once per frame
	void Update () {


       
        
        
    }

    public void lockOnPosition(Vector3 pos)
    {
        transform.position = pos;
        
    }
}
