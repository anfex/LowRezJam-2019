using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject follow = null;
    public float startSpeed = 0;
    public float increment = 0;
    public float maxSpeed = 0;

    public float currSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        currSpeed = startSpeed;    
    }

    // Update is called once per frame
    void Update()
    {
        if (currSpeed < maxSpeed)
            currSpeed += increment * Time.deltaTime;

        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + currSpeed, transform.position.z);
        transform.SetPositionAndRotation(newPos, Quaternion.identity);
    }
}
