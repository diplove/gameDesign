using UnityEngine;
using System.Collections;

public class PrototypePlayer : MonoBehaviour {
	//public rigidbody2D rb = GetComponent<Rigidbody2D> (); //apparently depreciated
	public float maxSpeed;
	public float turnRate;
	public float acelRate;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKey(KeyCode.UpArrow)) {
			Accelerate ();
		}
		*/

	}

	void OnCollisionEnter2D (Collision2D coll) {
		//if (coll.gameObject.tag == "PlanetGravity") {
		//
		//}
	}

	/*
	void Accelerate () {
		rb.AddForce (acelRate);
	}
	*/
}
