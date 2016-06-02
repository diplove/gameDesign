using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceMineController : MonoBehaviour {

    private Rigidbody2D rb;
    private List<Transform> targets = new List<Transform>();
    private Vector3 startPos;

    public GameObject physicalMine;
    public GameObject explosionPrefab;
    public Collider2D aggroRadius;
    public Collider2D explodeRadius;
    public float health = 200f;
    public float speedConstant = 4f;
    public float explosionRadius = 10f;
    public float explosionForce = 100f;
    public float damage = 400f;


    void Start() {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (targets.Count == 0) {
            ResetPosition();
        } else {
            MoveTowardsTarget();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "player") {
            if (other.IsTouching(explodeRadius)) {
                Explode();
            } else if (other.IsTouching(aggroRadius)) {
                targets.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "player") {
            if (!other.IsTouching(aggroRadius)) {
                targets.Remove(other.transform);
            }
        }
        rb.velocity = Vector3.zero;
        physicalMine.GetComponent<Animator>().speed = 1;
    }

    void Explode() {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in enemies) {
            if (col.tag == "player" || col.tag == "enemyVessel") {
                Rigidbody2D colrb = col.GetComponent<Rigidbody2D>();
                col.SendMessage("HitDamage", damage);

                Vector3 deltaPos = col.transform.position - transform.position;
                Vector3 force = deltaPos.normalized * explosionForce;
                colrb.AddForce(force);
            } else if (col.tag == "projectile" || col.tag == "enemyProjectile") {
                col.SendMessage("Explode");
            }
        }
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void HitDamage(float damage) {
        if ((health -= damage) < 0) {
            Explode();
        }
    }

    void ResetPosition() {
        if (transform.position != startPos) {
            transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
        }

    }

    void MoveTowardsTarget() {
        foreach (Transform t in targets) {
            Vector3 diff = t.position - transform.position;
            Vector3 direction = diff.normalized;
            float speed = diff.magnitude;

            rb.AddForce(direction * ( speedConstant / speed));
            physicalMine.GetComponent<Animator>().speed = 1 * (30 / speed);
        }
    }

}
