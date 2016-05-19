using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHullText : MonoBehaviour {
	[SerializeField]
	private Text hullLabel; // A serialized field where you put in the object in the inspector. In this case, it is a UIText object.

    private GameObject player; // Variable storing the player GameObject


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("player"); // Finding the player in the hierachy, with the assigned player tag
	}
	
	// Update is called once per frame
	void Update () {
		hullLabel.text = "Hull: " + player.GetComponent<PrototypePlayer>().curHull + "/" + player.GetComponent<PrototypePlayer>().maxHull; // Retrieving values from the script component of the player
	}
}
