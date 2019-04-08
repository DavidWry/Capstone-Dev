using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AssemblyCSharp;

public class NPCManager : MonoBehaviour {

    public const string path = "TestRelation";
    private CharacterNodeContainer characterNodeContainer;
    private GameObject PlayerObj;
    public Player_New player;
    public List<string> possibleIds;
    public List<GameObject> allNpcs;
    public GameObject Pos;
    public GameObject CurrentPosNPC;
    public GameObject canvas;

    // Use this for initialization
    void Start () {
        Inite();
    }

    public void Inite()
    {
        characterNodeContainer = CharacterNodeContainer.Load(path);
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player_New>();
        FindAllPossibleId();
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject gameObject = GenerateNPC();
            if(gameObject != null)
            {
                Instantiate(gameObject);
            }
            else
            {
                Debug.Log("Can't find the NPC Object");
            }
        }
    }

    public void FindAllPossibleId()
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
                            bool canAdd = true;
                            foreach (string usedID in player.NPCIDs)
                            {
                                if (usedID == NewIds)
                                    canAdd = false;
                            }
                            if (canAdd)
                                possibleNodes.Add(NewIds);
                        }
                    }
                    else
                    {
                        if(NpcNode.State == "Start")
                        {
                            bool canAdd = true;
                            foreach (string usedID in player.NPCIDs)
                            {
                                if (usedID == NpcNode.ID)
                                    canAdd = false;
                            }
                            if (canAdd)
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
                    bool canAdd = true;
                    foreach (string usedID in player.NPCIDs)
                    {
                        if (usedID == NpcNode.ID)
                            canAdd = false;
                    }
                    if (canAdd)
                        possibleNodes.Add(NpcNode.ID);
                }
            }
        }
        possibleIds = possibleNodes.Distinct().ToList();
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

    public GameObject GenerateNPC()
    {
        int num = Random.Range(0, possibleIds.Count);
        string name = possibleIds[num];
        foreach (GameObject curentNPC in allNpcs)
        {
            if (curentNPC.GetComponent<NPC>().NPCText == name)
            {
                return curentNPC;
            }
        }
        return null;
    }

    public void DeletUsedID(string Id)
    {
        possibleIds.Remove(Id);
    }

    public void UINPCGen()
    {
        if (CurrentPosNPC != null)
        {
            if (Pos.transform.childCount > 0)
            {
                Destroy(Pos.transform.GetChild(0).gameObject);
            }
            GameObject newUINPC = Instantiate(CurrentPosNPC, Pos.transform);
            newUINPC.transform.position = Pos.transform.position;
            Vector3 scale = newUINPC.transform.lossyScale;
            Vector3 scales = canvas.transform.lossyScale;
            newUINPC.transform.localScale = newUINPC.transform.localScale / scale.y * scales.x;
            if (newUINPC.GetComponent<LookAtPlayer>())
            {
                newUINPC.GetComponent<LookAtPlayer>().op = false;
            }
        }
    }
}
