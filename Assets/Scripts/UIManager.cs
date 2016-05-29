using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;
    GameObject Vessel;
    PrototypePlayer Player;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        GameObject Vessel = GameObject.Find("Vessel");
        Player = (PrototypePlayer)Vessel.GetComponent(typeof(PrototypePlayer));
        hidePaused();
    }
	
	// Update is called once per frame
	void Update () {

        //uses the P or ESC button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            pauseControl();
        }

    }

    //Reloads the Level
    public void Reload() {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    //controls the pausing of the scene
    public void pauseControl() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            showPaused();
        } else if (Time.timeScale == 0) {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(false);
        }
    }

    public void Respawn() {
        pauseControl();
        Player.DestroySelf();
    }

    //loads inputted level
    public void LoadLevel(string level) {
        SceneManager.LoadScene(level);
    }
}
