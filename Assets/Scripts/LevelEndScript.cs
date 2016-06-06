using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndScript : MonoBehaviour {

	public int level;
	private bool levelComplete;

	public int objectiveType;

	public GameObject target;
	public int targetAmount;
	private int currentAmount;

	public GameObject targetLanding;
	public GameObject player;
	private float distance;
	public float targetDistance = 100f;

	// Use this for initialization
	void Start () {
		levelComplete = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (objectiveType == 0) {
			if (currentAmount >= targetAmount) {
				levelComplete = true;
                GameObject.Find("LevelComplete").GetComponent<Text>().text = "LEVEL COMPLETE!";
                Invoke("levelCompleted", 3);
            }
		} else if (objectiveType == 1) {
			distance = Vector2.Distance (targetLanding.transform.position, player.transform.position);
			if (distance < targetDistance) {
				levelComplete = true;
                GameObject.Find("LevelComplete").GetComponent<Text>().text = "LEVEL COMPLETE!";
                Invoke("levelCompleted", 3);
			}
		}



	}

    void levelCompleted()
    {
        if (levelComplete == true)
        {
            if (level == 1)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().LoadLevel("area2");
            }
            else if (level == 2)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().LoadLevel("area3");
            }
            else if (level == 3)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().LoadLevel("area4");
            }
            else if (level == 4)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().LoadLevel("bossPrototype");
            }
            else {
                GameObject.Find("Canvas").GetComponent<UIManager>().LoadLevel("MainMenu");
            }
        }
    }

	public void IncrementTargetsDestroyed () {
		currentAmount++;
	}

	public void DysonDestroyed() {
		levelComplete = true;
	}

	public string getObjective() {
        if (objectiveType == 0) {
            return "Remaining Hostiles: " + currentAmount + "/" + targetAmount;
        } else if (objectiveType == 1) {
            return "Find the Orbital Platform";
		} else {
			return "Destroy the Dyson Sphere";
		}
	}
}
