using UnityEngine;
using System.Collections;

public class Boss_Sphere_LaserTurret : MonoBehaviour {

    public float laserMaxDistance;
    public float laserDamage;
    public Material laserMaterial;
    public GameObject laserStartPoint;

    public float health;

    private LineRenderer lr;
    private ParticleSystem ps;
    private bool isSpawned = false;

    private bool firing = false;

	// Use this for initialization
	void Start () {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.SetWidth(1, 1);
        lr.material = laserMaterial;
        lr.SetColors(Color.red, Color.red);
        lr.enabled = false;

        ps = lr.GetComponent<ParticleSystem>();
        var em = ps.emission;
        em.enabled = false;
    }
	
    void FixedUpdate() {
        if (firing) {
            lr.enabled = true;
            lr.SetPosition(0, laserStartPoint.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(laserStartPoint.transform.position, transform.up, laserMaxDistance);

            if (hit.collider != null) {
                lr.SetPosition(1, hit.point);
                Debug.Log(hit.collider.gameObject);
                if (hit.collider.gameObject.tag == "player") {
                    hit.collider.gameObject.SendMessage("HitDamage", laserDamage);
                } else if (hit.collider.gameObject.tag == "projectile") {
                    hit.collider.gameObject.SendMessage("Explode");
                }
            } else {
                lr.SetPosition(1, laserStartPoint.transform.position + (transform.up * laserMaxDistance));
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}

    void hasSpawned() {
        isSpawned = true;
    }

    void HitDamage(float damage) {
        if (isSpawned) {
            if ((health -= damage) <= 0) {
                DestorySelf();
            }
        }
    }

    public void Spawn() {
        if (!isSpawned) {
            GetComponent<Animator>().Play("LaserTurretSpawn");
        }
    }

    void DestorySelf() {
        isSpawned = false;
        DeactivateLaser();
        GetComponentInParent<Boss_Sphere>().TurretDestroyed();
        GetComponent<Animator>().Play("LaserTurretPreSpawn");
        StartCoroutine(GetComponentInParent<Boss_Sphere>().ActivateNewTurret(gameObject));

    }

   /* IEnumerator ActivateLaser() {
        if (firing == false) {
            yield return (LaserCharge());
            firing = true;
        }    
        lr.enabled = true;
        lr.SetPosition(0, laserStartPoint.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(laserStartPoint.transform.position, transform.up, laserMaxDistance);
        
        if (hit.collider != null) {
            lr.SetPosition(1, hit.point);
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.tag == "player") {
                hit.collider.gameObject.SendMessage("HitDamage", laserDamage);
            } else if (hit.collider.gameObject.tag == "projectile") {
                hit.collider.gameObject.SendMessage("Explode");
            }
        } else {
            lr.SetPosition(1, laserStartPoint.transform.position + (transform.up * laserMaxDistance));
        }
        yield return null;
    } */

    void DeactivateLaser() {
        firing = false;
        lr.enabled = false;
        var em = ps.emission;
        em.enabled = false;
    }

    /* IEnumerator LaserCharge() {
        Debug.Log("Particles Charge");
        var em = ps.emission;
        em.enabled = true;
        yield return new WaitForSeconds(5);
    } */

    void ActivateLaser() {
        if (firing == false) {
            StartCoroutine(LaserCharge());
            firing = true;
        }
    }

    IEnumerator LaserCharge() {
        Debug.Log("Laser Charging");
        var em = ps.emission;
        em.enabled = true;
        yield return new WaitForSeconds(5);
    }
}
