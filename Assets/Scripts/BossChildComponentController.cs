using UnityEngine;
using System.Collections;

public class BossChildComponentController : MonoBehaviour {

    public GameObject parentObj;

	void HitDamage(float damage) {
            parentObj.SendMessage("HitDamage", damage); 
    }
}
