using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeImage : MonoBehaviour {
    public GameObject cursor;
  
   public List<Sprite> spritelist;
    
	// Use this for initialization
	void Start () {
        


    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Image>().sprite = spritelist[cursor.GetComponent<ManuController>().currentButton];

    }
}
