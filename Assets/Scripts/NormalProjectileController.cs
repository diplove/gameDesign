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

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "player") {
            Explode();
        }
        if (other.gameObject.tag == "asteroid") {
            Explode();
            other.transform.SendMessage("HitDamage", damage);
        }
        
    }

    void UpdateDamage() {
        damage = FindObjectOfType<PrototypePlayer>().primDamage;
    }

    void Explode() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
 
	void Update () {
		if (isCounting == true) {
			countDown -= 1;
		}

		if (countDown <= 0) {
			DeactivateSelf ();
		}
	}

    
}
