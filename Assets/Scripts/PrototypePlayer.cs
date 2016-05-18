using UnityEngine;
using System.Collections;

public class PrototypePlayer : MonoBehaviour {
    public float maxVelocity;
    public float accelerationRate;
    public float decelerationRate;
    public float turnAmount;
	public float turnRate;

    private Vector3 rotation;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
	rb = GetComponent<Rigidbody2D>();
        rotation = transform.rotation.eulerAngles;
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Accelerate();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Reverse();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation.z += turnAmount;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation.z -= turnAmount;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), turnRate * Time.deltaTime);
    }

	
	void Accelerate ()
    {
        rb.AddForce(transform.up * accelerationRate);
        enforceSpeedLimit();
    }

    void Reverse()
    {
        rb.AddForce(transform.up * -decelerationRate);
        enforceSpeedLimit();
    }

    void enforceSpeedLimit()
    {
        rb.velocity = rb.velocity.y > maxVelocity ? new Vector2(rb.velocity.x, maxVelocity) : rb.velocity;
        rb.velocity = rb.velocity.x > maxVelocity ? new Vector2(maxVelocity, rb.velocity.y) : rb.velocity;
        rb.velocity = rb.velocity.y < -maxVelocity ? new Vector2(rb.velocity.x, -maxVelocity) : rb.velocity;
        rb.velocity = rb.velocity.y < -maxVelocity ? new Vector2(rb.velocity.x, -maxVelocity) : rb.velocity;
    }
	
}
