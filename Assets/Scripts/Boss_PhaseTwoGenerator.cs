using UnityEngine;
using System.Collections;

public class Boss_PhaseTwoGenerator : MonoBehaviour {

    public GameObject generatorExplosion;

    void HitDamage(float damage) {
        transform.root.GetComponent<Boss_Sphere_PhaseTwo>().GeneratorHit(gameObject, damage);
    }

    void DestroySelf() {
        Instantiate(generatorExplosion, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
