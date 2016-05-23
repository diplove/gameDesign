using UnityEngine;
using System.Collections;

public class PrototypePlayer : MonoBehaviour {
    //Mobility
	public float maxVelocity;
    public float accelerationRate;
    public float decelerationRate;
    public float turnAmount;
	public float turnRate;

	//Hull
	public int maxHull;
	public int curHull;
	public int hullType;

	//Shield
	public int maxShield;
	public int curShield;
	public int shieldType;

	//Generator
	public int maxBatt;
	public int curBatt;
	public float rechargeRate;
	public float rechargeBuffer;
	public int bufferAmount;

	//Heat
	public float curHeat;
	public float thresholdHeat;
	public int heatDamageTick;
	public int heatTickThreshold;
	public float heatDecreaseRate;
	public int heatDecreaseMultiplier;

	//Primary Weapon
	public int primDamage;
	public int primType;
	public int primRate;
	public int primCost;
	public float primHeat;

	//Auxiliary Weapon
	public int auxDamage;
	public int auxType;
	public int auxRate;
	public int auxCost;
	public float auxHeat;

	//Support System
	public int suppSystem;
	public bool suppActive;
	public int suppCost;
	public float suppBattRechargeBlock;


    private Vector3 rotation;

    private Rigidbody2D rb;

    private bool landed;

	// Use this for initialization
	void Start () {
	rb = GetComponent<Rigidbody2D>();
        rotation = transform.rotation.eulerAngles;

        maxHull = 500; // Temporary max hull value
        curHull = maxHull; // Current is set to max at start
	}

    // Update is called once per frame
    void Update()
    {
		//Controls
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
        if (!landed) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), turnRate * Time.deltaTime);
        }

		if (Input.GetKey("z")) {
			if (curBatt >= primCost) {
				curBatt -= primCost;
				curHeat += primHeat;
				FirePrimary();
			}
		}

		if (Input.GetKey("x")) {
			if (curBatt >= auxCost) {
				curBatt -= auxCost;
				curHeat += auxHeat;
				FireAuxiliary();
			}
		}

		if (Input.GetKeyDown("c")) {
			ToggleSupport ();
		}

		//Generator Upkeep
		if (curBatt < maxBatt) {
			if (suppActive == true) {
				rechargeBuffer = rechargeBuffer + (rechargeRate - suppBattRechargeBlock);
			} 
			else {
				rechargeBuffer += rechargeRate;
			}

			if (rechargeBuffer >= bufferAmount) {
				rechargeBuffer -= bufferAmount;
				curBatt += bufferAmount;
			}
		}

		if (suppActive == false) {
			if (curBatt > maxBatt) {
				curBatt = maxBatt;
			}
		} else {
			if (curBatt > (maxBatt - suppCost)) {
				curBatt = maxBatt - suppCost;
			}
		}

		//Heat Upkeep
		if (curHeat > thresholdHeat) {
			heatDamageTick += 1;
			if (heatDamageTick > heatTickThreshold) {
				curHull -= 1;
				heatDamageTick -= heatTickThreshold;
			}
		}

		if (curHeat > 0) {
			curHeat -= (heatDecreaseRate*heatDecreaseMultiplier);
		}
    }

    // Method called for dealing damage to vessel
    public void HitDamage(int damage) {
       // Debug.Log("Hit For: " + damage);
        if (curShield > 0) {
            if (curShield - damage <= 0) {
                curShield = 0;
                curHull -= damage;
            } else {
                curShield -= damage;
            }
        } else {
            curHull -= damage;
        }
    }

	
	void Accelerate ()
    {
        if (landed) {
            breakFixedLock();
        }
        rb.AddForce(transform.up * accelerationRate);
        enforceSpeedLimit();
    }

    void Reverse()
    {        
            rb.AddForce(transform.up * -decelerationRate);
            enforceSpeedLimit();        
    }

	void FirePrimary() {
		curBatt = 0; //Placeholder for the creation of a Primary Weapon Projectile
	}

	void FireAuxiliary() {
		curHeat += 10; //Placeholder for the creation of an Auxiliary Weapon Projectile
	}

	void ToggleSupport() {
        suppActive = !suppActive;
	}

    void enforceSpeedLimit()
    {
        rb.velocity = rb.velocity.y > maxVelocity ? new Vector2(rb.velocity.x, maxVelocity) : rb.velocity;
        rb.velocity = rb.velocity.x > maxVelocity ? new Vector2(maxVelocity, rb.velocity.y) : rb.velocity;
        rb.velocity = rb.velocity.y < -maxVelocity ? new Vector2(rb.velocity.x, -maxVelocity) : rb.velocity;
        rb.velocity = rb.velocity.y < -maxVelocity ? new Vector2(rb.velocity.x, -maxVelocity) : rb.velocity;
    }

    void createFixedLock(GameObject other) {
        gameObject.AddComponent<FixedJoint2D>();
        transform.parent = other.transform;
        GetComponent<FixedJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
        landed = true;
    }

    void breakFixedLock() {
        rotation = transform.rotation.eulerAngles;
        landed = false;
        transform.parent = null;
        Destroy(GetComponent<FixedJoint2D>());
    }

}
