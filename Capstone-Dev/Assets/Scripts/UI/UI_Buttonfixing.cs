﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Buttonfixing : MonoBehaviour {
    public List<Button> buttons;
    public Transform content;
    int currentButton = 0;
    int top;
    float timer = 0;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            buttons.Add(content.GetChild(i).gameObject.GetComponent<Button>());
        }
        top = content.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[currentButton].gameObject);
        timer += Time.deltaTime;
        if (timer > 0.3f)
        {
            InputCheck();
        }
    }

    void InputCheck()
    {
        if (Input.GetAxis("Left X") > 0.5)
        {
            if (currentButton < top - 1)
                currentButton++;
            timer = 0;
        }
        else if (Input.GetAxis("Left X") < -0.5)
        {
            if (currentButton > 0)
                currentButton--;
            timer = 0;
        }
    }

}
