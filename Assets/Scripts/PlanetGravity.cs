using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetGravity : MonoBehaviour {

    public float mass;
    public float gravConstant;

    [SerializeField]
    private Collider2D gravDeadZone;
    [SerializeField]
    private Collider2D gravOrbit;

    private List<Transform> orbitObjects = new List<Transform>();
    private List<Transform> deadZoneObjects = new List<Transform>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        calculateOrbit();
        calculateDeadZone();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "moon")
        {
            if (gravOrbit.IsTouching(other))
            {
                orbitObjects.Add(other.transform);
            }
            if (gravDeadZone.IsTouching(other))
            {
                deadZoneObjects.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "moon") { 
            if (!gravOrbit.IsTouching(other))
            {
             orbitObjects.Remove(other.transform);
             }
            if (!gravDeadZone.IsTouching(other))
            {
            deadZoneObjects.Remove(other.transform);
            }
         }
    }

    void calculateOrbit()
    {
        foreach (Transform t in orbitObjects)
        {
            t.RotateAround(transform.position, Vector3.forward, 5 * Time.deltaTime);
        }
    }

    void calculateDeadZone()
    {
        foreach (Transform t in deadZoneObjects)
        {
            Vector3 diff = transform.position - t.position;
            Vector3 direction = diff.normalized;
            Rigidbody2D tRb = t.GetComponent<Rigidbody2D>();

            float gravForce = (mass * tRb.mass * gravConstant) / diff.sqrMagnitude;

            tRb.AddForce(direction * gravForce);
        }
    }

    /*void OnTriggerStay2D(Collider2D other)
    {
        if (gravDeadZone.IsTouching(other))
        {
            Vector3 diff = transform.position - other.transform.position;
            Vector3 direction = diff.normalized;
            Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();

            float gravForce = (mass * otherRb.mass * gravConstant) / diff.sqrMagnitude;

            otherRb.AddForce(direction * gravForce);
        }
        if (gravOrbit.IsTouching(other))
        {
            other.gameObject.transform.RotateAround(transform.position, Vector3.forward, 5 * Time.deltaTime);
        }
    }*/
}
