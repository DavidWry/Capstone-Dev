using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UITemp : MonoBehaviour {

    public GameObject PlayerObj;
    public float AP = 0;
    public Slider PowerBar;
    public float percentage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AP = PlayerObj.GetComponent<Player>().Power;
        percentage = AP / 100f;
        PowerBar.value = percentage;
	}
}
