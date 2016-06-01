﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss_Sphere_PhaseTwo : MonoBehaviour {

    public float health;
    public GameObject PhaseTwoCentreExplosionPoints;
    public GameObject PhaseTwoCentreExplosionPrefab;
    public GameObject MegaLaserLeftPoint;
    public GameObject MegaLaserRightPoint;
    public Material MegaLaserMaterial;

    // Mega Laser
    public float megaLaserDamage;
    public float megaLaserMaxDistance;

    private LineRenderer megaLeftlr;
    private LineRenderer megaRightlr;
    private LineRenderer sidelr;
    private bool laserFiringLeft = false;
    private bool laserFiringRight = false;
    private bool turningLeft = false;
    private bool turningRight = false;
    public float laserTime = 15f;
    private float rotationAmount;

    private ParticleSystem.EmissionModule leftEm;
    private ParticleSystem.EmissionModule rightEm;

    Vector3 direction = new Vector3();
    GameObject side = null;

    // State
    private bool leftSideRotated = false;
    private bool rightSideRotated = false;
    private bool normalMode = false;
    private bool initialSpawnCompleted = false;
    private bool PhaseTwoEventsRunning = false;
    private bool initialSpawning = false;

    // Generators
    public GameObject GeneratorOneTop;
    public GameObject GeneratorOneBottom;
    public GameObject GeneratorTwoTop;
    public GameObject GeneratorTwoBottom;
    private BoxCollider2D GeneratorOne;
    private BoxCollider2D GeneratorTwo;

    public float GeneratorHealth = 200f;

    private float GeneratorOneHealth;
    private float GeneratorTwoHealth;

    private bool GeneratorOneOnline = true;
    private bool GeneratorTwoOnline = true;

    private GameObject GeneratorCoreOne;
    private GameObject GeneratorCoreTwo;

    public GameObject generatorCore;

    // Turrets
    public GameObject[] turrets = new GameObject[4];
    public GameObject projectilePrefab;
    public int numberOfProjectiles;
    public float projectileDamage;


    private List<GameObject> bossProjectiles = new List<GameObject>();

    // Use this for initialization
    void Start () {

        megaLeftlr = MegaLaserLeftPoint.AddComponent<LineRenderer>();
        megaLeftlr.SetWidth(8, 8);
        megaLeftlr.material = MegaLaserMaterial;
        megaLeftlr.enabled = false;

        megaRightlr = MegaLaserRightPoint.AddComponent<LineRenderer>();
        megaRightlr.SetWidth(8, 8);
        megaRightlr.material = MegaLaserMaterial;
        megaRightlr.enabled = false;

        leftEm = MegaLaserLeftPoint.GetComponent<ParticleSystem>().emission;
        leftEm.enabled = false;
        rightEm = MegaLaserRightPoint.GetComponent<ParticleSystem>().emission;
        rightEm.enabled = false;

        GeneratorOne = MegaLaserLeftPoint.GetComponent<BoxCollider2D>();
        GeneratorTwo = MegaLaserRightPoint.GetComponent<BoxCollider2D>();
        GeneratorOneHealth = GeneratorHealth;
        GeneratorTwoHealth = GeneratorHealth;

        rotationAmount = 360f / laserTime;

        

        InstantiateProjectiles();

        OpeningExplosion();
	}

    void FixedUpdate() {
        if (!initialSpawnCompleted && !initialSpawning) {
            TriggerStartSequence();
        }
        if (!PhaseTwoEventsRunning && initialSpawnCompleted) {
            StartCoroutine(PhaseTwoEvents());
        }
        if (turningLeft) {
            transform.Rotate(Vector3.forward, -rotationAmount * Time.deltaTime);
        } else if (turningRight) {
            transform.Rotate(Vector3.forward, rotationAmount * Time.deltaTime);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        MegaLaser();
	}

    void OpeningExplosion() {
        for (int i = 0; i < PhaseTwoCentreExplosionPoints.transform.childCount; i++) {
            Instantiate(PhaseTwoCentreExplosionPrefab, PhaseTwoCentreExplosionPoints.transform.GetChild(i).transform.position, PhaseTwoCentreExplosionPoints.transform.GetChild(i).transform.rotation);
        }
        PhaseTwoCentreExplosionPoints.SetActive(false);
    }

    public void TriggerStartSequence() {
        initialSpawning = true;
        StartCoroutine(ActivateTurrets());
        SpawnCores();
        normalMode = true;
    }

    void SpawnCores() {
        GeneratorCoreOne = (GameObject)Instantiate(generatorCore, MegaLaserLeftPoint.transform.position, MegaLaserLeftPoint.transform.rotation);
        GeneratorCoreOne.transform.parent = MegaLaserLeftPoint.transform;
        GeneratorCoreTwo = (GameObject)Instantiate(generatorCore, MegaLaserRightPoint.transform.position, MegaLaserRightPoint.transform.rotation);
        GeneratorCoreTwo.transform.parent = MegaLaserRightPoint.transform;

    }

    IEnumerator ActivateTurrets() {
        yield return new WaitForSeconds(2);
        foreach (GameObject obj in turrets) {
            obj.SendMessage("Spawn");
        }

        yield return new WaitForSeconds(5);

        foreach (GameObject obj in turrets) {
            obj.SendMessage("ToggleVulnerable");
        }
        yield return new WaitForSeconds(2);
        initialSpawnCompleted = true;
    }

    IEnumerator PhaseTwoEvents() {

            PhaseTwoEventsRunning = true;
            if (!GeneratorOneOnline) {
                StartCoroutine(LaserChargeRight());
            } else if (!GeneratorTwoOnline) {
                StartCoroutine(LaserChargeLeft());
            } else if (Random.value < 0.25f) {
                if (Random.value < 0.5f) {
                    StartCoroutine(LaserChargeLeft());
                } else {
                    StartCoroutine(LaserChargeRight());
                }
            } else {
                StartCoroutine(NormalState());
            }

            //yield return new WaitUntil(() => !laserFiringLeft && !laserFiringRight);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    

    IEnumerator NormalState() {
            PhaseTwoEventsRunning = true;
            yield return new WaitForSeconds(20);
            PhaseTwoEventsRunning = false;
        }

    IEnumerator LaserChargeLeft() {
        GeneratorOne.enabled = false;
        GetComponent<Animator>().Play("BossPhaseTwoMegaLaser");
        leftEm.enabled = true;
        GeneratorCoreTwo.SetActive(false);
        while (!leftSideRotated) {
            yield return new WaitForSeconds(1);
        }
        laserFiringLeft = true;
        turningLeft = true;

        yield return new WaitForSeconds(laserTime); // Before mega laser stops
        turningLeft = false;
        laserFiringLeft = false;
        leftEm.enabled = false;
        megaLeftlr.enabled = false;
        GetComponent<Animator>().Play("BossPhaseTwoMegaLaserReverse");
        while (leftSideRotated) {
            yield return new WaitForSeconds(1);
        }
        GeneratorCoreTwo.SetActive(true);
        GeneratorOne.enabled = true;
        PhaseTwoEventsRunning = false;
    }

    IEnumerator LaserChargeRight() {
        GeneratorTwo.enabled = false;
        GetComponent<Animator>().Play("BossPhaseTwoMegaLaserR");
        rightEm.enabled = true;
        GeneratorCoreOne.SetActive(false);
        while (!rightSideRotated) {
            yield return new WaitForSeconds(1);
        }
        laserFiringRight = true;
        turningRight = true;

        yield return new WaitForSeconds(laserTime); // Before mega laser stops
        turningRight = false;

        laserFiringRight = false;
        rightEm.enabled = false;
        megaRightlr.enabled = false;
        GetComponent<Animator>().Play("BossPhaseTwoMegaLaserReverseR");
        while (rightSideRotated) {
            yield return new WaitForSeconds(1);
        }
        GeneratorCoreOne.SetActive(true);
        GeneratorTwo.enabled = true;
        PhaseTwoEventsRunning = false;
    }

    public void ToggleLeftRotated() {
        leftSideRotated = !leftSideRotated;
    }

    public void ToggleRightRotated() {
        rightSideRotated = !rightSideRotated;
    }

    void MegaLaser() {
        if (laserFiringLeft) {
            direction = -transform.right;
            side = MegaLaserLeftPoint;
            sidelr = megaLeftlr;
        } else if (laserFiringRight) {
            direction = transform.right;
            side = MegaLaserRightPoint;
            sidelr = megaRightlr;
        }

        if (laserFiringLeft || laserFiringRight) {
            sidelr.enabled = true;
            float laserMaterialTiling = MegaLaserMaterial.mainTextureOffset.x - 0.005f;
            MegaLaserMaterial.mainTextureOffset = new Vector2(laserMaterialTiling, 1);
            sidelr.SetPosition(0, side.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(side.transform.position, direction, megaLaserMaxDistance);

            if (hit.collider != null) {
                if (hit.collider.gameObject.tag == "player") {
                    sidelr.SetPosition(1, side.transform.position + (direction * megaLaserMaxDistance));
                    hit.collider.gameObject.SendMessage("HitDamage", megaLaserDamage);
                } else if (hit.collider.gameObject.tag == "projectile" || hit.collider.gameObject.tag == "enemyProjectile") {
                    sidelr.SetPosition(1, side.transform.position + (direction * megaLaserMaxDistance));
                    hit.collider.gameObject.SendMessage("Explode");
                }
            } else {
                sidelr.SetPosition(1, side.transform.position + (direction * megaLaserMaxDistance));
            }
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