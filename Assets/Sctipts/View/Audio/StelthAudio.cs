using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StelthAudio : MonoBehaviour
{
    private bool playConstant = false;

    private int playCount = 0;
    private float deltaTime;

    public AudioSource audioSource { get; private set; }
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        playCount++;
        if (deltaTime < 0.5f)
        {
            playConstant = true;
        }
        else
        {
            playConstant = false;
        }
        audioSource.Play();
        deltaTime = 0;
    }
    public void FixedUpdate()
    {
        deltaTime += Time.fixedDeltaTime;
    }
    public float GetSuspiciousLevel()
    {
        var tmp = 0.0f;
        if(playCount > 100)
        {
            playCount = 100;
        }

        if(playCount > 80)
        {
            tmp = 100.0f - playCount;
        }
        else if(playConstant)
        {
            tmp = 80.0f;
        }
        else
        {
            tmp = 100.0f - playCount;
        }

        return tmp / 100.0f;
    }
}
