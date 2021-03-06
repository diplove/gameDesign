using UnityEngine;
using System.Collections;

public class NormalLaserController : MonoBehaviour {

	public Sprite laserSprite;

	private float damage;

	void FixedUpdate() {

	}

	void Start() {
		UpdateDamage();
	}

	void Update() {
		
	}

	/*void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "asteroid") {
			other.transform.SendMessage("HitDamage", damage);
		}

	}*/

    void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "asteroid") {
			other.transform.SendMessage("HitDamage", damage);
		}

	}

	void UpdateDamage() {
		damage = FindObjectOfType<PrototypePlayer>().primDamage;
	}

	public void setDamage(float damageAmount) {
		damage = damageAmount;
	}

    public float getDamage() {
        return damage;
    }



}
 