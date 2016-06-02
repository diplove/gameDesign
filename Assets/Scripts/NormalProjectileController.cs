using UnityEngine;
using System.Collections;

public class NormalProjectileController : MonoBehaviour {

    public Sprite projectileSprite;
    public Sprite explosionSprite;

	private int countDown;
	private bool isCounting;

    private float damage;

    void FixedUpdate() {

    }

    void Start() {
        UpdateDamage();
		countDown = 5;
		isCounting = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject);
        switch (other.gameObject.tag) {
            case "asteroid":
                Explode();
                other.transform.SendMessage("HitDamage", damage);
                break;
            case "bossChild":
                other.gameObject.SendMessage("HitDamage", damage);
                Explode();
                break;
            case "boss":
                other.gameObject.SendMessage("HitDamage", gameObject);
                break;
            case "player":
                other.gameObject.SendMessage("HitDamage", (int)damage);
                Explode();
                break;
            case "playerChild":
                if (gameObject.tag == "enemyProjectile") {
                    other.gameObject.SendMessage("HitDamage", (int)damage);
                    Explode();
                }
                break;
            case "enemyShip":
                other.gameObject.SendMessage("HitDamage", damage);
                Explode();
                break;
            case "enemyProjectile":
                Explode();
                break;
            case "ignoreTrigger":
                break;
            default:
                //Debug.Log("Projectile " + gameObject + " encountered object with no tag handler");
                Explode();
                break;
        }
    }

    void UpdateDamage() {
        damage = FindObjectOfType<PrototypePlayer>().primDamage;
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

    public float getDamage() {
        return damage;
    }
 
	void Update () {
		if (isCounting == true) {
			countDown -= 1;
		}

		if (countDown <= 0) {
			DeactivateSelf ();
		}
	}

    
}
