using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip audioSource01;
    public AudioClip audioSource02;
    private int current;
    private bool changing = false;

    // Use this for initialization
    void Start () {
        current = 1;
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (changing)
        {
            if (current == 2)
            {
                audioSource.volume -= 2 * Time.deltaTime;
                if (audioSource.volume <= 0)
                {
                    audioSource.Stop();
                    audioSource.clip = audioSource02;
                    audioSource.Play();
                    changing = false;
                }
            }
            else
            {
                if (current == 1)
                {
                    audioSource.volume -= 2 * Time.deltaTime;
                    if (audioSource.volume <= 0)
                    {
                        audioSource.Stop();
                        audioSource.clip = audioSource01;
                        audioSource.Play();
                        changing = false;
                    }
                }
            }
        }
        else
        {
            if (audioSource.volume <= 1)
            {
                audioSource.volume += 2 * Time.deltaTime;
            }
        }
	}

    public void ChangingMusic()
    {
        if (current == 1)
        {
            current = 2;
            changing = true;
        }
        else
        {
            current = 1;
            changing = true;
        }
    }
}
