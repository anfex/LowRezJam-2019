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
    public List<GameObject> branches = null;
    public float[] stepPlaceDegRange = new float[2];
    public Vector2[] stepPlacePosRange = new Vector2[2];
    public GameObject stepParent = null;

    // support vars
    float previousY = 0.0f;


    void Start()
    {
        currRotSpeed = startRotSpeed;
    }

    void Update()
    {
        transform.RotateAround(point, Vector3.up, currRotSpeed * Time.deltaTime);
        if (currRotSpeed < maxSpeed)
            currRotSpeed += speedIncrease * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewStep();
    }

    private void CreateNewStep()
    {
        // based on the previos step
        // based on the difficulty level
        // do the math and create a new step

        GameObject newStep =    Instantiate(branches[0], 
                                            new Vector3(Random.Range(stepPlacePosRange[0].x, stepPlacePosRange[0].y), 
                                                        Random.Range(stepPlacePosRange[1].x, stepPlacePosRange[1].y), 
                                                        0), 
                                            Quaternion.AngleAxis(Random.Range(stepPlaceDegRange[0], stepPlaceDegRange[1]), Vector3.up),
                                            stepParent.transform);
    }
}