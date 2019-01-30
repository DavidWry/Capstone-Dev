using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AssemblyCSharp;

public class NPCManager : MonoBehaviour {

    public const string path = "TestRelation";
    private CharacterNodeContainer characterNodeContainer;
    private GameObject PlayerObj;
    private Player_New player;
    public List<string> possibleIds;

    // Use this for initialization
    void Start () {
        characterNodeContainer = CharacterNodeContainer.Load(path);
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player_New>();
        possibleIds = FindAllPossibleId();
    }
	
	// Update is called once per frame
	void Update () {

    }

    private List<string> FindAllPossibleId()
    {
        List<string> possibleNodes = new List<string>();
        if (player.NPCIDs.Count != 0)
        {
            foreach(CharacterNode NpcNode in characterNodeContainer.CharacterNodes)
            {
                foreach(string NpcID in player.NPCIDs)
                {
                    if (NpcID == NpcNode.ID && NpcNode.State != "End")
                    {
                        foreach (string NewIds in NpcNode.Childs)
                        {
                            possibleNodes.Add(NewIds);
                        }
                    }
                    else
                    {
                        if(NpcNode.State == "Start")
                        {
                            possibleNodes.Add(NpcNode.ID);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (CharacterNode NpcNode in characterNodeContainer.CharacterNodes)
            {
                if (NpcNode.State == "Start")
                {
                    possibleNodes.Add(NpcNode.ID);
                }
            }
        }
        List<string> allNodes = possibleNodes.Distinct().ToList();
        return allNodes;
    }

    public List<string> GetParent(string ID)
    {
        List<string> result = new List<string>();
        CharacterNode target = new CharacterNode();
        foreach (CharacterNode NpcNode in characterNodeContainer.CharacterNodes)
        {
            if (NpcNode.ID == ID)
            {
                target = NpcNode;
                break;
            }
        }
        if (target.ID != null)
        {
            if (target.Parents.Length != 0)
            {
                foreach (string parent in target.Parents)
                {
                    foreach (string NpcID in player.NPCIDs)
                    {
                        if (parent == NpcID)
                        {
                            result.Add(parent);
                        }
                    }
                }
            }
            else
            {
                result.Add("00");
            }
        }
        return result;
    }
}
