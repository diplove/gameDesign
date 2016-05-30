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

        switch(other.gameObject.tag) {
            case "player":
                other.transform.SendMessage("HitDamage", damage);
                Explode();
                break;
            case "asteroid":
                other.transform.SendMessage("HitDamage", damage);
                Explode();
                break;
            case "projectile":
                Explode();
                break;
            case "enemyProjectile":
                //Explode();
                break;
            case "enemyShip":
                other.transform.SendMessage("HitDamage", damage);
                Explode();
                break;
        }


    }

    void UpdateDamage() {
        damage = FindObjectOfType<Boss_Sphere>().projectileDamage;
    }

    void Explode() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        DeactivateSelf(); // Temporary

    }

    public void DeactivateSelf() {
        gameObject.SetActive(false);

    }

    public void setDamage(float damageAmount) {
        damage = damageAmount;
    }



}


