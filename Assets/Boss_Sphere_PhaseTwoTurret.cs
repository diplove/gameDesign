using UnityEngine;
using System.Collections;

public class Boss_Sphere_PhaseTwoTurret : MonoBehaviour {

    public GameObject turretTop;
    public Collider2D hitBox;
    public GameObject shootPoint;
    public GameObject turretExplosion;

    private Transform player;

    public float projectileSpeed;
    public float timeBetweenSteps;
    private float lastStep;
    private bool isSpawned = false;
    private bool vulnerable = false;
    public float health;

    void Start() {
        player = GameObject.FindGameObjectWithTag("player").transform;
        //gameObject.GetComponent<Animator>().Play("BossProjectileTurretPreSpawn");
    }

    public void hasSpawned() {
        isSpawned = true;
    }

    public void Spawn() {
        if (!isSpawned) {
            GetComponent<Animator>().Play("BossProjectileTurretSpawn");
        }
    }

    void Update() {
        if (isSpawned && vulnerable) {
            if (player.GetComponent<Collider2D>().IsTouching(hitBox) && !player.GetComponent<PrototypePlayer>().getDeathState()) {
                LookAtPlayer();
                FireTurret();
            }
        }

    }

    void HitDamage(float damage) {
        if (isSpawned && vulnerable) {
            if ((health -= damage) < 0) {
                Instantiate(turretExplosion, transform.position, transform.rotation);
            }
        }
    }

    public void ToggleVulnerable() {
        vulnerable = !vulnerable;
    }

    void DestroySelf() {
        Instantiate(turretExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void FireTurret() {
        if (Time.time - lastStep > timeBetweenSteps) {
            GameObject p1 = GetComponentInParent<Boss_Sphere_PhaseTwo>().getProjectile();
            p1.transform.position = shootPoint.transform.position;
            p1.transform.rotation = turretTop.transform.rotation;
            p1.GetComponent<Rigidbody2D>().AddForce(p1.transform.up * projectileSpeed);
            lastStep = Time.time;
        }
    }

    void LookAtPlayer() {
        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        turretTop.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

}
