using UnityEngine;
using System.Collections;

public class EnemyShipScript : MonoBehaviour
{
    public float MinForce = 20f;
    public float MaxForce = 40f;
    public float DirectionChangeInterval = 1f;
    public float ShotInterval = 1f;
    public float ShotDamage = 10f;

    private GameObject vessel;
    private float directionChangeInterval;
    private float shotInterval;

    public float health;
    private float defaultHealth = 200;

    public int moveSpeed; //Movement speed of EnemyShip
    public int rotationSpeed; //Rotation Speed of EnemyShip
    private Transform myTransform; //transformation of EnemyShip
    private Transform target; //transfomration of Player 
  
    private float maxSqrDistance = 300; //Max Distance between the player and EnemyShip, i.e 75 meters
    private bool follow;

    //Enemy Dead effects
    public GameObject DeathExplosion;

    void Awake()
    {
        myTransform = transform;
    }

    // Use this for initialization
    void Start ()
    {
        if (health == 0)
        {
            health = defaultHealth;
        }

        directionChangeInterval = DirectionChangeInterval;
        Push();
        shotInterval = ShotInterval;
        

        GameObject player = GameObject.FindGameObjectWithTag("player");
        target = player.transform;
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


        Vector3 dir = target.position - myTransform.position;
        float distance = dir.sqrMagnitude;
        dir.z = 0.0f;

        //If Player is detected withing range then set follow as true and shoot at the player
        if (distance < maxSqrDistance)
        {
            follow = true;
            //Shoot at the Player Ship at 1 sec interval
            shotInterval -= Time.deltaTime;
            if (shotInterval < 0)
            {
                Shoot();
                shotInterval = ShotInterval;
                Debug.Log("Shooting at the Player");
            }
        }

        //If follow is true they never leave the player
        if(follow)
        {
            transform.LookAt(target);
            if (dir != Vector3.zero)
            {
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.FromToRotation(Vector3.up, dir), rotationSpeed * Time.deltaTime);

                //Move Towards Player Ship
                myTransform.position += (target.position - myTransform.position).normalized * moveSpeed * Time.deltaTime;
            }
        }

        if (health < 200)
        {
            Debug.Log(health);
            Debug.Log("I am Dying");
        }
        
    }

    void Push()
    {
        float force = Random.Range(MinForce, MaxForce);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector3(0f, 0f);
        rb2d.AddForce(force * new Vector3(x, y));
    }

    void Shoot()
    {

        GetComponent<ProjectileController>().shootNormalProjectile();

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
            Instantiate(DeathExplosion, transform.position, transform.rotation);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
    }
}
