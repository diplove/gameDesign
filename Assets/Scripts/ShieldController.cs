using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {

    public CircleCollider2D shieldTrigger;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void deactivateShield() {
        GetComponent<Animator>().enabled = false;
        shieldTrigger.enabled = false;
    }

    public void activateShield() {
        GetComponent<Animator>().enabled = true;
        shieldTrigger.enabled = true;

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "projectileNormal") {
            transform.parent.SendMessage("HitDamage", 20); // temporary hit damage
            GetComponent<Animator>().Play("hit");
            other.gameObject.SetActive(false);
        }
    }
}
