using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser2Controller : MonoBehaviour {

	public bool fire;

	public GameObject weaponHardPoint;

	public GameObject laser;

	public GameObject laserParticles;

	public float laserMaxDistance;

	private float damage;

    private LayerMask mask = ~(1 << 8 | 1 << 2);

    // Use this for initialization
    void Start () {
		laser = Instantiate(laser);
		laser.GetComponent<LineRenderer>().sortingLayerName = "Projectiles";
		laserParticles = Instantiate(laserParticles);
		laserParticles.SetActive(false);
		laser.SetActive (false);
		UpdateDamage();
	}

    void ApplyDamage(GameObject target) {
        switch (target.gameObject.tag) {
            case "asteroid":
                target.transform.SendMessage("HitDamage", damage);
                break;
            case "bossChild":
                target.gameObject.SendMessage("HitDamage", damage);
                break;
            case "boss":
                target.gameObject.SendMessage("HitDamage", gameObject);
                break;
            case "enemyShip":
                target.gameObject.SendMessage("HitDamage", damage);
                break;
            case "enemyProjectile":
                target.gameObject.SendMessage("Explode");
                break;
            default:
                //Debug.Log("Laser " + gameObject + " encountered object with no tag handler");
                break;
        }
    }

    public void activateLaser() {
		fire = true;
	}

	public void deactivateLaser() {
		fire = false;
	}

	void UpdateDamage() {
		damage = FindObjectOfType<PrototypePlayer>().auxDamage;
	}

	public void setDamage(float damageAmount) {
		damage = damageAmount;
	}

	// Update is called once per frame
	void Update () {
		if (fire == true) {
			laser.GetComponent<LineRenderer>().SetPosition(0, weaponHardPoint.transform.position);
			RaycastHit2D hit = Physics2D.Raycast(weaponHardPoint.transform.position, weaponHardPoint.transform.up, laserMaxDistance, mask);

			if (hit.collider != null) {
                Debug.Log("Hit!");
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
	}
}
