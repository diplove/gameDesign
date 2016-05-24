using UnityEngine;
using System.Collections;

public class NormalProjectileController : MonoBehaviour {

    public Sprite projectileSprite;
    public Sprite explosionSprite;

    private float damage;

    void FixedUpdate() {

    }

    void Start() {
        UpdateDamage();
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
        GetComponent<Animator>().Play("normalProjectileExplode");
    }

    public void DeactivateSelf() {
        gameObject.SetActive(false);
        
    }

    public void setDamage(float damageAmount) {
        damage = damageAmount;
    }
 

    
}
