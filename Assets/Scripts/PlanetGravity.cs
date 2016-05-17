using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {

    public float mass;
    public float gravConstant;

    [SerializeField]
    private Collider2D gravDeadZone;
    [SerializeField]
    private Collider2D gravOrbit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {

    }

    void OnTriggerStay2D(Collider2D other)
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
    }
}
