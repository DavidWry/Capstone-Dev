using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashing : MonoBehaviour {
    public bool slashin=false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Slashin()
    {
         
        if (slashin)
        {
            slashin = false;
        }
        else
        {
            slashin = true;
        }

    }
}
