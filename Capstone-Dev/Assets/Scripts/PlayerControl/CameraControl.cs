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
        if (PlayerPos.position.x > border.x + 17 && PlayerPos.position.x < border2.x - 17)
        {
            pos.x = PlayerPos.position.x;
        }
        else if (PlayerPos.position.x < border.x + 17)
        {
            pos.x = border.x + 17;
        }
        else if (PlayerPos.position.x > border2.x - 17)
        {
            pos.x = border2.x - 17;
        }
        if (PlayerPos.position.y > border.y+9.56f && PlayerPos.position.y < border2.y-9.56f)
        {
            pos.y = PlayerPos.position.y;
        }
        else if (PlayerPos.position.y < border.y + 9.56f)
        {
            pos.y = border.y + 9.56f;
        }
        else if (PlayerPos.position.y > border2.y - 9.56f)
        {
            pos.y = border2.y - 9.56f;
        }
        transform.position = pos;
    }
}
