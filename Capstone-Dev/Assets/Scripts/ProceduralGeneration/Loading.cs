using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private bool isPressed;
    private float fps = 10.0f;
    private float time;
    private float waitTime;
    private bool isLoading;
    private GUIStyle labelStyle;
    public Text myText;

   
    public Texture2D[] animations;
    private int nowFram;
  
    AsyncOperation async;

    public GameObject BG2_1;
    public GameObject BGFirstLevel;
    public GameObject BG2_3;
    public GameObject BGMainRoom;
    public GameObject BGWeaponRoom;
    public GameObject BG3_1;

    int progress = 0;

    void Start()
    {
       
        waitTime = 5.0f;
        isPressed = false;
        isLoading = false;

        labelStyle = new GUIStyle
        {
            fontSize = 50,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            
        };

        if (NextScene.loadName == "First Level")
        {
            BGFirstLevel.SetActive(true);
        }
        else if (NextScene.loadName == "2_1" || NextScene.loadName == "2_2")
        {
            BG2_1.SetActive(true);
        }
        else if (NextScene.loadName == "3_1" || NextScene.loadName == "3_2")
        {
            BG3_1.SetActive(true);
        }
        else if (NextScene.loadName == "2_3"|| NextScene.loadName == "3_3")
        {
            BG2_3.SetActive(true);
        }
        else if (NextScene.loadName == "MainRoom")
        {
            BGMainRoom.SetActive(true);
        }
        else if (NextScene.loadName == "WeaponRoom")
        {
            BGWeaponRoom.SetActive(true);
        }

    }

    
    IEnumerator LoadScene()
    {

        async = SceneManager.LoadSceneAsync(NextScene.loadName);
        
        while (!async.isDone || !isPressed)
        {
            yield return null;
        }
   
    }

    void OnGUI()
    {
        
        //DrawAnimation(animations);

    }

    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else if(!isLoading){
            StartCoroutine(LoadScene());
            isLoading = true;
        }
       

        /*
        if (Input.GetKeyDown(KeyCode.Space)) {
            //async.allowSceneActivation = true;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(NextScene.loadName));
        }
        */


        if (isLoading)
        {
            progress = (int)(async.progress * 100 / 100 + (5 - waitTime) / 5 * 100 / 100 * 99);
        }

        else
        {
            progress = (int)((5 - waitTime) / 5 * 100 / 100 * 99);
            
        }
        if (NextScene.loadName == "First Level") {
            myText.text = "Heading to the Tutorial Level..." + progress;
        }
        else if (NextScene.loadName == "2_1"|| NextScene.loadName == "2_2")
        {
            myText.text = "Welcome to Mercury..." + progress;
        }
        else if (NextScene.loadName == "3_1" || NextScene.loadName == "3_2")
        {
            myText.text = "Welcome to Titan..." + progress;
        }
        else if (NextScene.loadName == "2_3"|| NextScene.loadName == "3_3")
        {
            myText.text = "Get ready for the battle..." + progress;
        }
        else if (NextScene.loadName == "MainRoom")
        {
            myText.text = "Heading to the Headquarters..." + progress;
        }
        else if (NextScene.loadName == "WeaponRoom")
        {
            myText.text = "Welcome to the Training grounds..." + progress;
        }

        //Debug.Log("xuanyusong" + progress);
    }
    /*
    void DrawAnimation(Texture2D[] tex)
    {

        time += Time.deltaTime;

        if (time >= 1.0 / fps)
        {

            nowFram++;

            time = 0;

            if (nowFram >= tex.Length)
            {
                nowFram = 0;
            }
        }
        //GUI.DrawTexture(new Rect(0, 0, 1920, 1080), tex[nowFram]);

        
        GUI.Label(new Rect(700, 800, 300, 100),"<color=FFFFFFFF>" + "LOADING......" + progress+ "</color>", labelStyle);

    }
    */
}
