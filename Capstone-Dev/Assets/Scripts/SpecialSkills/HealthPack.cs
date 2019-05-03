using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class HealthPack : MonoBehaviour {

    public bool SorL;
    public GameObject Impact;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject.GetComponent<Player_New>().HitPoint < 100)
        {
            SoundManager.PlaySound("Health_Pack_Pickup");
            if (SorL)
            {
                other.gameObject.GetComponent<Player_New>().HitPoint += 20;
                GameObject newImpact = Instantiate(Impact, other.transform);
                newImpact.transform.position = other.transform.position;
                newImpact.transform.localScale = transform.lossyScale / 15f;
                Destroy(gameObject);
            }
            else
            {
                other.gameObject.GetComponent<Player_New>().HitPoint += 10;
                GameObject newImpact = Instantiate(Impact, other.transform);
                newImpact.transform.position = other.transform.position;
                newImpact.transform.localScale = transform.lossyScale / 15f;
                Destroy(gameObject);
            }
        }
    }
}
