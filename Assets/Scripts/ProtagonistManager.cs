using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistManager : MonoBehaviour
{
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

    public Animator protagonistAnimator = null;
    public LevelManager levelObject = null;

    // supporting vars
    bool jumping = false;
    bool jumpRequesting = false;
    bool grounded = false;
    int layerGround = 8;
    float currDistFromGround;
    Vector3 startPos;
    RaycastHit hitInfo;
    public Vector3 velocity;

    private void Awake()
    {
    }

    void Start()
    {
        startPos = transform.position;
        constCylinderCenter = new Vector3(cylinder.position.x, 0.0f, cylinder.position.z);
        constDistFromCylinderCenter = Vector3.Distance(constCylinderCenter, new Vector3(transform.position.x, 0.0f, transform.position.z));
    }

    void Update()
    {
        CheckGrounded();
        JumpRequestMgr();
        GravityMgr();
        AnimationMgr();

        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }

    private void AnimationMgr()
    {
        protagonistAnimator.SetBool("Jumping", velocity.y != 0);
    }

    private void Reset()
    {
        transform.position = startPos;
        velocity = Vector3.zero;
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

                    //float newY = minDistToGround - currDistFromGround;
                    //Vector3 newPos = transform.position + new Vector3(0.0f, newY, 0.0f);
                    //transform.SetPositionAndRotation(newPos, transform.rotation);
                }    
            }
        }
    }

    private void JumpRequestMgr()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequesting = true;
            levelObject.CreateNewStep();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpRequesting = false;
            if (velocity.y > 0)
                velocity.y = -0.1f;
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