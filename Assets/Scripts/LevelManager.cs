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
    public float constStepsDeg = -45;
    public Vector2[] stepPlacePosRange = new Vector2[3];
    public GameObject stepParent = null;

    // support vars
    float previousY = 0.0f;
    float previousDeg = 90.0f;


    void Start()
    {
        currRotSpeed = startRotSpeed;
        
        for (int i = 0; i < 1000; i++)
            CreateNewStep();
    }

    void Update()
    {
        transform.RotateAround(point, Vector3.up, currRotSpeed * Time.deltaTime);
        if (currRotSpeed < maxSpeed)
            currRotSpeed += speedIncrease * Time.deltaTime;
    }

    private void CreateNewStep()
    {
        previousY += Random.Range(stepPlacePosRange[1].x, stepPlacePosRange[1].y);
        previousDeg += Random.Range(stepPlaceDegRange[0], stepPlaceDegRange[1]) + constStepsDeg;

        GameObject newStep =    Instantiate(branches[Random.Range(0,branches.Count)], 
                                            new Vector3(Random.Range(stepPlacePosRange[0].x, stepPlacePosRange[0].y), 
                                                        previousY,
                                                        Random.Range(stepPlacePosRange[2].x, stepPlacePosRange[2].y)), 
                                            Quaternion.AngleAxis(previousDeg, Vector3.up),
                                            stepParent.transform);
    }
}