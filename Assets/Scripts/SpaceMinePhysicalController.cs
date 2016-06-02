using UnityEngine;
using System.Collections;

public class SpaceMinePhysicalController : MonoBehaviour {

 void HitDamage(float damage)
    {
        transform.root.SendMessage("HitDamage", damage);
    }
}
