using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleController : MonoBehaviour {


    public GameObject FirstObject;
    public void Switch()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObject, null);
    }
}
