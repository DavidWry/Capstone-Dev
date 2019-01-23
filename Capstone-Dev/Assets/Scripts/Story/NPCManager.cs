using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public const string path = "TestRelation";
    private CharacterNodeContainer characterNodeContainer;

    // Use this for initialization
    void Start () {
        characterNodeContainer = CharacterNodeContainer.Load(path);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(characterNodeContainer.CharacterNodes[0].Childs);
	}
}
