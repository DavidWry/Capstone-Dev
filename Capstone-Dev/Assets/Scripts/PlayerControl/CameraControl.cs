using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform PlayerPos;
    private float speed;
    [SerializeField]
    private Vector2 border;    //left top
    [SerializeField]
    private Vector2 border2;   //right bottom

    // Use this for initialization
    void Start () {
        speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        if (PlayerPos.position.x > border.x && PlayerPos.position.x < border2.x)
        {
            pos.x = PlayerPos.position.x;
        }
        if (PlayerPos.position.y < border.y && PlayerPos.position.y > border2.y)
        {
            pos.y = PlayerPos.position.y;
        }
        transform.position = pos;
    }
}
