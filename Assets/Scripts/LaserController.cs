using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserController : MonoBehaviour {

	public bool fire;
	//public bool fire2; // Aux Laser

	public GameObject weaponHardPoint;

	public GameObject laser;
	//public GameObject laser2; // Aux Laser

    public GameObject laserParticles;
	//public GameObject laserParticles2; // Aux Laser

    public float laserMaxDistance;
	//public float laser2MaxDistance; // Aux Laser

    private float damage;
	//private float damage2; // Aux Laser


	//public float offset; //To get the Laser to project from the vessel

	// Use this for initialization
	void Start () {
        laser = Instantiate(laser);
		//laser2 = Instantiate(laser2);
        laser.GetComponent<LineRenderer>().sortingLayerName = "Projectiles";
		//laser2.GetComponent<LineRenderer>().sortingLayerName = "Projectiles";
        laserParticles = Instantiate(laserParticles);
        laserParticles.SetActive(false);
		//laserParticles2 = Instantiate(laserParticles2);
		//laserParticles2.SetActive(false);
		laser.SetActive (false);
		//laser2.SetActive (false);
        UpdateDamage();
	}

    void FixedUpdate() {


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
	/*
	public void activateLaser2() {
		fire2 = true;
	}

	public void deactivateLaser2() {
		fire2 = false;
	}
	*/
    void UpdateDamage() {
        damage = FindObjectOfType<PrototypePlayer>().primDamage;
		//damage2 = FindObjectOfType<PrototypePlayer>().auxDamage;
    }

    public void setDamage(float damageAmount) {
        damage = damageAmount;
    }

	public void setDamage2(float damageAmount) {
		//damage2 = damageAmount;
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

        if (fire == true) {
            laser.GetComponent<LineRenderer>().SetPosition(0, weaponHardPoint.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(weaponHardPoint.transform.position, weaponHardPoint.transform.up, laserMaxDistance);

            if (hit.collider != null) {
                laser.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                laserParticles.transform.position = hit.point;
                laserParticles.SetActive(true);
                ApplyDamage(hit.collider.gameObject);
            } else {
                laser.GetComponent<LineRenderer>().SetPosition(1, weaponHardPoint.transform.position + (weaponHardPoint.transform.up * laserMaxDistance));
                laserParticles.SetActive(false);
            }

            laser.SetActive(true);
        } else {
            laser.SetActive(false);
            laserParticles.SetActive(false);
        }

		/*
		if (fire2 == true) {
			laser2.GetComponent<LineRenderer>().SetPosition(0, weaponHardPoint.transform.position);
			RaycastHit2D hit = Physics2D.Raycast(weaponHardPoint.transform.position, weaponHardPoint.transform.up, laser2MaxDistance);

			if (hit.collider != null) {
				laser2.GetComponent<LineRenderer>().SetPosition(1, hit.point);
				laserParticles2.transform.position = hit.point;
				laserParticles2.SetActive(true);
				ApplyDamage(hit.collider.gameObject);
			} else {
				laser2.GetComponent<LineRenderer>().SetPosition(1, weaponHardPoint.transform.position + (weaponHardPoint.transform.up * laser2MaxDistance));
				laserParticles2.SetActive(false);
			}

			laser2.SetActive(true);
		} else {
			laser2.SetActive(false);
			laserParticles2.SetActive(false);
		}
		*/
    }
}
