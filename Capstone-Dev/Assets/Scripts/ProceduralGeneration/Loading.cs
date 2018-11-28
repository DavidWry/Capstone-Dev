using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private bool isPressed;
    private float fps = 10.0f;
    private float time;


   
    public Texture2D[] animations;
    private int nowFram;
  
    AsyncOperation async;

   
    int progress = 0;

    void Start()
    {
        isPressed = false;
        SceneManager.LoadScene(NextScene.loadName, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        //StartCoroutine(LoadScene());
    }

    
    IEnumerator LoadScene()
    {

        // async = SceneManager.LoadScene(NextScene.loadName, LoadSceneMode.Additive);
        
        while (!async.isDone || !isPressed)
        {
            yield return null;
        }
   
    }

    void OnGUI()
    {
        
        DrawAnimation(animations);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //async.allowSceneActivation = true;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(NextScene.loadName));
        }
            

        
        progress = (int)(async.progress * 100);

        
        Debug.Log("xuanyusong" + progress);

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
        GUI.DrawTexture(new Rect(100, 100, 40, 60), tex[nowFram]);

        
        GUI.Label(new Rect(100, 180, 300, 60), "lOADING!!!!!" + progress);

    }
}
