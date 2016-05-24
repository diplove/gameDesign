using UnityEngine;
using System.Collections;

public class NormalProjectileController : MonoBehaviour {

    public Sprite projectileSprite;
    public Sprite explosionSprite;

    void FixedUpdate() {

    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "player") {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Animator>().Play("normalProjectileExplode");
        }
        
    }

    public void deactiveSelf() {
        gameObject.SetActive(false);
    }

    
}
