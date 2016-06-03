using UnityEngine;
using System.Collections;

public class UIShield : MonoBehaviour {

    CanvasGroup Shield;

    // Use this for initialization
    void Start () {
        Shield = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Fade() {
        while (Shield.alpha > 0) {                 //use "< 1" when fading in
            Shield.alpha -= Time.deltaTime / 1;    //fades out over 1 second. change to += to fade in    
            yield return null;
        }
    }
}
