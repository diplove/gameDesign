using UnityEngine;
using System.Collections;

public class Boss_Sphere_LaserTurret : MonoBehaviour {

    public float laserMaxDistance;
    public float laserDamage;
    public Material laserMaterial;
    public GameObject laserStartPoint;
    public GameObject turretExplosion;

    public float health;

    private LayerMask mask = ~(1 << 8 | 1 << 2);

    private LineRenderer lr;
    //private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;
    private bool isSpawned = false;
    private bool vulnerable = false;
    private bool firing = false;

	// Use this for initialization
	void Start () {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.SetWidth(1, 1);
        lr.material = laserMaterial;
        lr.SetColors(Color.red, Color.red);
        lr.enabled = false;

        em = laserStartPoint.GetComponent<ParticleSystem>().emission;
        em.enabled = false;
        StartCoroutine(GetComponentInParent<Boss_Sphere>().ActivateNewTurret(gameObject)); // TESTING
    }
	
	// Update is called once per frame
	void Update () {
        if (firing) {
            lr.enabled = true;
            lr.SetPosition(0, laserStartPoint.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(laserStartPoint.transform.position, transform.up, laserMaxDistance, mask);

            if (hit.collider != null) {
                if (hit.collider.gameObject.tag == "player") {
                    lr.SetPosition(1, hit.point);
                    hit.collider.gameObject.SendMessage("HitDamage", laserDamage);
                } else if (hit.collider.gameObject.tag == "enemyShip") {
                    lr.SetPosition(1, hit.point);
                    hit.collider.gameObject.SendMessage("HitDamage", laserDamage / 2);
                } else if (hit.collider.gameObject.tag == "projectile" || hit.collider.gameObject.tag == "enemyProjectile") {
                    lr.SetPosition(1, laserStartPoint.transform.position + (transform.up * laserMaxDistance));
                    hit.collider.gameObject.SendMessage("Explode");
                }
            } else {
                lr.SetPosition(1, laserStartPoint.transform.position + (transform.up * laserMaxDistance));
            }
        }
    }

    void hasSpawned() {

        isSpawned = true;
        if (GetComponentInParent<Boss_Sphere>())
        {
            if (GetComponentInParent<Boss_Sphere>().PhaseOneLoaded())
            {
                ToggleVulnerable();
            }
        }
    }

    void HitDamage(float damage) {
        if (isSpawned && vulnerable) {
            if ((health -= damage) <= 0) {
                Instantiate(turretExplosion, transform.GetChild(0).position, transform.GetChild(0).rotation);
                GetComponentInParent<Boss_Sphere>().TurretDestroyedTest(gameObject); // TESTING
            }
        }
    }

    public void ToggleVulnerable() {
        vulnerable = !vulnerable;
    }

    public void Spawn() {
        if (!isSpawned) {
            GetComponent<Animator>().Play("LaserTurretSpawn");
        }
    }

    void DestroySelf() {
        Instantiate(turretExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void DeactivateLaser() {
        firing = false;
        lr.enabled = false;
        em.enabled = false;
    }

    public void ActivateLaser() {
        if (firing == false) {
            StartCoroutine(LaserCharge());
        }
    }

    IEnumerator LaserCharge() {
        while (!isSpawned && !vulnerable) {
            yield return new WaitForSeconds(1);
        }
        em.enabled = true;
        yield return new WaitForSeconds(3);
        firing = true;
    }
}
