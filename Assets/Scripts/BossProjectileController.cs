using UnityEngine;
using System.Collections;

public class BossProjectileController : MonoBehaviour {

    public Sprite projectileSprite;
    public Sprite explosionSprite;

    private float damage;

    private int countDown;
    private bool isCounting;

    void Update() {
        if (isCounting == true) {
            countDown -= 1;
        }

        if (countDown <= 0) {
            DeactivateSelf();
        }
    }

    void Start() {
        UpdateDamage();
        countDown = 5;
        isCounting = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch(other.gameObject.tag) {
            case "player":
                other.transform.SendMessage("HitDamage", damage);
                Explode();
                break;
            case "playerChild":
                other.transform.SendMessage("HitDamage", damage);
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
            case "ignoreTrigger":
                break;
            default:
                Explode();
                break;
        } 


    }

    void UpdateDamage() {
        damage = FindObjectOfType<Boss_Sphere>().projectileDamage;
    }

    void Explode() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().sprite = explosionSprite;
        isCounting = true;
    }

    public void DeactivateSelf() {
        isCounting = false;
        countDown = 5;
        gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = projectileSprite;
    }

    public void setDamage(float damageAmount) {
        damage = damageAmount;
    }



}


