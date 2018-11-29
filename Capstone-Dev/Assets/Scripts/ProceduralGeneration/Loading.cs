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
        myText.text = "LOADING......"+progress;
        //Debug.Log("xuanyusong" + progress);
    }
    
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
}
