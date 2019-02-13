using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {
    public Image myImage;
    public Text myText;
    private float alpha = 1;
    
    // Use this for initialization
    void Start () {
       

    }
	
	// Update is called once per frame
	void Update () {

        
            Color newcolor = Color.white;
            newcolor.a = alpha;
            alpha -= 0.01f;

            myImage.color = newcolor;
            myText.color = newcolor;
        
        if (alpha < 0)
            myImage.gameObject.SetActive(false);
	}
}
