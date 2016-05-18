using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackholeGravity : MonoBehaviour {

    public float mass;
    public float gravStrength;

    [SerializeField]
    private Collider2D gravityWell;
    private List<Transform> blackholeObjects = new List<Transform>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        pullObjects();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "moon" || other.tag != "planet")
        {
            if (gravityWell.IsTouching(other))
            {
                blackholeObjects.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "moon" || other.tag != "planet")
        {
            if (!gravityWell.IsTouching(other))
            {
                blackholeObjects.Remove(other.transform);
            }
        }
    }

    void pullObjects()
    {
        foreach (Transform t in blackholeObjects)
        {
            Vector3 diff = transform.position - t.position;
            Vector3 direction = diff.normalized;
            Rigidbody2D tRb = t.GetComponent<Rigidbody2D>();

            float gravForce = (mass * tRb.mass * gravStrength) / diff.sqrMagnitude;

            tRb.AddForce(direction * gravForce);
        }
    }

}
