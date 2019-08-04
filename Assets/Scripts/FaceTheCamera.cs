using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTheCamera : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        // Point the object at the world origin
        transform.LookAt(target);
    }
}
