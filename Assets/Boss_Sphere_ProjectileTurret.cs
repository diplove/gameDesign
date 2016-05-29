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

	void Start () {
        player = GameObject.FindGameObjectWithTag("player").transform;
	
	}

    void FixedUpdate() {
        if (player.GetComponent<Collider2D>().IsTouching(hitBox)) {
            LookAtPlayer();
            FireTurret();
        } else {
        }


    }
	
	void Update () {

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
