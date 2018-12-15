using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public GameObject FloatNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int Damage)
    {
        GameObject FloatObj = Instantiate(FloatNum, transform.position, transform.rotation);
        FloatObj.GetComponent<FloatingNumbers>().Damage = Damage;
    }
}
