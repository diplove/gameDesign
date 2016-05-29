using UnityEngine;
using System.Collections;

public class Boss_Sphere_ProjectileTurret : MonoBehaviour {

    public GameObject turretTop;
    public Collider2D hitBox;
    public GameObject[] shootPoints = new GameObject[2];

    private Transform player;

    public float projectileSpeed;
    public float timeBetweenSteps;
    private float lastStep;
    private bool isSpawned = false;

    public float health;

 
	void Start () {
        player = GameObject.FindGameObjectWithTag("player").transform;
	
	}

    void FixedUpdate() {
        if (isSpawned) {
            if (player.GetComponent<Collider2D>().IsTouching(hitBox)) {
                LookAtPlayer();
                FireTurret();
            } 
        }


    }

    public void hasSpawned() {
        isSpawned = true;
    }

    public void Spawn() {
        if (!isSpawned) {
            GetComponent<Animator>().Play("BossProjectileTurretSpawn");
        }
    }
	
	void Update () {

    }

    void HitDamage(float damage) {
        if (isSpawned) {
            if ((health -= damage) < 0) {
                DestroySelf();
            } 
        }
    }

    void DestroySelf() {
        isSpawned = false;
        GetComponentInParent<Boss_Sphere>().TurretDestroyed();
        GetComponent<Animator>().Play("BossProjectileTurretPreSpawn");
        turretTop.transform.rotation = new Quaternion(0, 0, 0, 0);
        StartCoroutine(GetComponentInParent<Boss_Sphere>().ActivateNewTurret(gameObject));
    }

    void FireTurret() {
        if (Time.time - lastStep > timeBetweenSteps) {
            GameObject p1 = GetComponentInParent<Boss_Sphere>().getProjectile();
            GameObject p2 = GetComponentInParent<Boss_Sphere>().getProjectile();
            p1.transform.position = shootPoints[0].transform.position;
            p1.transform.rotation = turretTop.transform.rotation;
            p2.transform.position = shootPoints[1].transform.position;
            p2.transform.rotation = turretTop.transform.rotation;
            p1.GetComponent<Rigidbody2D>().AddForce(p1.transform.up * projectileSpeed);
            p2.GetComponent<Rigidbody2D>().AddForce(p2.transform.up * projectileSpeed);
            lastStep = Time.time;
        }
    }

    void LookAtPlayer() {
        Vector3 dir = player.position - transform.position;
         float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
         turretTop.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);         
    }
}
