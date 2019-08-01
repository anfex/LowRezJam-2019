using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float startRotSpeed = 1.0f;

    public Vector3 point = Vector3.zero;
    public float currRotSpeed = 0.0f;
    public float speedIncrease = 1.0f;
    public float maxSpeed = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        currRotSpeed = startRotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(point, Vector3.up, currRotSpeed * Time.deltaTime);
        currRotSpeed += speedIncrease * Time.deltaTime;
    }
}
