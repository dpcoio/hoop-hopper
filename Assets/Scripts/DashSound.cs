using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSound : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        int clipIndex = Random.Range(0, clips.Length);
        AudioClip clip = clips[clipIndex];
        audioSource.clip = clip;
        audioSource.Play();
    }
}
