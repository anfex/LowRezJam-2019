using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public GameObject gameObject;
    public GameObject splinterGo;
    public Animator anim;
    public Animator animCoin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (splinterGo != null)
            splinterGo.transform.LookAt(Camera.main.transform);
    }

    public bool KillBush()
    {
        bool returnVal = false;

        if (gameObject != null)
        {
            anim.SetTrigger("play");
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                returnVal = true;
            }
        }

        return returnVal;
    }

    public void GetCoin()
    {
        animCoin.SetTrigger("play");
    }
}
