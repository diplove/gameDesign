using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHullText : MonoBehaviour {

	public Text hullLabel; // A serialized field where you put in the object in the inspector. In this case, it is a UIText object.
    public Text speedLabel;
    public Text shieldLabel;
    public Text heatLabel;
    public Text batteryLabel;

    private GameObject player; // Variable storing the player GameObject


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("player"); // Finding the player in the hierachy, with the assigned player tag
	}
	
	// Update is called once per frame
	void Update () {
        batteryLabel.text = "Battery: " + player.GetComponent<PrototypePlayer>().curBatt + "/" + player.GetComponent<PrototypePlayer>().maxBatt;
        heatLabel.text = "Heat: " + player.GetComponent<PrototypePlayer>().curHeat + "/" + player.GetComponent<PrototypePlayer>().thresholdHeat;
		hullLabel.text = "Hull: " + player.GetComponent<PrototypePlayer>().curHull + "/" + player.GetComponent<PrototypePlayer>().maxHull; // Retrieving values from the script component of the player
        shieldLabel.text = "Shield: " + player.GetComponent<PrototypePlayer>().curShield + "/" + player.GetComponent<PrototypePlayer>().maxShield;
        speedLabel.text = "Speed: " + Mathf.Round(player.GetComponent<Rigidbody2D>().velocity.magnitude) + " km/s" + " || X: " + player.GetComponent<Rigidbody2D>().velocity.x
                            + " | Y: " + player.GetComponent<Rigidbody2D>().velocity.y; // Need to round to two decimal places
	}
}
