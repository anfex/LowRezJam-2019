using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public ProtagonistManager protagonistToFollow;
    public Vector3 distToKeep;

    void Start()
    {
        distToKeep = transform.position - protagonistToFollow.transform.position;
    }

    void Update()
    {
        if (protagonistToFollow != null)
        {
            Vector3 newPos = new Vector3(transform.position.x, protagonistToFollow.transform.position.y + distToKeep.y, transform.position.z);
            transform.SetPositionAndRotation(newPos, Quaternion.identity);
        }
    }
}