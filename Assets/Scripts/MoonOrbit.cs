using UnityEngine;
using System.Collections;

public class MoonOrbit : MonoBehaviour {

    public float orbitSpeed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        transform.RotateAround(transform.parent.transform.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }
}
