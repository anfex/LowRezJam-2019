using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public ProtagonistManager protagonist = null;
    public LevelManager levelMgr = null;
    public CameraManager cameraMgr = null;
    public GameObject startSceneObject = null;
    public GameObject endSceneObject = null;



    private void Awake()
    {
        Restart();
        startSceneObject.SetActive(true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        AF_MessageManager.Instance.Update();

        InitialStartGameMgr();

        DeathChecker();

        RestartMgr();
    }

    private void RestartMgr()
    {
        if (!protagonist.Alive && Input.GetKeyDown(KeyCode.Space))
        {
            levelMgr.Reset();

            protagonist.Reset();

            if (endSceneObject != null)
                endSceneObject.SetActive(false);
        }
    }

    private void InitialStartGameMgr()
    {
        if (startSceneObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            startSceneObject.SetActive(false);
            levelMgr.Active = true;
            protagonist.Active = true;
        }
    }

    private void DeathChecker()
    {
        if (!protagonist.Alive)
        {
            if (endSceneObject != null)
                endSceneObject.SetActive(true);
        }
    }

    private void Restart()
    {
        endSceneObject.SetActive(false);
    }
}
