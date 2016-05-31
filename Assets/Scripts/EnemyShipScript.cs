using UnityEngine;
using System.Collections;

public class EnemyShipScript : MonoBehaviour
{
    public float MinForce = 20f;
    public float MaxForce = 40f;
    public float DirectionChangeInterval = 1f;
    public float ShotInterval = 1f;
    public GameObject EnemyShipBullet;

    private GameObject vessel;
    private float directionChangeInterval;
    private float shotInterval;

    public float health;
    private float defaultHealth = 100;

    // Use this for initialization
    void Start ()
    {
        if (health == 0)
        {
            health = defaultHealth;
        }

        directionChangeInterval = DirectionChangeInterval;
        shotInterval = ShotInterval;
        Push();
	}
	
	// Update is called once per frame
	void Update ()
    {
        directionChangeInterval -= Time.deltaTime;
        if (directionChangeInterval < 0)
        {
            Push();
            directionChangeInterval = DirectionChangeInterval;
        }

        shotInterval -= Time.deltaTime;
        if (shotInterval < 0)
        {
            Shoot();
            shotInterval = ShotInterval;
        }
	}

    void Push()
    {
        float force = Random.Range(MinForce, MaxForce);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0f, 0f);
        rb2d.AddForce(force * new Vector2(x, y));
    }

    void Shoot()
    {
        vessel = UnityEngine.GameObject.FindGameObjectWithTag("player");

        float angle = (Mathf.Atan2(
            vessel.transform.position.y - transform.position.y,
            vessel.transform.position.x - transform.position.x) - Mathf.PI / 2) * Mathf.Rad2Deg;

        Instantiate(EnemyShipBullet, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "player")
        {
            Rigidbody2D rb2d = collider.gameObject.GetComponent<Rigidbody2D>();
            if (rb2d.velocity.sqrMagnitude < 1)
            {
                if (Vector3.Angle(collider.transform.up, transform.position - collider.transform.position) > 170)
                {
                    collider.gameObject.SendMessage("createFixedLock", gameObject);
                }
            }
            else if (rb2d.velocity.magnitude > 1.5)
            {
                collider.gameObject.SendMessage("HitDamage", (rb2d.velocity.sqrMagnitude * rb2d.mass) * GetComponent<Rigidbody2D>().mass);
            }
        }
    }

    void HitDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
