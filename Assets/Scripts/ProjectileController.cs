﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour {

    private float lastStep;
    public float timeBetweenSteps;

    public int initialNormalProjectileAmount;
    public int maxNormalProjectiles;
    public int normalProjectileSpeed;

	public int numWeaponPorts; // Number of weapon ports used

	//public int projectileDamage; // Used by turrets

    // Lists of projectiles
    private List<GameObject> normalProjectiles = new List<GameObject>();
    
    // Inspector assigned projectiles
    public GameObject normalProjectile;

    public GameObject WeaponOneShootPoint; // Point where the first weapon shoots from
    public GameObject WeaponTwoShootPoint; // ^^ Second weapon
	public GameObject WeaponThreeShootPoint; // ^^ Third weapon
	public GameObject WeaponFourShootPoint; // ^^ Fourth weapon



    // Use this for initialization
    void Start () {
        // Instantiate the initial amount of normal projectiles - Object Pooling
        for (int i = 0; i < initialNormalProjectileAmount; i++) {
            GameObject np = Instantiate(normalProjectile);
            normalProjectiles.Add(np);
            switch(gameObject.tag)
            {
                case "player":
                    np.GetComponent<NormalProjectileController>().setDamage(GetComponent<PrototypePlayer>().primDamage);
                    np.tag = "projectile";
                    break;
                case "enemyShip":
                    np.GetComponent<NormalProjectileController>().setDamage(GetComponent<EnemyShipScript>().ShotDamage);
                    np.tag = "enemyProjectile";
                    break;
                case "enemyTurret":
                    np.GetComponent<NormalProjectileController>().setDamage(GetComponent<EnemyTurret>().damage);
                    np.tag = "enemyProjectile";
                    break;
            }
            np.SetActive(false);
        }	
	}
	
    GameObject newProjectile() {
        // Find inactive projectile in list
        foreach (GameObject projectile in normalProjectiles) {
            if (projectile.activeInHierarchy == false) {
                projectile.SetActive(true);
                Debug.Log("Object: " + gameObject + " || Damage: " + projectile.GetComponent<NormalProjectileController>().getDamage());
                if(gameObject.tag == "player")
                {
                    projectile.tag = "projectile";
                }
                return projectile;
            }
        }
        // If there are no avaliable inactive projectiles, make a new one. - Dynamic
        GameObject np = Instantiate(normalProjectile);
		normalProjectiles.Add(np);
        switch (gameObject.tag)
        {
            case "player":
                np.GetComponent<NormalProjectileController>().setDamage(GetComponent<PrototypePlayer>().primDamage);
                np.tag = "projectile";
                break;
            case "enemyShip":
                np.GetComponent<NormalProjectileController>().setDamage(GetComponent<EnemyShipScript>().ShotDamage);
                np.tag = "enemyProjectile";
                break;
            case "enemyTurret":
                np.GetComponent<NormalProjectileController>().setDamage(GetComponent<EnemyTurret>().damage);
                np.tag = "enemyProjectile";
                break;
        }


        return np;
    }

    public void shootNormalProjectile() {

        if (Time.time - lastStep > timeBetweenSteps) {
            GameObject p1 = newProjectile();
            
            p1.transform.position = WeaponOneShootPoint.transform.position;
			p1.transform.rotation = transform.rotation;
            p1.GetComponent<Rigidbody2D>().AddForce(p1.transform.up * normalProjectileSpeed);
            
			if (numWeaponPorts > 1) {
				GameObject p2 = newProjectile();

				p2.transform.position = WeaponTwoShootPoint.transform.position;
				p2.transform.rotation = transform.rotation;
				p2.GetComponent<Rigidbody2D>().AddForce(p2.transform.up * normalProjectileSpeed);
			}

			if (numWeaponPorts > 2) {
				GameObject p3 = newProjectile();

				p3.transform.position = WeaponThreeShootPoint.transform.position;
				p3.transform.rotation = transform.rotation;
				p3.GetComponent<Rigidbody2D>().AddForce(p3.transform.up * normalProjectileSpeed);
			}

			if (numWeaponPorts > 3) {
				GameObject p4 = newProjectile();

				p4.transform.position = WeaponFourShootPoint.transform.position;
				p4.transform.rotation = transform.rotation;
				p4.GetComponent<Rigidbody2D>().AddForce(p4.transform.up * normalProjectileSpeed);
			}

            lastStep = Time.time;
        }

       // Debug.Log(normalProjectiles.Count + " : " + maxNormalProjectiles + " (" + initialNormalProjectileAmount + ")");
    }

}
