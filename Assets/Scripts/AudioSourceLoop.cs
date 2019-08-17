using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceLoop : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public bool MusicActive = true;


    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.loop = true;
        m_AudioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MusicActive)
            {
                m_AudioSource.Stop();
                MusicActive = false;
            }
            else
            {
                m_AudioSource.Play();
                MusicActive = true;
            }
        }
        
    }
}