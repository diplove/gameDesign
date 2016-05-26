using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserController : MonoBehaviour {

	public bool fire;

	public GameObject weaponHardpoint;

	public GameObject laser;

	public float offset; //To get the Laser to project from the vessel

	// Use this for initialization
	void Start () {
		laser.SetActive (false);
	}

	public void activateLaser() {
		fire = true;
	}

	public void deactivateLaser() {
		fire = false;
	}

	// Update is called once per frame
	void Update () {
		if (fire == true) {
			laser.transform.position = new Vector3 (weaponHardpoint.transform.position.x, weaponHardpoint.transform.position.y+offset, -1);
			laser.transform.rotation = weaponHardpoint.transform.rotation;
			laser.SetActive (true);
		} else {
			laser.SetActive (false);
		}
	}
}
