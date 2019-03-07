using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LeaveToScene : MonoBehaviour {

    public string nextSceneName;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void loadS()
    {
        transform.parent.gameObject.SetActive(false);
        NextScene.loadName = nextSceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
