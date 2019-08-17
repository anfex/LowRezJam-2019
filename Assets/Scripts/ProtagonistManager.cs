using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistManager : MonoBehaviour
{
    public bool Active { get; set; } = false;

    public bool GravityEnabled = true;

    // protagonist specs
    public float jumpHeight = 1.5f;
    public Vector3 lateralVectorSpeed;
    public float walkAcceleration;
    public float airAcceleration;

    public Transform cylinder;
    public Vector3 constCylinderCenter;
    public float constDistFromCylinderCenter;
    public float minDistToGround = 0.5f;
    public float distCheck = 0.25f;
    public float maxChopDist = 0.5f;
    public float deathYDistanceGrace = 1.0f;
    private float floorMinYPos = 0.1261f;
    public int newStepPoints = 1;
    public int bushPoints = 3;

    public Animator protagonistAnimator = null;
    public LevelManager levelObject = null;

    public AudioSource[] wooshes = new AudioSource[3];
    public AudioSource impact;

    // supporting vars
    float deadYPosThreshold = 0.0f;  // TODO: should be based on the previous step height
    bool jumping = false;
    bool jumpRequesting = false;
    bool grounded = false;
    int layerGround = 8;
    float currDistFromGround;
    Vector3 startPos;
    RaycastHit hitInfo;
    public Vector3 velocity;
    int lastSafeStepId = int.MinValue;
    float lastSafeStepYPos = float.MinValue;

    AF_MessageHandler msgHandler = null;

    public int Score { get; set; }
    public Vector3 CurrPosition { get { return transform.position; } }
    public bool Alive { get; set; }


    private void Awake()
    {
        msgHandler = new AF_MessageHandler();
    }

    void Start()
    {
        Alive = true;
        startPos = transform.position;
        constCylinderCenter = new Vector3(cylinder.position.x, 0.0f, cylinder.position.z);
        constDistFromCylinderCenter = Vector3.Distance(constCylinderCenter, new Vector3(transform.position.x, 0.0f, transform.position.z));
        Score = 0;
    }

    void Update()
    {
        if (Active)
        {
            if (Alive)
            {
                CheckGrounded();
                UserInputsMgr();
                GravityMgr();
                DeathMgr();
                AnimationMgr();
            }
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //    Reset();
    }

    private void DeathMgr()
    {
        // Dead when:
        // protagonist Y position goes below last safe step Y
        // (floor excluded. This rule starts after landing on first step)

        if (transform.position.y < lastSafeStepYPos-deathYDistanceGrace)
        {
            Alive = false;
        }
    }

    private void AnimationMgr()
    {
        protagonistAnimator.SetBool("Jumping", velocity.y != 0);
    }

    public void Reset()
    {
        transform.position = startPos;
        velocity = Vector3.zero;
        Alive = true;
        lastSafeStepId = int.MinValue;
        lastSafeStepYPos = float.MinValue;
        Score = 0;
    }

    private void CheckGrounded()
    {
        grounded = false;
        if (velocity.y <= 0 && Physics.Raycast(transform.position, -Vector3.up, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == layerGround)
            {
                currDistFromGround = hitInfo.distance;
                if (currDistFromGround <= minDistToGround)
                {
                    grounded = true;
                    velocity.y = 0;

                    // try to fix the Y position and not make him fall into the ground/step
                    //float newY = minDistToGround - currDistFromGround;
                    //Vector3 newPos = transform.position + new Vector3(0.0f, newY, 0.0f);
                    //transform.SetPositionAndRotation(newPos, transform.rotation);

                    // check new step
                    int currStepID = hitInfo.transform.gameObject.GetInstanceID();
                    if (lastSafeStepId != currStepID
                        && hitInfo.transform.position.y > lastSafeStepYPos
                        && hitInfo.transform.gameObject.name != "Floor")
                    {
                        lastSafeStepId = currStepID;
                        lastSafeStepYPos = transform.position.y;
                        Score += newStepPoints;
                        levelObject.CreateNewStep();

                        // play coin anim
                        if (hitInfo.transform.gameObject.GetComponent<Branch>() != null)
                        {
                            //hitInfo.transform.gameObject.GetComponent<Branch>().GetCoin();
                        }
                    }
                }    
            }
        }
    }

    private void UserInputsMgr()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequesting = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpRequesting = false;
            if (velocity.y > 0)
                velocity.y = -0.1f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChopDown();
            protagonistAnimator.SetTrigger("Chop");
        }
    }

    private void ChopDown()
    {
        int random = UnityEngine.Random.Range(0, 2);
        wooshes[random].Play();

        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == layerGround)
            {
                currDistFromGround = hitInfo.distance;
                if (currDistFromGround <= maxChopDist)
                {
                    // play chopping anim
                    if (hitInfo.transform.gameObject.GetComponent<Branch>() != null)
                    {
                        bool chopped = hitInfo.transform.gameObject.GetComponent<Branch>().KillBush();
                        if (hitInfo.transform.gameObject.name == "Branch_tier3" && chopped)
                        {
                            Score += bushPoints;
                            impact.Play();
                        }
                    }
                }
            }
        }
    }

    private void GravityMgr()
    {
        if (GravityEnabled)
        {
            if (jumpRequesting)
            {
                if (!jumping && grounded)
                {
                    jumping = true;
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                }
            }
            else
                jumping = false;

            if (!grounded)
                velocity.y += Physics2D.gravity.y * Time.deltaTime;

            transform.Translate(velocity * Time.deltaTime);

            if (transform.position.y < floorMinYPos)
                transform.position = new Vector3(transform.position.x, floorMinYPos, transform.position.z);
        }
    }

    private void KeepCylindricalDist()
    {
        Vector3 currPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        float distanceFromCnt = Vector3.Distance(currPos, constCylinderCenter);
        if (distanceFromCnt != constDistFromCylinderCenter)
        {
            Vector3 fromOriginToObject = currPos - constCylinderCenter; 
            fromOriginToObject *= constDistFromCylinderCenter / distanceFromCnt; 
            Vector3 newLocation = constCylinderCenter + fromOriginToObject; 

            transform.position = new Vector3(Mathf.Abs(newLocation.x), Mathf.Abs(transform.position.y), Mathf.Abs(newLocation.z));
        }
    }

    private void HorizMovementMgr()
    {
        Vector3 lateralSpeed = Vector3.zero;
        lateralSpeed.x = grounded ? walkAcceleration : airAcceleration;

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.SetPositionAndRotation(transform.position - lateralSpeed, transform.rotation);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.SetPositionAndRotation(transform.position + lateralSpeed, transform.rotation);
    }
}