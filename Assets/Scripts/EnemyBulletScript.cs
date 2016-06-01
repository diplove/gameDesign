using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour
{
    public float BulletForce;
    //public float TargetLife;

    //private float life;
    // Use this for initialization
    void Start()
    {  
        //life = TargetLife;
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(transform.up * BulletForce);
    }

    // Update is called once per frame
    void Update()
    {
        //life -= Time.deltaTime;
        //if (life < 0) Destroy(gameObject);
    }
}
