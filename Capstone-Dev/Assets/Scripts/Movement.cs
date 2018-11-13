using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Movement : MonoBehaviour {

    public float WalkSpeed = 3.0f;
    public bool IsFaceRight = true;
    public GameObject RightAimIcon = null;
    public GameObject LeftAimIcon = null;

    private float yRotate;
    private bool isBulletTime;
    private Player player;
    private Rigidbody playerBody;
    private int aimScale = 3;
    private int aimDistance;
    //private Transform Sprite;

    // Use this for initialization
    void Start () {
        
        RightAimIcon = Instantiate(RightAimIcon, transform);
        RightAimIcon.transform.position = transform.position;
        RightAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * aimScale; 

        LeftAimIcon = Instantiate(LeftAimIcon, transform);
        LeftAimIcon.transform.position = transform.position;
        LeftAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * aimScale;
        LeftAimIcon.SetActive(false);

        isBulletTime = false;
        player = gameObject.GetComponent<Player>();
        playerBody = gameObject.GetComponent<Rigidbody>();

        player.LeftTarget = LeftAimIcon;
        player.RightTarget = RightAimIcon;

        aimDistance = 3;
        //Sprite = GameObject.Find("Sprite").transform;
    }
	
	// Update is called once per frame
	void Update () {

        //set the right aim icon
        if (Mathf.Abs(Input.GetAxis("Right X")) <= 1 || Mathf.Abs(Input.GetAxis("Right Y")) <= 1)
        {

            if (Mathf.Abs(Input.GetAxis("Right X")) > 0.3 || Mathf.Abs(Input.GetAxis("Right Y")) > 0.3)
            {
                //aimming can also change the direction of player
                //if (Input.GetAxis("Right X") > 0.05 && !isBulletTime)
                //{
                //    IsFaceRight = true;
                //}
                //else if (Input.GetAxis("Right X") < -0.05 && !isBulletTime) {
                //    IsFaceRight = false;
                //}
                float tempx;
                if (IsFaceRight)
                    tempx = Input.GetAxis("Right X");
                else
                    tempx = -Input.GetAxis("Right X");

                float tempy = Input.GetAxis("Right Y");
                Vector3 tempvector = new Vector3(tempx, tempy, 0);
                tempvector = tempvector.normalized;
                RightAimIcon.transform.localPosition = tempvector * aimDistance;

                
            }
            else
            {
                float tempx = 1;
                float tempy = 0;

                //Debug.Log(tempx);
                Vector3 tempvector = new Vector3(tempx, tempy, 0);
                RightAimIcon.transform.localPosition = tempvector * aimDistance;
            }
            
            //rotate right hand
            
            /*
            Transform rightHand = player.RightHand;
            if (rightHand.childCount > 0)
            {
                
                Vector3 targetPosition = RightAimIcon.transform.position;
                //targetPosition.x = -targetPosition.x;
                //targetPosition.y = -targetPosition.y;
                // Debug.Log(targetPosition.x);
                Vector3 selfPosition = rightHand.position;
                Quaternion newRotation;
                if (Input.GetAxis("Right X") < 0)
                {
                    newRotation = Quaternion.LookRotation(targetPosition - selfPosition, -Vector3.up);
                }
                else
                {
                    newRotation = Quaternion.LookRotation(targetPosition - selfPosition, Vector3.up);
                }
                newRotation.x = 0;
                newRotation.y = 0;
                rightHand.rotation = newRotation;              
            }*/
        

        }

        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 &&
            player.leftWeapon.Name != "" && player.rightWeapon.Name != "")
        {
            isBulletTime = true;
        }
        else
        {
            isBulletTime = false;
        }

        //show left aimIcon
        if (isBulletTime)
        {
            if (Mathf.Abs(Input.GetAxis("Left X")) <= 1 || Mathf.Abs(Input.GetAxis("Left Y")) <= 1)
            {
                LeftAimIcon.SetActive(true);
                if (Mathf.Abs(Input.GetAxis("Left X")) > 0.05 || Mathf.Abs(Input.GetAxis("Left Y")) > 0.05)
                {
                    float tempx;
                    if (IsFaceRight)
                        tempx = Input.GetAxis("Left X");
                    else
                        tempx = -Input.GetAxis("Left X");

                    float tempy = Input.GetAxis("Left Y");
                    Vector3 tempvector = new Vector3(tempx, tempy, 0);
                    tempvector = tempvector.normalized;
                    LeftAimIcon.transform.localPosition = tempvector * aimDistance;
                }
                else
                {
                    float tempx = 1;
                    float tempy = 0;

                    //Debug.Log(tempx);
                    Vector3 tempvector = new Vector3(tempx, tempy, 0);
                    LeftAimIcon.transform.localPosition = tempvector * aimDistance;
                }

                //rotate left hand
                Transform leftHand = player.LeftHand;
                if (leftHand.childCount > 0)
                {

                    Vector3 targetPosition = LeftAimIcon.transform.localPosition;

                    // Debug.Log(targetPosition.x);
                    Vector3 selfPosition = leftHand.localPosition;
                    Quaternion newRotation;

                    if (Input.GetAxis("Right X") < 0)
                    {
                        newRotation = Quaternion.LookRotation(targetPosition - selfPosition, -Vector3.up);
                    }
                    else
                    {
                        newRotation = Quaternion.LookRotation(targetPosition - selfPosition, Vector3.up);
                    }

                    newRotation.x = 0;
                    newRotation.y = 0;
                    leftHand.localRotation = newRotation;

                }
            }
        }

        //move
        else
        {
            LeftAimIcon.SetActive(false);
            if (Mathf.Abs(Input.GetAxis("Left X")) >= 0.1 || Mathf.Abs(Input.GetAxis("Left Y")) >= 0.1)
            {
                if (Input.GetAxis("Left X") > 0.1)
                {
                    IsFaceRight = true;
                    
                }
                else if (Input.GetAxis("Left X") < 0.1)
                {
                    IsFaceRight = false;                
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, yRotate, 0);
                    Vector3 rigimove = new Vector3 (Input.GetAxis("Left X") * WalkSpeed * Time.deltaTime, Input.GetAxis("Left Y") * WalkSpeed * Time.deltaTime, 0);
                    playerBody.MovePosition(transform.position + rigimove);
                }

                if (IsFaceRight)
                {
                    yRotate = 0;
                    Quaternion rigiRotate = Quaternion.Euler(0, yRotate, 0);
                    Vector3 rigimove = new Vector3 (Input.GetAxis("Left X") * WalkSpeed * Time.deltaTime, Input.GetAxis("Left Y") * WalkSpeed * Time.deltaTime, 0);
                    playerBody.MovePosition(transform.position + rigimove);
                    transform.rotation = rigiRotate;
                }
                else {
                    yRotate = 180;
                    Quaternion rigiRotate = Quaternion.Euler(0, yRotate, 0);
                    Vector3 rigimove = new Vector3(Input.GetAxis("Left X") * WalkSpeed * Time.deltaTime, Input.GetAxis("Left Y") * WalkSpeed * Time.deltaTime, 0);
                    playerBody.MovePosition(transform.position + rigimove);
                    transform.rotation = rigiRotate;
                }
            }
        }
    }
}
