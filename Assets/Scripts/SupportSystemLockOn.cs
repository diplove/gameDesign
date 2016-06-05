using UnityEngine;
using System.Collections;

public class SupportSystemLockOn : MonoBehaviour {

    private bool lockedOn;
    private GameObject lockOnMarker;
    private Rigidbody2D lockedOnTarget;
    private PrototypePlayer vessel;

    //Audio
    private GameObject audioObject;
    private AudioController ac;

    // Use this for initialization
    void Start () {
        lockOnMarker = GameObject.Find("LockOnTarget");
        vessel = GetComponent<PrototypePlayer>();

        audioObject = GameObject.Find("Audio");
        ac = audioObject.GetComponent<AudioController>();

        lockedOn = false;
        lockOnMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("a") && !lockedOn)
        {
            lockOnMarker.SetActive(true);
            Scan();
            
        }
        else if (Input.GetKeyDown("a") && lockedOn)
        {
            lockedOn = false;
            lockOnMarker.SetActive(false);
        }

        if (lockedOn)
            followTarget();

    }

    void Scan ()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 20);

        int i = 0;
        while (i < nearbyObjects.Length)
        {
            Rigidbody2D target = nearbyObjects[i].GetComponent<Rigidbody2D>();
            if (target && target.name != "Vessel")
            {
                if (target.tag == "asteroid" || target.tag == "enemyShip" || target.tag == "enemyTurret" || target.tag == "boss")
                {
                    float angle = 35;
                    if (Vector3.Angle(transform.up, target.position - (Vector2)transform.position) < angle)
                    {
                        lockOnMarker.SetActive(true);
                        lockedOnTarget = target;
                        lockedOn = true;
                        vessel.ApplyHeat(50);

                        ac.playLockOn();


                    }
                    else
                        lockOnMarker.SetActive(false);
                }
            }
            i++;
        }
    }

    void followTarget()
    {
        if (lockedOnTarget)
        {
            Vector3 vectorToTarget = (Vector3)lockedOnTarget.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 99);

            lockOnMarker.transform.position = lockedOnTarget.position;
        }
    }
}
