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
    private Player Player;
    // Use this for initialization
    void Start () {
        RightAimIcon = Instantiate(RightAimIcon, transform);
        RightAimIcon.transform.position = transform.position;
        RightAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        LeftAimIcon = Instantiate(LeftAimIcon, transform);
        LeftAimIcon.transform.position = transform.position;
        LeftAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        LeftAimIcon.SetActive(false);

        isBulletTime = false;
        Player = gameObject.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        //set the right aim icon
        if (Mathf.Abs(Input.GetAxis("Right X")) <= 1 || Mathf.Abs(Input.GetAxis("Right Y")) <= 1)
        {

            if (Mathf.Abs(Input.GetAxis("Right X")) > 0.05 || Mathf.Abs(Input.GetAxis("Right Y")) > 0.05)
            {
                float tempx;
                if (IsFaceRight)
                    tempx = Input.GetAxis("Right X");
                else
                    tempx = -Input.GetAxis("Right X");

                float tempy = Input.GetAxis("Right Y");
                //Debug.Log(tempx);
                //Debug.Log(tempy);
                Vector3 tempvector = new Vector3(tempx, tempy, 0);
                tempvector = tempvector.normalized;
                RightAimIcon.transform.localPosition = tempvector;
            }
            else
            {
                float tempx = 1;
                float tempy = 0;

                //Debug.Log(tempx);
                Vector3 tempvector = new Vector3(tempx, tempy, 0);
                RightAimIcon.transform.localPosition = tempvector;
            }
        }

        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0)
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
                    //Debug.Log(tempx);
                    //Debug.Log(tempy);
                    Vector3 tempvector = new Vector3(tempx, tempy, 0);
                    tempvector = tempvector.normalized;
                    LeftAimIcon.transform.localPosition = tempvector;
                }
                else
                {
                    float tempx = 1;
                    float tempy = 0;

                    //Debug.Log(tempx);
                    Vector3 tempvector = new Vector3(tempx, tempy, 0);
                    LeftAimIcon.transform.localPosition = tempvector;
                }
            }
        }

        //move
        else
        {
            LeftAimIcon.SetActive(false);
            if (Mathf.Abs(Input.GetAxis("Left X")) <= 1 && Mathf.Abs(Input.GetAxis("Left Y")) <= 1)
            {
                if (Input.GetAxis("Left X") > 0.03)
                {
                    IsFaceRight = true;
                    yRotate = 0;
                    transform.rotation = Quaternion.Euler(0, yRotate, 0);
                    transform.Translate(Input.GetAxis("Left X") * Time.deltaTime * WalkSpeed, Input.GetAxis("Left Y") * Time.deltaTime * WalkSpeed, 0);
                }
                else if (Input.GetAxis("Left X") < -0.03)
                {
                    IsFaceRight = false;
                    yRotate = 180;
                    transform.rotation = Quaternion.Euler(0, yRotate, 0);
                    transform.Translate(-Input.GetAxis("Left X") * Time.deltaTime * WalkSpeed, Input.GetAxis("Left Y") * Time.deltaTime * WalkSpeed, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, yRotate, 0);
                    transform.Translate(-Input.GetAxis("Left X") * Time.deltaTime * WalkSpeed, Input.GetAxis("Left Y") * Time.deltaTime * WalkSpeed, 0);
                }
            }
        }

       


    }
}
