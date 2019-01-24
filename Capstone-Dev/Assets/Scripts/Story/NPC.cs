using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AssemblyCSharp;

public class NPC : MonoBehaviour {

    public TextBoxManagerXML theTextBox;
    public string NPCText = "TestXML";
    [SerializeField]
    private string NPCName = "NPC";
    [SerializeField]
    private string NpcID = "02";
    [SerializeField]
    private List<string> NpcParents = new List<string>();
    public bool NonRelatedNpc = true;

    public NPCManager npcManager;

    // Use this for initialization
    void Start () {
        theTextBox = FindObjectOfType<TextBoxManagerXML>();
        /*if (NonRelatedNpc)
        {
            NPCText = NpcID;
        }*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && NpcParents.Count == 0 && !NonRelatedNpc)
        {
            npcManager = FindObjectOfType<NPCManager>();
            NpcParents = npcManager.GetParent(NpcID);
        }
        
        /*
        if (NpcParents.Count == 0)
        {
            NPCText = NpcID;
        }
        else
        {
            //Random get a parent;
        }*/
        
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
                    other.GetComponent<Player>().NPCIDs.Add(NpcID);
                    other.GetComponent<Player>().NPCIDs = other.GetComponent<Player>().NPCIDs.Distinct().ToList();
                }
            }
        }
    }
}
