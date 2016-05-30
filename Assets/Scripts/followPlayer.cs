using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (target.position.x, target.position.y, -15);
	}

    public void SetTarget(Transform target) {
        this.target = target;
    }
}
