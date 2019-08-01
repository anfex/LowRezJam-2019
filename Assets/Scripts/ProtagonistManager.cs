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
    // protagonist specs
    public Vector3 jumpForce = new Vector3(0, 10, 0);
    public float maxJumpTime = 3.0f;
    public float minDistToGround = 0.5f;
    public float gravitySpeed = 0.2f;
    public float jumpTime = 2.0f;
    public float jumpHeight = 1.5f;

    // supporting vars
    public Rigidbody rb = null;
    public bool jumping = false;
    public bool jumpRequesting = false;
    public bool grounded = false;
    public int layerGround = 8;
    public float currDistFromGround;
    public float jump_peakYpos;
    public RaycastHit hitInfo;
    public Tween jumpTweener;

    private void Awake()
    {
        rb = this.transform.GetComponent<Rigidbody>();
    }

    void Start()
    {
    }

    void Update()
    {
        
        

        CheckGrounded();
        Gravity();
        JumpRequestSolver();
        JumpResolver();
    }

    private void Gravity()
    {
        Vector3 newPos = transform.position;

        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == layerGround)
            {
                currDistFromGround = hitInfo.distance;
                if (currDistFromGround > minDistToGround)
                    newPos += new Vector3(0.0f, -gravitySpeed, 0.0f);
                else
                    newPos += new Vector3(0.0f, minDistToGround-currDistFromGround, 0.0f);
            }
        }

        transform.SetPositionAndRotation(newPos, transform.rotation);
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
                jumpTweener = transform.DOMoveY(jump_peakYpos, jumpTime).OnComplete(JumpComplete);
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
}