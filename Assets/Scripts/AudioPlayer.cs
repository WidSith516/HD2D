using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum AudioTag
{
    Attack,
    TakeDamage,
    Action,
};

public class AudioPlayer : MonoBehaviour
{
    public AudioTag Tag;
    public AudioClip AC;

    private AudioSource AS;

    // Start is called before the first frame update
    void Start()
    {
        AS = gameObject.AddComponent<AudioSource>();
        AS.clip = AC;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAudio(bool bNeedStop = false)
    {
        if (bNeedStop)
        {
            StopAudio();
        }

        if (AC == null)
        {
            return;
        }

        AS.Play();
    }

    public void StopAudio()
    {
        if (AS.isPlaying)
        {
            AS.Stop();
        }
    }
}
