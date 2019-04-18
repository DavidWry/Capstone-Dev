using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour {

    public float strength;
    public Vector3 windDirection;
    public float speed;
    public GameObject effect;

    private void Update()
    {
        transform.position += new Vector3(speed,0,0);
       // Instantiate(effect, transform.position, Quaternion.identity);
    }

    void OnTriggerStay(Collider col)
    {
        Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
        if (colRigidbody != null)
        {
            colRigidbody.AddForce(windDirection * strength);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Rigidbody colRigidbody = col.GetComponent<Rigidbody>();
        if (colRigidbody != null)
        {
            colRigidbody.AddForce(windDirection * strength);
        }
    }
}
