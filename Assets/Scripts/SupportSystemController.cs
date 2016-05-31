using UnityEngine;
using System.Collections;

public class SupportSystemController : MonoBehaviour {

    private PrototypePlayer vessel;
    
    bool active;
    bool lockedOn;
    private GameObject lockOnMarker;
    Rigidbody2D lockedOnTarget;

    private LockOnTargetController lockOnCrontroller;

    // Use this for initialization
    void Start () {
        vessel = GetComponent<PrototypePlayer>();
        lockOnMarker = GameObject.Find("LockOnTarget");
        lockOnCrontroller = lockOnMarker.GetComponent<LockOnTargetController>();

        lockedOn = false;
        active = false;
        lockOnMarker.SetActive(false);


    }



    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("l") && !active)
        {
            lockOnMarker.SetActive(true);
            active = true;
        }
        else if (Input.GetKeyDown("l") && active)
        {
            active = false;
            lockOnMarker.SetActive(false);
        }


        if (active)
            Scan();


        if (lockedOn)
            followTarget();

        if (Input.GetKeyDown("j"))
        {
            lockedOn = false;
            lockOnMarker.SetActive(false);
        }



    }

    void Scan ()
    {
        

        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 20);
        //Debug.Log(nearbyObjects.Length);
        int i = 0;
        while (i < nearbyObjects.Length)
        {
            Rigidbody2D target = nearbyObjects[i].GetComponent<Rigidbody2D>();
            if (target && target.name != "Vessel" && target.tag == "asteroid")
            {
                float angle = 35;
                //Debug.Log(transform.forward);
                //Debug.Log(Vector3.Angle(transform.forward, target.transform.position - transform.position));
                if (Vector3.Angle(transform.up, target.position - (Vector2)transform.position) < angle)
                {
                    lockOnMarker.SetActive(true);
                    lockOnCrontroller.lockOnPosition(target.position);

                    if (Input.GetKeyDown("k") && !lockedOn)
                    {
                        lockedOnTarget = target;
                        lockedOn = true;
                        active = false;
                    }
                }
                else
                    lockOnMarker.SetActive(false);
            }
           


            i++;
        }


    }

    void followTarget()
    {
        Vector3 vectorToTarget = (Vector3)lockedOnTarget.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 99);

        lockOnCrontroller.lockOnPosition(lockedOnTarget.position);
    }
}
