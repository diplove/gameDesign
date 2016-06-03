using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHullText : MonoBehaviour {

	public Text hullLabel; // A serialized field where you put in the object in the inspector. In this case, it is a UIText object.
    public Text speedLabel;
    public Text shieldLabel;
    public Text heatLabel;
    public Text batteryLabel;

    public Scrollbar HullBar;
    public Scrollbar ShieldBar;
    public Scrollbar HeatBar;
    public Scrollbar BatteryBar;

    private GameObject player; // Variable storing the player GameObject


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("player"); // Finding the player in the hierachy, with the assigned player tag
	}
	
	// Update is called once per frame
	void Update () {
        batteryLabel.text = player.GetComponent<PrototypePlayer>().curBatt + " / " + player.GetComponent<PrototypePlayer>().maxBatt;
		hullLabel.text = player.GetComponent<PrototypePlayer>().curHull + " / " + player.GetComponent<PrototypePlayer>().maxHull; // Retrieving values from the script component of the player
        speedLabel.text = "Speed: " + Mathf.Round(player.GetComponent<Rigidbody2D>().velocity.magnitude) + " km/s" + " || X: " + player.GetComponent<Rigidbody2D>().velocity.x
                            + " | Y: " + player.GetComponent<Rigidbody2D>().velocity.y; // Need to round to two decimal places

        HullBar.size = (float)player.GetComponent<PrototypePlayer>().curHull / player.GetComponent<PrototypePlayer>().maxHull;
        HeatBar.size = (float)player.GetComponent<PrototypePlayer>().curHeat / player.GetComponent<PrototypePlayer>().thresholdHeat;
        BatteryBar.size = (float)player.GetComponent<PrototypePlayer>().curBatt / player.GetComponent<PrototypePlayer>().maxBatt;
    }


    public void Damage(int value) {
        player.GetComponent<PrototypePlayer>().curHull -= value;
    }

    public void Discharge(int value) {
        player.GetComponent<PrototypePlayer>().curBatt -= value;
    }
}
