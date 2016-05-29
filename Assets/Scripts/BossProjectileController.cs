using UnityEngine;
using System.Collections;

public class BossProjectileController : MonoBehaviour {

    public Sprite projectileSprite;
    //public Sprite explosionSprite;

    private float damage;

    void FixedUpdate() {

    }

    void Start() {
        UpdateDamage();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "asteroid") {
            Explode();
            other.transform.SendMessage("HitDamage", damage);
        } else if (other.gameObject.tag == "projectile") {
            Explode();
        }


    }

    void UpdateDamage() {
        damage = FindObjectOfType<Boss_Sphere>().projectileDamage;
    }

    void Explode() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        DeactivateSelf(); // Temporary
        //GetComponent<Animator>().Play("normalProjectileExplode");
    }

    public void DeactivateSelf() {
        gameObject.SetActive(false);

    }

    public void setDamage(float damageAmount) {
        damage = damageAmount;
    }



}


