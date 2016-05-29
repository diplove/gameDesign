using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_Sphere : MonoBehaviour {

    public float outerRotationSpeed;
    public int numberOfProjectiles;
    public float projectileDamage;
    public float health;

    private List<GameObject> bossProjectiles = new List<GameObject>();
    private int battlePhase = 1;



    // Inspector Assigned Variables
    public GameObject[] turrets = new GameObject[4];
    public GameObject projectilePrefab;

    // Phase 1 parameters
    private bool InitialTurretsActivated = false;
    private bool isSpawningInitial = false;
    


    void Start() {
        InstantiateProjectiles();
    }
    

    void FixedUpdate() {
        if (battlePhase == 1) {
            RotateOuterTurrets();
            if (!InitialTurretsActivated && !isSpawningInitial) {
                StartCoroutine(ActivateAllTurrets());
            }
        }
    }

    void Update() {

    }

    public void TurretDestroyed() {
        health -= 1000;
        if (health <= 0) {
            battlePhase = 2;
        }
    }

    void HitDamage(GameObject obj) {
        if (obj.GetComponent<NormalProjectileController>()) {
            obj.GetComponent<Rigidbody2D>().velocity = obj.GetComponent<Rigidbody2D>().velocity * -0.5f; // Reflect Projectiles
            obj.transform.up = -obj.transform.up;
        }
    }

    void RotateOuterTurrets() {
        foreach (GameObject obj in turrets) {
            obj.transform.RotateAround(transform.position, Vector3.forward, outerRotationSpeed * Time.deltaTime);
        }
    }

    /*public void ActivateNewTurret() {
        if (battlePhase == 1) {
            StartCoroutine(TurretActivate());
            foreach (GameObject obj in turrets) {
                obj.SendMessage("Spawn");
            }
        }
    } */

    public IEnumerator ActivateNewTurret(GameObject obj) {
        if (battlePhase == 1) {
            yield return new WaitForSeconds(5);
            obj.SendMessage("Spawn");
        }
    }

    IEnumerator ActivateAllTurrets() {
        isSpawningInitial = true;
        foreach (GameObject obj in turrets) {
            obj.SendMessage("Spawn");
            yield return new WaitForSeconds(5);
        }
        InitialTurretsActivated = true;
    }


    IEnumerator TurretActivate() {
        yield return new WaitForSeconds(3);
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
