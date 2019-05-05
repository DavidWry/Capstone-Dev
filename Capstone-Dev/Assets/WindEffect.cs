using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour {

    public float Strength;
    public Vector3 WindDirection;
    public float speed;
    public GameObject effect;

    private bool shouldMakeSound;
    private bool hasMadeSound;
    private float soundTime;
    private int num;


    private void Start()
    {
        shouldMakeSound = true;
        hasMadeSound = false;
        soundTime = 6f;
    }

    private void Update()
    {
        transform.position += new Vector3(speed,0,0);
        if (shouldMakeSound == true && soundTime == 6f)
        {
            num = Random.Range(1, 4);
            if (num == 1)
            {
                SoundManager.PlaySound("Wind1");
            }
            else if (num == 2)
            {
                SoundManager.PlaySound("Wind2");
            }
            else if (num == 3)
            {
                SoundManager.PlaySound("Wind3");
            }
            
            shouldMakeSound = false;
        }
        soundTime -= Time.deltaTime;

        if (soundTime <= 0.0f)
        {
            shouldMakeSound = true;
            soundTime = 6f;
        }
        
        //   if (shouldMakeSound == true && hasMadeSound == false)
        //    {

        //        hasMadeSound = true;
        //     }

        // Instantiate(effect, transform.position, Quaternion.identity);
    }

    void OnTriggerStay(Collider col)
    {
        Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
        if (colRigidbody != null)
        {
            colRigidbody.AddForce(WindDirection * Strength);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
        if (colRigidbody != null)
        {
            colRigidbody.AddForce(WindDirection * Strength);
            
        }
    }
}
