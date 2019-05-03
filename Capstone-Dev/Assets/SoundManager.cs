using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    // Use this for initialization
    
    public static  AudioClip click;
    public static AudioClip scroll;

    public static AudioClip jumperJump;
    public static AudioClip sliderSlide;
    public static AudioClip suiciderExplode;

    public static AudioClip pickup;
    public static AudioClip healthPickup;

    public static AudioClip rain;
    public static AudioClip wind1;
    public static AudioClip wind2;
    public static AudioClip wind3;

    static AudioSource audioSrc;

    void Start ()
    {
        click = Resources.Load<AudioClip>("Click");
        scroll = Resources.Load<AudioClip>("Scroll");
        

        jumperJump = Resources.Load<AudioClip>("Jumper");
        sliderSlide = Resources.Load<AudioClip>("Sliding");
        suiciderExplode = Resources.Load<AudioClip>("Suicider_Exploding");

        pickup = Resources.Load<AudioClip>("Pickup");
        healthPickup = Resources.Load<AudioClip>("Health_Pack_Pickup");

        rain = Resources.Load<AudioClip>("Raining");
        wind1 = Resources.Load<AudioClip>("Wind1");
        wind2 = Resources.Load<AudioClip>("Wind2");
        wind3 = Resources.Load<AudioClip>("Wind3");

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public static void PlaySound(string clip)
    {
      
        switch (clip)
        {
            case "Click":
                audioSrc.PlayOneShot(click);
                break;

            case "Scroll":
                audioSrc.PlayOneShot(scroll);
                break;

            case "Jumper":
                audioSrc.PlayOneShot(jumperJump);
                break;

            case "Sliding":
                audioSrc.PlayOneShot(sliderSlide);
                break;

            case "Suicider_Exploding":
                audioSrc.PlayOneShot(suiciderExplode);
                break;

            case "Pickup":
                audioSrc.PlayOneShot(pickup);
                break;

            case "Health_Pack_Pickup":
                audioSrc.PlayOneShot(healthPickup);
                break;

            case "Raining":
                audioSrc.PlayOneShot(rain);
                break;

            case "Wind1":
                audioSrc.PlayOneShot(wind1);
                break;

            case "Wind2":
                audioSrc.PlayOneShot(wind2);
                break;

            case "Wind3":
                audioSrc.PlayOneShot(wind3);
                break;

            

        }
    }
}
