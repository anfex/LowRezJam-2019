using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/Difficulty Levels", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    public string prefabName;

    const int number = 5;

    public float constStepsDeg;
    public float[] stepPlaceDegRange = new float[number];

    public Vector2[] stepPlacePosRange = new Vector2[number];
}