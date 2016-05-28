using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserController : MonoBehaviour {

	public bool fire;

	public GameObject weaponHardpoint;

	public GameObject laser;

    public GameObject laserParticles;

    public float laserMaxDistance;

    private float damage;


	//public float offset; //To get the Laser to project from the vessel

	// Use this for initialization
	void Start () {
        laser = Instantiate(laser);
        laserParticles = Instantiate(laserParticles);
        laserParticles.SetActive(false);
		laser.SetActive (false);
        UpdateDamage();
	}

    void FixedUpdate() {

        if (fire == true) {
            laser.GetComponent<LineRenderer>().SetPosition(0, weaponHardpoint.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(weaponHardpoint.transform.position, transform.up, laserMaxDistance);

            if(hit.collider != null) {
                laser.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                laserParticles.transform.position = hit.point;
                laserParticles.SetActive(true);
                ApplyDamage(hit.collider.gameObject);
            } else {
                laser.GetComponent<LineRenderer>().SetPosition(1, weaponHardpoint.transform.position + (transform.up * laserMaxDistance));
                laserParticles.SetActive(false);
            }
            
           laser.SetActive(true);
        } else {
            laser.SetActive(false);
            laserParticles.SetActive(false);
        }
    }

    void ApplyDamage(GameObject target) {
        if (target.tag == "asteroid") {
            target.SendMessage("HitDamage", damage);
        }
    }

	public void activateLaser() {
		fire = true;
	}

	public void deactivateLaser() {
		fire = false;
	}

    void UpdateDamage() {
        damage = FindObjectOfType<PrototypePlayer>().primDamage;
    }

    public void setDamage(float damageAmount) {
        damage = damageAmount;
    }

    // Update is called once per frame
    void Update () {
		/*if (fire == true) {
            laser.SetActive(true);
            laser.transform.position = new Vector3 (weaponHardpoint.transform.position.x, weaponHardpoint.transform.position.y, -1);
			laser.transform.rotation = weaponHardpoint.transform.rotation;
		} else {
			laser.SetActive (false);
		}*/
	}
}
