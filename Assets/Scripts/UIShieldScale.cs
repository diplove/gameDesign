using UnityEngine;
using System.Collections;

public class UIShieldScale : MonoBehaviour {

    private GameObject player;

	void Start() {
        player = GameObject.FindGameObjectWithTag("player");

        Vector2 shieldSize = player.GetComponent<PrototypePlayer>().shieldSize;
        transform.localScale = new Vector2(shieldSize.x * 1.5f, shieldSize.y * 1.5f);
        //transform.position = player.GetComponent<ShieldController>().transform.position;
    }

    void Update() {
        transform.rotation = player.transform.rotation;
    }
}
