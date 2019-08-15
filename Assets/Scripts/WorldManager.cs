using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public ProtagonistManager protagonist = null;
    public LevelManager levelMgr = null;
    public CameraManager cameraMgr = null;
    public GameObject startSceneObject = null;
    public GameObject endSceneObject = null;
    public Animator endSceneAnimator = null;
    public GameObject ScoreObject = null;
    public Text scoreText = null;


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

        ScoreViewer();

        RestartMgr();
    }

    private void ScoreViewer()
    {
        scoreText.text = protagonist.Score.ToString();
    }

    private void RestartMgr()
    {
        if (!protagonist.Alive && Input.GetKeyDown(KeyCode.Space))
        {
            AnimatorStateInfo currStateInfo = endSceneAnimator.GetCurrentAnimatorStateInfo(0);
            if (currStateInfo.IsName("EndScreen_Loop"))
            {
                levelMgr.Reset();

                protagonist.Reset();

                if (endSceneObject != null)
                {
                    endSceneObject.SetActive(false);
                }
            }
        }
    }

    private void InitialStartGameMgr()
    {
        if (startSceneObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            startSceneObject.SetActive(false);
            levelMgr.Active = true;
            protagonist.Active = true;
            ScoreObject.SetActive(true);
        }
    }

    private void DeathChecker()
    {
        if (!protagonist.Alive)
        {
            if (endSceneObject != null)
            {
                endSceneObject.SetActive(true);
                endSceneAnimator.SetTrigger("PlayIntro");
            }
        }
    }

    private void Restart()
    {
        endSceneObject.SetActive(false);
        endSceneAnimator.SetTrigger("Reset");
    }
}
