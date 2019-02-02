using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2stab : MonoBehaviour {
    public GameObject stabSprite;
    public bool stab1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void stabStart()
    {
        stabSprite.SetActive(true);
    }
    public void stabEnd()
    {
        stabSprite.SetActive(false);
    }
    public void stabbing()
    {
        if (stab1) { stab1 = false; }
        if (!stab1) { stab1 = true; }
    }
}
