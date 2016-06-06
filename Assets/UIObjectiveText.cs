using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIObjectiveText : MonoBehaviour {

    private GameObject levelScriptObject;

	// Use this for initialization
	void Start () {
        levelScriptObject = GameObject.Find("ObjectiveSystem");
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = "Objective: " + levelScriptObject.GetComponent<LevelEndScript>().getObjective();
	}
}
