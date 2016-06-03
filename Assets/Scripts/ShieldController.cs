using UnityEngine;
using System.Collections;


public class ShieldController : MonoBehaviour
{

    public Collider2D shieldTrigger;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void deactivateShield()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;
        shieldTrigger.enabled = false;
    }

    public void activateShield()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Animator>().enabled = true;
        shieldTrigger.enabled = true;

    }

    void HitDamage(float damage)
    {
        transform.parent.SendMessage("HitDamage", damage);
        if (transform.parent.GetComponent<PrototypePlayer>().getCurrentShield() > 0)
        {
            GetComponent<Animator>().Play("hit");
        }
    }
}
