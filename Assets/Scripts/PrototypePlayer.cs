﻿using UnityEngine;
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
    private bool shieldActive;
    private bool shieldRecharging;

    private float lastShieldRechargeStep;
    public float timeBetweenShieldRechargeSteps;

    //Generator
    public int maxBatt;
	public int curBatt;
	public float rechargeRate;
	public float rechargeBuffer;
	public int bufferAmount;

    private float lastGeneratorStep;
    public float timeBetweenGeneratorSteps;

	//Heat
	public float curHeat;
	public float thresholdHeat;
	public int heatDamageTick;
	public int heatTickThreshold;
	public float heatDecreaseRate;
	public int heatDecreaseMultiplier;

    private float lastHeatStep;
    public float timeBetweenHeatSteps; // In seconds
    
    // ######## Projectile creation and movement is handled by the projectile controller ########

	//Primary Weapon
	public int primDamage;
	public int primType;
	public int primRate;
    public float primBaseSpeed;
	public int primCost;
	public float primHeat;

    private float lastPrimStep;
    private float timeBetweenPrimSteps;
   

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

    private float lastSupportstep;
    public float timeBetweenSupportSteps;

    // Rotation vector
    private Vector3 rotation;

    // Rigidbody reference for force manupulation
    private Rigidbody2D rb;

    // Keep track of vessel to see if it has landed on a planet or moon surface
    private bool landed;

    // Vessel Children
    public GameObject shield; // Shield Object


	// Use this for initialization
	void Start () {
	rb = GetComponent<Rigidbody2D>();
        rotation = transform.rotation.eulerAngles;

        curHull = maxHull; // Current is set to max at start
        curShield = maxShield; // Current shield is initialised to the max value
        curBatt = maxBatt;
        curHeat = 0;

        UpdatePrimaryWeapon();
	}

    void FixedUpdate() {
        HeatUpkeep();
        GeneratorUpkeep();
        SupportUpkeep();
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
    }

    void GeneratorUpkeep() {
        //Generator Upkeep
        if (Time.time - lastGeneratorStep > timeBetweenGeneratorSteps) {
            if (curBatt < maxBatt) {
                if (suppActive == true) {
                    rechargeBuffer = rechargeBuffer + (rechargeRate - suppBattRechargeBlock);
                } else {
                    rechargeBuffer += rechargeRate;
                }

                if (rechargeBuffer >= bufferAmount) {
                    rechargeBuffer -= bufferAmount;
                    curBatt += bufferAmount;
                }
            }
            lastGeneratorStep = Time.time;
        }
    }

    void SupportUpkeep() {
        // Support Upkeep
        if (Time.time - lastSupportstep > timeBetweenSupportSteps) {
            if (suppActive == false) {
                if (curBatt > maxBatt) {
                    curBatt = maxBatt;
                }
            } else {
                if (curBatt > (maxBatt - suppCost)) {
                    curBatt = maxBatt - suppCost;
                }
            }
            lastSupportstep = Time.time;
        }
    }

    void HeatUpkeep() {
        //Heat Upkeep
        if (Time.time - lastHeatStep > timeBetweenHeatSteps) {
            if (curHeat > thresholdHeat) {
                heatDamageTick += 1;
                if (heatDamageTick > heatTickThreshold) {
                    curHull -= 1;
                    heatDamageTick -= heatTickThreshold;
                }
            }

            if (curHeat > 0) {
                curHeat -= heatDecreaseRate * heatDecreaseMultiplier;
                if (curHeat < 0) {
                    curHeat = 0;
                }
                VisualHeatEffect();
            }
            lastHeatStep = Time.time;
        }
    }

    void UpdatePrimaryWeapon() {
        timeBetweenPrimSteps = primBaseSpeed / primRate;
    }

    // Method called for dealing damage to vessel
    public void HitDamage(int damage) {
        if (curShield > 0) {
            if (curShield - damage <= 0) {
                curShield = 0;
                curHull -= damage;
            } else {
                curShield -= damage;
            }
            shield.GetComponent<Animator>().Play("hit");
        } else {
            curHull -= damage;
        }
    }

    public void ApplyHeat(float heat) {
        curHeat += heat;
        VisualHeatEffect();
    }

    // Test
    void VisualHeatEffect() {
        float colorScale = curHeat / thresholdHeat;
        GetComponent<SpriteRenderer>().color = new Color(1, 1 - 1 * colorScale, 1 - 1 * colorScale);
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
        if (Time.time - lastPrimStep > timeBetweenPrimSteps) {
            GetComponent<ProjectileController>().shootNormalProjectile();
            ApplyHeat(primHeat);
            lastPrimStep = Time.time;
        }
	}

	void FireAuxiliary() {
		curHeat += 10; //Placeholder for the creation of an Auxiliary Weapon Projectile
	}

	void ToggleSupport() {
        suppActive = !suppActive;
	}

    void enforceSpeedLimit()
    {
        if (rb.velocity.y > maxVelocity) {
            rb.velocity = new Vector2(rb.velocity.x, maxVelocity); 
        } else if (rb.velocity.x > maxVelocity) {
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        } else if (rb.velocity.y < -maxVelocity) {
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
        } else if (rb.velocity.x < -maxVelocity) {
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
        }
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
