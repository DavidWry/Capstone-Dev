using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding_Ice_Effect : MonoBehaviour
{

    // Use this for initialization

    public GameObject player;
    public float strength;
    public Vector3 windDirection;
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            windDirection = (other.transform.position - transform.position).normalized;
            rb.AddForce(windDirection * strength);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            windDirection = (other.transform.position - transform.position).normalized;
            rb.AddForce(windDirection * strength);
        }
    }
}
