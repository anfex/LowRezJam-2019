using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// task: make it jump a la mario
// task: distance from center of the cylinder should always be the same
// task: make it stop on the ground, not stay in the center all the time

public class ProtagonistManager : MonoBehaviour
{
    public bool GravityEnabled = true;

    // protagonist specs
    public float minDistToGround = 0.5f;
    public float gravitySpeed = 0.2f;
    public float jumpTime = 2.0f;
    public float jumpHeight = 1.5f;
    public Vector3 lateralVectorSpeed;
    public Transform cylinder;
    public Vector3 constCylinderCenter;
    public float constDistFromCylinderCenter;
    public float distCheck = 0.25f;
    public Ease jumpEasingType = Ease.InOutQuad;

    // supporting vars
    bool jumping = false;
    bool jumpRequesting = false;
    bool grounded = false;
    int layerGround = 8;
    float currDistFromGround;
    float jump_peakYpos;
    Vector3 startPos;
    RaycastHit hitInfo;
    Tween jumpTweener;

    private void Awake()
    {
        DOTween.Init();
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
        Gravity();
        JumpRequestSolver();
        JumpResolver();
        LeftRightResolver();
        KeepCylindricalDist();

        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }

    private void Reset()
    {
        transform.position = startPos;
    }

    private void Gravity()
    {
        if (GravityEnabled)
        {
            Vector3 newPos = transform.position;

            if (!jumping)
            {
                if (!grounded)
                    newPos += new Vector3(0.0f, -gravitySpeed, 0.0f);

                if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, distCheck))
                {
                    if (hitInfo.transform.gameObject.layer == layerGround)
                    {
                        currDistFromGround = hitInfo.distance;
                        if (currDistFromGround <= minDistToGround)
                            newPos = transform.position + new Vector3(0.0f, minDistToGround-currDistFromGround, 0.0f);
                    }
                }
            }

            transform.SetPositionAndRotation(newPos, transform.rotation);
        }
    }

    private void CheckGrounded()
    {
        grounded = false;
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == layerGround)
            {
                currDistFromGround = hitInfo.distance;
                grounded = currDistFromGround <= minDistToGround;
            }
        }
    }

    private void JumpRequestSolver()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequesting = true;

        if (Input.GetKeyUp(KeyCode.Space))
            jumpRequesting = false;
    }

    private void JumpResolver()
    {
        if (jumpRequesting)
        {
            if (!jumping && grounded)
            {
                jumping = true;
                jump_peakYpos = transform.position.y + jumpHeight;
                jumpTweener = transform.DOMoveY(jump_peakYpos, jumpTime).SetEase(jumpEasingType).OnComplete(JumpComplete);
            }
        }
        else
        {
            jumping = false;
            jumpTweener.Kill();
        }
    }

    private void JumpComplete()
    {
        jumpRequesting = jumping = false;
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

            transform.position = new Vector3(newLocation.x, transform.position.y, newLocation.z);
        }
    }

    private void LeftRightResolver()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.SetPositionAndRotation(transform.position - lateralVectorSpeed, transform.rotation);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.SetPositionAndRotation(transform.position + lateralVectorSpeed, transform.rotation);
    }
}