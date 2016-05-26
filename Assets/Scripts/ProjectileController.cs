using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour {

    private float lastStep;
    public float timeBetweenSteps;

    public int initialNormalProjectileAmount;
    public int maxNormalProjectiles;
    public int normalProjectileSpeed;

    // Lists of projectiles
    private List<GameObject> normalProjectiles = new List<GameObject>();
    
    // Inspector assigned projectiles
    public GameObject normalProjectile;

    public GameObject WeaponOneShootPoint; // Point where the first weapon shoots from
    public GameObject WeaponTwoShootPoint; // ^^ Second weapon



    // Use this for initialization
    void Start () {
        // Instantiate the initial amount of normal projectiles - Object Pooling
        for (int i = 0; i < initialNormalProjectileAmount; i++) {
            GameObject np = Instantiate(normalProjectile);
            np.SetActive(false);
            normalProjectiles.Add(np);

        }	
	}
	
    GameObject newProjectile() {
        // Find inactive projectile in list
        foreach (GameObject projectile in normalProjectiles) {
            if (projectile.activeInHierarchy == false) {
                projectile.SetActive(true);
                return projectile;
            }
        }
        // If there are no avaliable inactive projectiles, make a new one. - Dynamic
        GameObject np = Instantiate(normalProjectile);
        normalProjectiles.Add(np);
        return np;
    }

    public void shootNormalProjectile() {

        if (Time.time - lastStep > timeBetweenSteps) {
            GameObject p1 = newProjectile();
            GameObject p2 = newProjectile();
            p1.transform.position = WeaponOneShootPoint.transform.position;
            p1.transform.rotation = transform.rotation;
            p2.transform.position = WeaponTwoShootPoint.transform.position;
            p2.transform.rotation = transform.rotation;

            p1.GetComponent<Rigidbody2D>().AddForce(p1.transform.up * normalProjectileSpeed);
            p2.GetComponent<Rigidbody2D>().AddForce(p2.transform.up * normalProjectileSpeed);
            lastStep = Time.time;
        }

       // Debug.Log(normalProjectiles.Count + " : " + maxNormalProjectiles + " (" + initialNormalProjectileAmount + ")");
    }

}
