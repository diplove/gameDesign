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
		countDown = 2;
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
        //GetComponent<Animator>().Play("normalProjectileExplode");
		GetComponent<SpriteRenderer>().sprite = explosionSprite;
		isCounting = true;
    }

    public void DeactivateSelf() {
        gameObject.SetActive(false);
        
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
