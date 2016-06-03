using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class PrototypePlayer : MonoBehaviour {
    //Mobility
	public float maxVelocity;
    public float accelerationRate;
    public float decelerationRate;
    public float turnAmount;
	public float turnRate;
	private float overMaxSpeedDecelerationRate;

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
	public bool hasAux;

	public int auxDamage;
	public int auxType;
	public int auxRate;
	public float auxBaseSpeed;
	public int auxCost;
	public float auxHeat;

	private float lastAuxStep;
	private float timeBetweenAuxSteps;

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

    // Death Effects
    public GameObject deathExplosion;
    private bool dead = false;

    //Audio
    private GameObject audioObject;
    private AudioController ac;


    // Use this for initialization
    void Start () {
	rb = GetComponent<Rigidbody2D>();
        rotation = transform.rotation.eulerAngles;

        curHull = maxHull; // Current is set to max at start
        curShield = maxShield; // Current shield is initialised to the max value
        curBatt = maxBatt;
        curHeat = 0;
		overMaxSpeedDecelerationRate = 0.30f;

        audioObject = GameObject.Find("Audio");
        ac = audioObject.GetComponent<AudioController>();


        UpdatePrimaryWeapon();
		UpdateAuxiliaryWeapon();
	}

    void FixedUpdate() {

        if (!landed) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), turnRate * Time.deltaTime);
        }
        if (!dead) {
            HeatUpkeep();
            GeneratorUpkeep();
            SupportUpkeep();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Controls
        if (Input.GetKey(KeyCode.UpArrow))
        {           
            Accelerate();
            ac.playThrust();
           
        }
        if (Input.GetKeyUp("up"))
        {
            ac.fadeThrust();
           
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
        if (Input.GetKey(KeyCode.Alpha1))
        {
            EditorSceneManager.LoadScene(1);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            EditorSceneManager.LoadScene(2);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            EditorSceneManager.LoadScene(3);
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            EditorSceneManager.LoadScene(4);
        }

        if (Input.GetKey(KeyCode.Alpha5))
        {
            EditorSceneManager.LoadScene(5);
        }



        if (Input.GetKey("z")) {
			if (curBatt >= primCost) {
				//curBatt -= primCost;
				//curHeat += primHeat;
				FirePrimary();
			} else {
				StopPrimary ();
			}
		}

		if (Input.GetKeyUp("z")) {
			StopPrimary();
		}

		if (Input.GetKey("x")) {
			if (curBatt >= auxCost && hasAux == true) {
				//curBatt -= auxCost;
				//curHeat += auxHeat;
				FireAuxiliary();
			}
		}

		if (Input.GetKeyUp("x")) {
			if (hasAux == true) {
				StopAux();
			}
		}

		if (Input.GetKeyDown("c")) {
			ToggleSupport ();
		}
    }


    void Respawn() {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().isKinematic = false;
        curHeat = 0;
        VisualHeatEffect();
        curShield = maxShield;
        shield.GetComponent<ShieldController>().activateShield();
        curHull = maxHull;
        curBatt = maxBatt;
        transform.position = GameObject.Find("SpaceDock").transform.position;
        transform.rotation = GameObject.Find("SpaceDock").transform.rotation;
        rotation = transform.rotation.eulerAngles;
        dead = false;
    }

    public float getCurrentShield() {
        return curShield;
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
                heatDamageTick = (int)(curHeat - thresholdHeat);
                if (heatDamageTick > heatTickThreshold) {
                    if ((curHull -= heatDamageTick) < 0) {
                        curHull = 0;
                        DestroySelf();
                    }
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

	void UpdateAuxiliaryWeapon() {
		timeBetweenAuxSteps = auxBaseSpeed / auxRate;
	}

    public void DestroySelf() {
        if (!dead) {
            StopAux();
            StopPrimary();
            ac.playDeath();
            dead = true;
            Instantiate(deathExplosion, transform.position, transform.rotation);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            GetComponent<SpriteRenderer>().enabled = false;
            Invoke("Respawn", 3);
        }
    }

    public bool getDeathState() {
        return dead;
    }

    // Method called for dealing damage to vessel
    public void HitDamage(int damage) {
        //Debug.Log("Getting hit for: " + damage);
        if (!dead) {
            if (curShield > 0) {
                
                if (curShield - damage <= 0) {
                    int diff = damage - curShield;
                    curShield = 0;
                    curHull -= diff;
                } else {
                    curShield -= damage;
                }
                shield.GetComponent<Animator>().Play("hit");

                ac.playShieldHit();


            } else {
                ac.playHullHit();
                if ((curHull -= damage) <= 0) {
                    curHull = 0;

                    DestroySelf();
                }
            }
        }
    }

     public void ApplyHeat(float heat) {
        curHeat += heat;
        VisualHeatEffect();
    }

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
        if (!dead)
        {
            if (Time.time - lastPrimStep > timeBetweenPrimSteps && curBatt >= primCost)
            {
                // Test for weapon type (0 = projectile, 1 = laser) and act accordingly
                curBatt -= primCost;
                if (primType == 0)
                {
                    GetComponent<ProjectileController>().shootNormalProjectile();

                    ac.playShootProjectile();
                }
                else {
                    GetComponent<LaserController>().activateLaser();

                    ac.playShootLaserPulse();

                }
                ApplyHeat(primHeat);
                lastPrimStep = Time.time;
            }
            else {
                StopPrimary();
            }
        }
	}

	void StopPrimary() {
		if (primType == 1) {
			GetComponent<LaserController> ().deactivateLaser ();
		}
	}

	void FireAuxiliary() {
        //curHeat += 10; //Placeholder for the creation of an Auxiliary Weapon Projectile
        //curBatt -= auxCost;
        if (!dead)
        {
            if (Time.time - lastAuxStep > timeBetweenAuxSteps && curBatt >= auxCost)
            {
                // Test for weapon type (0 = projectile, 1 = laser) and act accordingly
                curBatt -= auxCost;
                if (auxType == 0)
                {
                    GetComponent<Projectile2Controller>().shootNormalProjectile();
                }
                else {
                    GetComponent<Laser2Controller>().activateLaser();
                    ac.playShootLaser();

                }
                ApplyHeat(auxHeat);
                lastAuxStep = Time.time;
            }
            else {
                StopAux();
            }
        }
	}

	void StopAux() {
		if (auxType == 1) {
			GetComponent<Laser2Controller> ().deactivateLaser ();
            ac.fadeShootLaser();
        }
	}

	void ToggleSupport() {
        suppActive = !suppActive;
	}

    void enforceSpeedLimit()
    {
        //if (rb.velocity.y > maxVelocity) {
        //    rb.velocity = new Vector2(rb.velocity.x, maxVelocity); 
        //}
        //if (rb.velocity.x > maxVelocity) {
		//	rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        //}
        //if (rb.velocity.y < -maxVelocity) {
        //    rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
        //}
        //if (rb.velocity.x < -maxVelocity) {
		//	rb.velocity = new Vector2(-maxVelocity, rb.velocity.y);
        //}

		if (Mathf.Sqrt(Mathf.Abs(rb.velocity.x*rb.velocity.x + rb.velocity.y*rb.velocity.y)) > Mathf.Sqrt(maxVelocity*maxVelocity)) {
			if (rb.velocity.x < 0) {
				rb.velocity = new Vector2(rb.velocity.x + Mathf.Abs(overMaxSpeedDecelerationRate*decelerationRate), rb.velocity.y);
			} else if (rb.velocity.x > 0) {
				rb.velocity = new Vector2(rb.velocity.x - Mathf.Abs(overMaxSpeedDecelerationRate*decelerationRate), rb.velocity.y);
			}

			if (rb.velocity.y < 0) {
				rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + Mathf.Abs(overMaxSpeedDecelerationRate*decelerationRate));
			} else if (rb.velocity.y > 0) {
				rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - Mathf.Abs(overMaxSpeedDecelerationRate*decelerationRate));
			}
		}
    }

	/* William's code from his Digital Multimedia subject
	velX = velX + maxAccel*cos(rotationValue);
    velY = velY + maxAccel*sin(rotationValue);
	if (sqrt(abs(velX*velX + velY*velY))>sqrt(abs(maxVel*maxVel))) {
		//reduce both velocity values proportionally
		float curVelRot;    
		curVelRot = atan(velY/velX);


		if (velX < 0) {
			velX = velX + abs(maxAccel*cos(curVelRot));
		}
		else if (velX > 0) {
			velX = velX - abs(maxAccel*cos(curVelRot));
		}

		if (velY < 0) {
			velY = velY + abs(maxAccel*sin(curVelRot));
		}
		else if (velY > 0) {
			velY = velY - abs(maxAccel*sin(curVelRot));
		}
	}
	*/

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
