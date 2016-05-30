using UnityEngine;
using System.Collections;

public class ExplosionDestroySelf : MonoBehaviour {

	void End() {
        Destroy(gameObject);
    }
}
