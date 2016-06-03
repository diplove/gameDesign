using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class Boss_Sphere_MainController : MonoBehaviour {

    private GameObject player;

    public float fullHealth = 10000f;
    public float phaseOneHealth;
    public float phaseTwoHealth;
    public GameObject PhaseOneSphere; // Prefab
    public GameObject PhaseTwoSphere; // Prefab

    private GameObject SphereOne;
    private GameObject SphereTwo;

    private bool battleStarted = false;

    // Phase ONE
    private int phase = 0;
    private bool phaseOneLoading = false;
    private bool phaseOneLoaded = false;
    //private bool phaseOneCompleted = false;

    // Phase TWO
    private bool phaseTwoLoading = false;
    private bool phaseTwoLoaded = false;
    //private bool phaseTwoCompleted = false;

    private bool battleFinished = false;

    // Special
    private GameObject mainCamera;
    private GameObject uiCanvas;

    // Use this for initialization
    void Awake() {
        SphereOne = (GameObject)Instantiate(PhaseOneSphere, transform.position, transform.rotation);
        SphereTwo = (GameObject)Instantiate(PhaseTwoSphere, transform.position, transform.rotation);
    }

    void Start () {
        player = GameObject.FindGameObjectWithTag("player");
        phaseOneHealth = fullHealth * 0.8f;
        phaseTwoHealth = fullHealth * 0.2f;
        mainCamera = GameObject.Find("Main Camera");
        uiCanvas = GameObject.Find("Canvas");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (battleStarted == false) {
            if (other.tag == "player") {
                SetCameraToBoss();
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                battleStarted = true;
                phase = 1;
            }
        }
    }

    void SetCameraToPlayer() {
        mainCamera.GetComponent<followPlayer>().SetTarget(player.transform);
        uiCanvas.SetActive(true);       
    }

    void SetCameraToBoss() {
        mainCamera.GetComponent<followPlayer>().SetTarget(transform);
        uiCanvas.SetActive(false);
    }

    public void PhaseOneLoadComplete() {
        phaseOneLoaded = true;
        phaseOneLoading = false;
        SetCameraToPlayer();
    }

    public void PhaseTwoLoadComplete() {
        phaseTwoLoaded = true;
        phaseTwoLoading = false;
    }

    public void PhaseOneFinished() {
        phaseTwoLoading = true;
        phase = 2;
        Destroy(SphereOne);
        SphereTwo.SetActive(true);
    }

    public bool IsChangingToPhaseTwo()
    {
        if (fullHealth <= phaseTwoHealth)
        {
            return true;
        }
        return false;
    }

    public void HitDamage(float damage) {
        if (phase == 1) {
            if ((fullHealth -= damage) <= phaseTwoHealth) {
                StartCoroutine(PhaseChangeToTwo());
            }
        } else if (phase == 2) {
            if ((fullHealth -= damage) <= 0) {
                StartCoroutine(SphereDeath());
            }
        }
    }

    public void SphereDead() {
        battleFinished = true;
    }

    IEnumerator PhaseChangeToTwo() {
        SetCameraToBoss();
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        SphereOne.GetComponent<Boss_Sphere>().ChangeToPhaseTwo();
        while (!phaseTwoLoaded) {
            yield return new WaitForSeconds(.5f);
        }
        SetCameraToPlayer();
    }

    IEnumerator SphereDeath() {
        SetCameraToBoss();
        yield return new WaitForSeconds(2);
        SphereTwo.GetComponent<Boss_Sphere_PhaseTwo>().StopAllCoroutines();
        SphereTwo.GetComponent<Animator>().Stop();
        StartCoroutine(SphereTwo.GetComponent<Boss_Sphere_PhaseTwo>().DestroySelf());
        while (!battleFinished) {
            yield return new WaitForSeconds(.5f);
        }
        SetCameraToPlayer();
        yield return new WaitForSeconds(3);
        EditorSceneManager.LoadScene("MainMenu");

    }

    // Update is called once per frame
    void Update() {
        if (!battleFinished) {
            // Start Phase One in the Boss object
            if (phase == 1 && !phaseOneLoaded && !phaseOneLoading) {
                StartCoroutine(SphereOne.GetComponent<Boss_Sphere>().StartBattle());
            }

            if (phase == 2 && !phaseTwoLoaded && !phaseTwoLoading) {
                SphereTwo.GetComponent<Boss_Sphere_PhaseTwo>().TriggerStartSequence();
            }
        }               
    
	}


}
