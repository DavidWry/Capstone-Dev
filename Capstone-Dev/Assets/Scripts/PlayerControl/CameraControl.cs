using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform PlayerPos;
    private GameObject playerObject;
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
        if (!playerObject)
            playerObject = GameObject.FindGameObjectWithTag("Player");
        if (!PlayerPos && playerObject)
            PlayerPos = playerObject.GetComponent<Transform>();
        Vector3 pos = transform.position;
        if (PlayerPos)
        {
 
            if (PlayerPos.position.x > border.x + 190 && PlayerPos.position.x < border2.x - 219)
            {
                pos.x = PlayerPos.position.x;
            }
            else if (PlayerPos.position.x < border.x + 190)
            {
                pos.x = border.x + 190;
            }
            else if (PlayerPos.position.x > border2.x - 219)
            {
                pos.x = border2.x - 219;
            }
            if (PlayerPos.position.y > border.y + 102 && PlayerPos.position.y < border2.y - 129)
            {
                pos.y = PlayerPos.position.y;
            }
            else if (PlayerPos.position.y < border.y + 102)
            {
                pos.y = border.y + 102;
            }
            else if (PlayerPos.position.y > border2.y - 129)
            {
                pos.y = border2.y - 129;
            }
        }
        transform.position = pos;
  
    }
}
