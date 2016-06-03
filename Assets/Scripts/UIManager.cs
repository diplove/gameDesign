using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;
    GameObject Vessel;
    PrototypePlayer Player;
    public CanvasGroup Shield;
    public CanvasGroup Heat;
    public GameObject player; // Variable storing the player GameObject

    public Image Battery;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        GameObject Vessel = GameObject.Find("Vessel");
        Player = (PrototypePlayer)Vessel.GetComponent(typeof(PrototypePlayer));
        hidePaused();
        //Shield = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
        ReadInput();
        UpdateUI();
    }

    void ReadInput() {
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

    void UpdateUI() {
        Shield.alpha = (float)player.GetComponent<PrototypePlayer>().curShield / player.GetComponent<PrototypePlayer>().maxShield;
        Heat.alpha = (float)player.GetComponent<PrototypePlayer>().curHeat / player.GetComponent<PrototypePlayer>().thresholdHeat;
        Battery.fillAmount = Mathf.Round((float)player.GetComponent<PrototypePlayer>().curBatt / player.GetComponent<PrototypePlayer>().maxBatt *10)/10f;
    }
}
