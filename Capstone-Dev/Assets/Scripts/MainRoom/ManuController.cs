using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ManuController : MonoBehaviour {

    public List<Button> buttons;
    public ScrollRect scrollRect;
    public Transform content;
    public int currentButton = 0;
    int top;
    float timer = 0;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < content.childCount; i++)
        {
            buttons.Add(content.GetChild(i).gameObject.GetComponent<Button>());
        }
        top = content.childCount;
    }
	
	// Update is called once per frame
	void Update () {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(buttons[currentButton].gameObject);
        timer += Time.deltaTime;
        if (timer > 0.3f)
        {
            InputCheck();
            timer = 0;
        }
    }

    void InputCheck()
    {
        if (Input.GetAxis("Left Y") < -0.2)
        {
            if (currentButton < top - 1)
                currentButton ++;
            scrollRect.verticalNormalizedPosition -= 0.05f;
        }
        else if (Input.GetAxis("Left Y") > 0.2)
        {
            if (currentButton > 0)
                currentButton --;
            scrollRect.verticalNormalizedPosition += 0.05f;
        }
    }
}
