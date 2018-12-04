using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInstructions : MonoBehaviour {

    // Use this for initialization
    public string text = "If you are surrounded by enemies on multiple sides, hold L2 and R2 together to get use your ultimate power.";
    public Rect BoxSize = new Rect(0, 0, 200, 100);
    public bool GuiOn;
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        GuiOn = true;
    }
    private void OnTriggerExit()
    {
        GuiOn = false;
    }

    private void OnGUI()
    {
        GUI.color = Color.blue;
        if (GuiOn == true)
        {
            GUI.BeginGroup(new Rect((Screen.width - BoxSize.width)/2, (Screen.height - BoxSize.height)/2, BoxSize.width, BoxSize.height));

            GUI.Label(BoxSize, text);

            GUI.EndGroup();

        }
    }
}
