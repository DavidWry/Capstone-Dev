﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scshake : MonoBehaviour
{
    public Camera cam;
 
   public float offx, offy, time;
    
    Vector3 Posrecord;
    // Use this for initialization
    void Start()
    {
        time = 0;
        Posrecord = cam.transform.localPosition;
        cam = Camera.main;

    }

    // Update is called once per frame

    public void prepshake()
    {

        time = Random.Range(0.3f, 0.5f);// 晃多久 调这函数就晃了



    }


    void shake()
    {
        offx = Random.Range(-0.9f, 0.9f);
        offy = Random.Range(-0.9f, 0.9f);
        Vector3 vecasdf = new Vector3(offx, offy, 0);
        if (time > 0)
        {
            cam.transform.localPosition += vecasdf;
            time = time - Time.deltaTime;
        }
        if (time <= 0&&gameObject.GetComponent<CameraSlidah>().offset==false)
        {


            resume();

        }
       
    }


    void resume()
    {
        if (cam.transform.localPosition.y > -.77f)
        {
            cam.transform.localPosition -= Vector3.up * 0.01f* 1000;

        }


        if (cam.transform.localPosition.y < -.73f)
        {
            cam.transform.localPosition += Vector3.up * 0.01f * 1000;

        }

       
            if (cam.transform.localPosition.x < 2.10f)
            {
                cam.transform.localPosition += Vector3.right * 0.01f * 1000;





            }
       


   
            if (cam.transform.localPosition.x > 2.14f)
            {
                cam.transform.localPosition += Vector3.left * 0.01f * 1000;





            }
       



    }




    void Update()
    {


        shake();

    }





}
