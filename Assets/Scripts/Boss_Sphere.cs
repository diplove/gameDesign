using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_Sphere : MonoBehaviour {

    public float outerRotationSpeed;
    public float maxOuterRotationSpeed;
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
    private bool phaseOneEventRunning = false;

    // Turret Prefabs
    public GameObject laserTurretPrefab;
    public GameObject projectileTurretPrefab;

    // Special
    private GameObject mainCamera;
    private GameObject uiCanvas;


    void Start() {
        InstantiateProjectiles();
        mainCamera = GameObject.Find("Main Camera");
        uiCanvas = GameObject.Find("Canvas");
       // mainCamera.GetComponent<followPlayer>().SetTarget(gameObject.transform);
       // uiCanvas.SetActive(false);
    }
    

    void FixedUpdate() {
        if (battlePhase == 1) {
            RotateOuterTurrets();
            if (!InitialTurretsActivated && !isSpawningInitial) {
                if ((outerRotationSpeed *= 1.001f) > maxOuterRotationSpeed) {
                    outerRotationSpeed = maxOuterRotationSpeed;
                }
                StartCoroutine(ActivateAllTurrets());
            }
        }
    }

    void Update() {
        if (!phaseOneEventRunning && battlePhase == 1 && InitialTurretsActivated) {
            StartCoroutine(PhaseOneEvents());
        } else {

        }
    }

    public IEnumerator PhaseOneEvents() {
        phaseOneEventRunning = true;
        ActivateLaserTurrets();
        yield return new WaitForSeconds((int)Random.Range(10, 20));
        DeactivateLaserTurrets();
        yield return new WaitForSeconds((int)Random.Range(1, 10));
        phaseOneEventRunning = false;

    }

    void ActivateLaserTurrets() {
        foreach (GameObject obj in turrets) {
            if (obj.GetComponent<Boss_Sphere_LaserTurret>()) {
                obj.GetComponent<Boss_Sphere_LaserTurret>().ActivateLaser();
            }
        }
    }

    void DeactivateLaserTurrets() {
        foreach (GameObject obj in turrets) {
            if (obj.GetComponent<Boss_Sphere_LaserTurret>()) {
                obj.GetComponent<Boss_Sphere_LaserTurret>().DeactivateLaser();
            }
        }
    }

    public void TurretDestroyedTest(GameObject obj) {
        health -= 1000;
        if (health <= 0) {
            battlePhase = 2;
            ChangeToPhaseTwo();
        } else {
            RespawnTurret(obj);
        }
    }

    void RespawnTurret(GameObject obj) {
        Vector3 pos = obj.transform.position;
        Quaternion rot = obj.transform.rotation;
        int arrayPos = 0;
        for (int i = 0; i < 4; i++) {
            if (turrets[i] == obj) {
                arrayPos = i;
                break;
            }
        }
        obj.SetActive(false);
        if (Random.value >= 0.5) {
            turrets[arrayPos] = (GameObject)Instantiate(projectileTurretPrefab, pos, rot);
        } else {
            turrets[arrayPos] = (GameObject)Instantiate(laserTurretPrefab, pos, rot);
            if (phaseOneEventRunning) {
                turrets[arrayPos].GetComponent<Boss_Sphere_LaserTurret>().ActivateLaser();
            }
        }
        turrets[arrayPos].transform.parent = gameObject.transform;
        Destroy(obj);
    }
    

    public bool PhaseOneLoaded() {
        return InitialTurretsActivated;
    }

    void HitDamage(GameObject obj) {
        if (obj.GetComponent<NormalProjectileController>()) {
            Debug.Log("Is hit");
            obj.GetComponent<Rigidbody2D>().velocity = obj.GetComponent<Rigidbody2D>().velocity * -0.5f; // Reflect Projectiles
            obj.transform.up = -obj.transform.up;
            obj.tag = "enemyProjectile";
        }
    }

    void RotateOuterTurrets() {
        foreach (GameObject obj in turrets) {
            obj.transform.RotateAround(transform.position, Vector3.forward, outerRotationSpeed * Time.deltaTime);
        }
    }

    public IEnumerator ActivateNewTurret(GameObject obj) {
        if (battlePhase == 1 && InitialTurretsActivated) {
            yield return new WaitForSeconds(5);
            obj.SendMessage("Spawn");
        }
    }

    IEnumerator ActivateAllTurrets() {
        isSpawningInitial = true;
        foreach (GameObject obj in turrets) {
            obj.SendMessage("Spawn");
            yield return new WaitForSeconds(2.5f);
        }

        mainCamera.GetComponent<followPlayer>().SetTarget(GameObject.FindGameObjectWithTag("player").transform);
        uiCanvas.SetActive(true);

        yield return new WaitForSeconds(2);
        InitialTurretsActivated = true;
        foreach (GameObject obj in turrets) {
            obj.SendMessage("ToggleVulnerable");
        }

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

    void ChangeToPhaseTwo() {
        StopAllCoroutines();
        DeactivateLaserTurrets();    
        
        foreach(GameObject obj in turrets) {
            obj.SendMessage("ToggleVulnerable");
        }
        StartCoroutine(TestDetatch());
    }

    IEnumerator TestDetatch() {
        mainCamera.GetComponent<followPlayer>().SetTarget(gameObject.transform);
        uiCanvas.SetActive(false);

        yield return new WaitForSeconds(2);

        foreach (GameObject obj in turrets) {
            obj.transform.parent = null;
           Rigidbody2D objrb = obj.AddComponent<Rigidbody2D>();
            objrb.AddForce(obj.transform.up * 100);
        }

        yield return new WaitForSeconds(3);

        foreach (GameObject obj in turrets) {
            obj.SendMessage("DestroySelf");
        }

        yield return new WaitForSeconds(2);

        mainCamera.GetComponent<followPlayer>().SetTarget(GameObject.FindGameObjectWithTag("player").transform);
        uiCanvas.SetActive(true);
        StopAllCoroutines();
    }


}
