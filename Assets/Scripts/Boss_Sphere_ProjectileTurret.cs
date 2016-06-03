using UnityEngine;
using System.Collections;

public class Boss_Sphere_ProjectileTurret : MonoBehaviour {

    public GameObject turretTop;
    public Collider2D hitBox;
    public GameObject[] shootPoints = new GameObject[2];
    public GameObject turretExplosion;

    private Transform player;



    public float projectileSpeed;
    public float timeBetweenSteps;
    private float lastStep;
    private bool isSpawned = false;
    private bool vulnerable = false;
    public float health;
    public float maxShotsBeforeCooldown = 15f;
    public float cooldownTime = 3f;
    private bool cooldownPeriod = false;
    private float shotCount = 0f;

    private AudioController ac;

 
	void Start () {
        player = GameObject.FindGameObjectWithTag("player").transform;
        StartCoroutine(GetComponentInParent<Boss_Sphere>().ActivateNewTurret(gameObject)); // TESTING
        ac = GameObject.Find("Audio").GetComponent<AudioController>();

    }

     public void hasSpawned() {
        isSpawned = true;
        if (GetComponentInParent<Boss_Sphere>().PhaseOneLoaded()) {
            ToggleVulnerable();
        }
    }

    public void Spawn() {
        if (!isSpawned) {
            GetComponent<Animator>().Play("BossProjectileTurretSpawn");
        }
    }
	
	void Update () {
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
                GetComponentInParent<Boss_Sphere>().TurretDestroyedTest(gameObject); // TESTING
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
        if (Time.time - lastStep > timeBetweenSteps && !cooldownPeriod) {
            GameObject p1 = GetComponentInParent<Boss_Sphere>().getProjectile();
            GameObject p2 = GetComponentInParent<Boss_Sphere>().getProjectile();
            p1.transform.position = shootPoints[0].transform.position;
            p1.transform.rotation = turretTop.transform.rotation;
            p2.transform.position = shootPoints[1].transform.position;
            p2.transform.rotation = turretTop.transform.rotation;
            p1.GetComponent<Rigidbody2D>().AddForce(p1.transform.up * projectileSpeed);
            p2.GetComponent<Rigidbody2D>().AddForce(p2.transform.up * projectileSpeed);
            lastStep = Time.time;
            ac.playTurretShoot();

            if (shotCount++ >= maxShotsBeforeCooldown) {
                cooldownPeriod = true;
                StartCoroutine(turretCooldown());
            }
        }
    }

    IEnumerator turretCooldown() {
        yield return new WaitForSeconds(cooldownTime);
        cooldownPeriod = false;
        shotCount = 0;
    }

    void LookAtPlayer() {
        Vector3 dir = player.position - transform.position;
         float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
         turretTop.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);         
    }
}
