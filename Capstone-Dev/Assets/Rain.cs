using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    private bool shouldMakeSound;
    private bool hasMadeSound;
    private float soundTime;

    private float hp;

    private float lifeTime;

    // Use this for initialization
    void Start ()
    {
        shouldMakeSound = true;
        hasMadeSound = false;
        soundTime = 16f;
        lifeTime = 15f;

    }

     void OnEnable()
    {
        shouldMakeSound = true;
        hasMadeSound = false;
        soundTime = 16f;
        lifeTime = 15f;
    }

    // Update is called once per frame
    void Update ()
    {
        if (lifeTime <= 0)
        {
            gameObject.SetActive(false);
            shouldMakeSound = false;
            lifeTime = 15f;
        }
        lifeTime -= Time.deltaTime;


        if (shouldMakeSound == true && soundTime == 16f)
        {
            SoundManager.PlaySound("Raining");
            shouldMakeSound = false;
        }
        soundTime -= Time.deltaTime;


        if(soundTime <= 0.0f)
        {
            shouldMakeSound = true;
            soundTime = 16f;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Minion")
        {
           

            if (other.gameObject.GetComponent<EnemyRangedSpear>())
            {
                //    Debug.Log("I am here spear");
                //    hp = other.GetComponent<EnemyRangedSpear>().health;
                //   hp = hp + 0.1f;
                //   other.GetComponent<EnemyRangedSpear>().health = hp;
                other.GetComponent<EnemyRangedSpear>().RainHealthUpdate(0.1f);
                other.GetComponent<EnemyRangedSpear>().shouldInstParticle = true;  
                
            }
            else if (other.gameObject.GetComponent<NewEnemyJumper>())
            {
                other.GetComponent<NewEnemyJumper>().RainHealthUpdate(0.1f);
                other.GetComponent<NewEnemyJumper>().shouldInstParticle = true;
            }
            else if (other.gameObject.GetComponent<EnemySlider>())
            {
                other.GetComponent<EnemySlider>().RainHealthUpdate(0.1f);
                other.GetComponent<EnemySlider>().shouldInstParticle = true;
            }
            else if (other.gameObject.GetComponent<EnemyRangedStomp>())
            {
                other.GetComponent<EnemyRangedStomp>().RainHealthUpdate(0.1f);
                other.GetComponent<EnemyRangedStomp>().shouldInstParticle = true;
            }
            else if (other.gameObject.GetComponent<EnemySuicideBomber>())
            {
                other.GetComponent<EnemySuicideBomber>().RainHealthUpdate(0.1f);
                other.GetComponent<EnemySuicideBomber>().shouldInstParticle = true;
            }

        }

       
    }
}
