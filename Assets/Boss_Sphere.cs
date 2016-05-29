using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_Sphere : MonoBehaviour {

    public float outerRotationSpeed;
    public int numberOfProjectiles;
    public float projectileDamage;

    private List<GameObject> bossProjectiles = new List<GameObject>();



    // Inspector Assigned Variables
    public GameObject[] turrets = new GameObject[4];
    public GameObject projectilePrefab;


    void Start() {
        InstantiateProjectiles();
    }
    

    void FixedUpdate() {
        RotateOuterTurrets();
    }

    void Update() {

    }

    void RotateOuterTurrets() {
        foreach (GameObject obj in turrets) {
            obj.transform.RotateAround(transform.position, Vector3.forward, outerRotationSpeed * Time.deltaTime);
        }
    }

    void InstantiateProjectiles() {
        for (int i = 0; i < numberOfProjectiles; i++) { 
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            bossProjectiles.Add(projectile);
        }
    }

    public GameObject getProjectile() {
        foreach (GameObject obj in bossProjectiles) {
            if (obj.activeInHierarchy == false) {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject newProj = Instantiate(projectilePrefab);
        bossProjectiles.Add(newProj);
        return newProj;
    }
}
