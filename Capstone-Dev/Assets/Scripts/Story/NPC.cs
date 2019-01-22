using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public TextBoxManagerXML theTextBox;
    public string NPCText = "TestXML";
    [SerializeField]
    private string NPCName = "NPC";

	// Use this for initialization
	void Start () {
        theTextBox = FindObjectOfType<TextBoxManagerXML>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown("AButton"))
            {
                if (!theTextBox.isActive)
                {
                    theTextBox.ReloadText(NPCText);
                    theTextBox.EnableTextBox();
                }
            }
        }
    }
}
