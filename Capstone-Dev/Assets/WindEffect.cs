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

    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    private int posnum;

    private Vector3 tempStartLoc;

    private float lifeTime;

    private void Start()
    {
        shouldMakeSound = true;
        hasMadeSound = false;
        soundTime = 6f;

        if (gameObject.name == "Long_Winds")
        {
            lifeTime = 23f;
        }
        else
        {
            lifeTime = 16f;
        }

        posnum = Random.Range(1, 5);

        if (posnum == 1)
        {
            transform.position = new Vector3(pos1.position.x, pos1.position.y, pos1.position.z);

        }
        else if (posnum == 2)
        {
            transform.position = new Vector3(pos2.position.x, pos2.position.y, pos2.position.z);
        }
        else if (posnum == 3)
        {
            transform.position = new Vector3(pos3.position.x, pos3.position.y, pos3.position.z);
        }
        else if (posnum == 4)
        {
            transform.position = new Vector3(pos4.position.x, pos4.position.y, pos4.position.z);
        }
        tempStartLoc = transform.position;
    }

    private void OnEnable()
    {
        shouldMakeSound = true;
        hasMadeSound = false;
        soundTime = 6f;

        if (gameObject.name == "Long_Winds")
        {
            lifeTime = 23f;
        }
        else
        {
            lifeTime = 16f;
        }

        posnum = Random.Range(1, 5);

        if (posnum == 1)
        {
            transform.position = new Vector3(pos1.position.x, pos1.position.y, pos1.position.z);

        }
        else if (posnum == 2)
        {
            transform.position = new Vector3(pos2.position.x, pos2.position.y, pos2.position.z);
        }
        else if (posnum == 3)
        {
            transform.position = new Vector3(pos3.position.x, pos3.position.y, pos3.position.z);
        }
        else if (posnum == 4)
        {
            transform.position = new Vector3(pos4.position.x, pos4.position.y, pos4.position.z);
        }
        tempStartLoc = transform.position;
    }

    private void Update()
    {

        transform.position += new Vector3(speed,0,0);

        if (lifeTime <= 0)
        {
            gameObject.SetActive(false);
            shouldMakeSound = false;


            if (gameObject.name == "Long_Winds")
            {
                lifeTime = 20f;
            }
            else
            {
                lifeTime = 16f;
            }

        }
        lifeTime -= Time.deltaTime;



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
        if (col.gameObject.tag == "Minion")
        {
            Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
            if (colRigidbody != null)
            {
                colRigidbody.AddForce(WindDirection * Strength);
            }
        }
     
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Minion")
        {
            Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
            if (colRigidbody != null)
            {
                colRigidbody.AddForce(WindDirection * Strength);
            }
        }
    }
}
