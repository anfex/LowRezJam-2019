using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public float startRotSpeed = 1.0f;

    public Vector3 point = Vector3.zero;
    public float currRotSpeed = 0.0f;
    public float speedIncrease = 1.0f;
    public float maxSpeed = 20.0f;
    public List<GameObject> branches = null;

    public int difficultySteps = 5;
    public Vector4[] stepPlacePosRanges = new Vector4[5];
    public Vector2[] stepPlaceDegRanges = new Vector2[5];
    public float timeForNextDifficultyLevel = 15;
    public float currTimeLeftForNextLevel;

    public GameObject stepParent = null;
    public int startingSteps = 5;

    // support vars
    public int currDifficultyStep = 0;
    float previousY = 0.0f;
    float previousDeg = 90.0f;

    public bool Active { get; set; } = false;


    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (Active)
        {
            transform.RotateAround(point, Vector3.up, currRotSpeed * Time.deltaTime);

            if (currRotSpeed < maxSpeed)
                currRotSpeed += speedIncrease * Time.deltaTime;

            currTimeLeftForNextLevel -= Time.deltaTime;
            if (currTimeLeftForNextLevel < 0)
            {
                currTimeLeftForNextLevel = timeForNextDifficultyLevel;
                if (currDifficultyStep < difficultySteps)
                    currDifficultyStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
            Reset();

        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewStep();
    }

    public void Reset()
    {
        currRotSpeed = startRotSpeed;
        previousY = 0.0f;
        previousDeg = 90.0f;
        currDifficultyStep = 0;
        currTimeLeftForNextLevel = timeForNextDifficultyLevel;

        foreach (Transform child in stepParent.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < startingSteps; i++)
            CreateNewStep();
    }

    public void CreateNewStep()
    {
        previousY += Random.Range(stepPlacePosRanges[currDifficultyStep].z, stepPlacePosRanges[currDifficultyStep].w);
        previousDeg += Random.Range(stepPlaceDegRanges[currDifficultyStep].x, stepPlaceDegRanges[currDifficultyStep].y);
        Quaternion newRot = stepParent.transform.rotation * Quaternion.Euler(0, previousDeg, 0);

        GameObject newStep = Instantiate(branches[Random.Range(0, branches.Count)],
                                            new Vector3(Random.Range(stepPlacePosRanges[currDifficultyStep].x, stepPlacePosRanges[currDifficultyStep].y), previousY, 0),
                                            newRot,
                                            stepParent.transform);
    }
}