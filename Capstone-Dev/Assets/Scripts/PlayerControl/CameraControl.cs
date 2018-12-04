using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform PlayerPos;
    private float speed;
    [SerializeField]
    public Vector2 border;    //left bottom
    [SerializeField]
    public Vector2 border2;   //right top

    // Use this for initialization
    void Start () {
        speed = 1;
        
    }
	
	// Update is called once per frame
	void Update () {
        if(!PlayerPos)
            PlayerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector3 pos = transform.position;
        if (PlayerPos.position.x > border.x + 15f && PlayerPos.position.x < border2.x - 15f)
        {
            pos.x = PlayerPos.position.x;
        }
        else if (PlayerPos.position.x < border.x + 15f)
        {
            pos.x = border.x + 15f;
        }
        else if (PlayerPos.position.x > border2.x - 15f)
        {
            pos.x = border2.x - 15f;
        }
        if (PlayerPos.position.y > border.y+5 && PlayerPos.position.y < border2.y-5)
        {
            pos.y = PlayerPos.position.y;
        }
        else if (PlayerPos.position.y < border.y + 5)
        {
            pos.y = border.y + 5;
        }
        else if (PlayerPos.position.y > border2.y - 5)
        {
            pos.y = border2.y - 5;
        }
        transform.position = pos;
    }
}
