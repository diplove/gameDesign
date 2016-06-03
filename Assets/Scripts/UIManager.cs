using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour {

    GameObject Vessel;
    PrototypePlayer PlayerCode;

    Scrollbar HullBar;
    Text HullLabel;
    CanvasGroup Shield;
    Image Battery;
    Text BatteryLabel;
    CanvasGroup Heat;
    CanvasGroup HeatAlarm;
    Text SpeedLabel;

    GameObject[] pauseObjects;

    // Use this for initialization
    void Start () {
        Vessel = GameObject.FindGameObjectWithTag("player");
        PlayerCode = (PrototypePlayer)Vessel.GetComponent(typeof(PrototypePlayer));

        HullBar = GameObject.Find("HullBar").GetComponent<Scrollbar>();
        HullLabel = GameObject.Find("HullLabel").GetComponent<Text>();

        Shield = GameObject.Find("StaticShield").GetComponent<CanvasGroup>();

        Battery = GameObject.Find("BatteryBars").GetComponent<Image>();
        BatteryLabel = GameObject.Find("BatteryLabel").GetComponent<Text>();

        Heat = GameObject.Find("HeatGlow").GetComponent<CanvasGroup>();
        HeatAlarm = GameObject.Find("HeatAlarm").GetComponent<CanvasGroup>();
        HeatAlarm.alpha = 0;

        SpeedLabel = GameObject.Find("SpeedLabel").GetComponent<Text>();

        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        Time.timeScale = 1;
        hidePaused();
    }
	
	// Update is called once per frame
	void Update () {
        ReadInput();
        UpdateUI();
        HeatCheck();
    }

    private void ReadInput() {
        //uses the P or ESC or PAUSE button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Pause)) {
            pauseControl();
        } else if (Input.GetKeyDown(KeyCode.N)) {
            SkipLevel();
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
    private void showPaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    private void hidePaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(false);
        }
    }

    public void Respawn() {
        pauseControl();
        PlayerCode.DestroySelf();
    }

    //loads inputted level
    public void LoadLevel(string level) {
        SceneManager.LoadScene(level);
    }

    //loads inputted level
    public void LoadLevel(int level) {
        SceneManager.LoadScene(level);
    }

    private void SkipLevel() {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateUI() {
        HullBar.size = Mathf.Round((float)Vessel.GetComponent<PrototypePlayer>().curHull / Vessel.GetComponent<PrototypePlayer>().maxHull * 100) /100f;
        HullLabel.text = Vessel.GetComponent<PrototypePlayer>().curHull + " / " + Vessel.GetComponent<PrototypePlayer>().maxHull;

        Shield.alpha = (float)Vessel.GetComponent<PrototypePlayer>().curShield / Vessel.GetComponent<PrototypePlayer>().maxShield;

        Battery.fillAmount = Mathf.Round((float)Vessel.GetComponent<PrototypePlayer>().curBatt / Vessel.GetComponent<PrototypePlayer>().maxBatt * 10) / 10f;
        BatteryLabel.text = Vessel.GetComponent<PrototypePlayer>().curBatt + " / " + Vessel.GetComponent<PrototypePlayer>().maxBatt;

        Heat.alpha = Vessel.GetComponent<PrototypePlayer>().curHeat / Vessel.GetComponent<PrototypePlayer>().thresholdHeat;

        SpeedLabel.text = "Speed: " + Mathf.Round(Vessel.GetComponent<Rigidbody2D>().velocity.magnitude) + " km/s" + " || X: " + Vessel.GetComponent<Rigidbody2D>().velocity.x + " | Y: " + Vessel.GetComponent<Rigidbody2D>().velocity.y; // Need to round to two decimal places
    }

    private void HeatCheck() {
        if ((Vessel.GetComponent<PrototypePlayer>().curHeat / Vessel.GetComponent<PrototypePlayer>().thresholdHeat) >= 0.5f) {
            HeatAlarm.alpha = Mathf.PingPong(Time.time, 1);
        } else {
            HeatAlarm.alpha = 0;
        }
    }
}
