using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UITemp02 : MonoBehaviour {

    public GameObject PlayerObj;
    Image image;
    float Hp = 100;

	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
        PlayerObj = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (!PlayerObj)
            PlayerObj = GameObject.FindGameObjectWithTag("Player");
        else
            Hp = PlayerObj.GetComponent<Player>().HitPoint;
        image.fillAmount = Hp / 100;
	}
}
