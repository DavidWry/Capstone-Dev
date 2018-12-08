using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireballcolide : MonoBehaviour {
    public GameObject explosion;
    private float remainingtime;
	// Use this for initialization
	void Start () {
        remainingtime = Random.Range(4,8);
		
	}
	
	// Update is called once per frame
	void Update () {
        remainingtime -= Time.deltaTime;
        if(remainingtime <= 0)
        {

            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);

        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
          
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
          
        }
    }
}
