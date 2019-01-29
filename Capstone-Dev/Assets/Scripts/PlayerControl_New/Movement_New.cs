using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using AssemblyCSharp;

public class Movement_New : MonoBehaviour {

    public float WalkSpeed = 3.0f;
    public bool IsFaceRight = true;
    public GameObject RightAimIcon = null;
    public GameObject LeftAimIcon = null;
    public Vector3 Recoil;

    private float yRotate;
    public bool isBulletTime;
    private Player_New player;
    private Rigidbody playerBody;
    private int aimScale = 3;
    private float aimDistance;
    private Shoot_New playerShoot;
    private Character character;


    // Use this for initialization
    void Start()
    {
        playerShoot = GetComponent<Shoot_New>();

        Recoil = Vector3.zero;

        RightAimIcon = Instantiate(RightAimIcon, transform);
        RightAimIcon.transform.position = transform.position;
        RightAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * aimScale;

        LeftAimIcon = Instantiate(LeftAimIcon, transform);
        LeftAimIcon.transform.position = transform.position;
        LeftAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * aimScale;
        LeftAimIcon.SetActive(false);

        isBulletTime = false;
        player = gameObject.GetComponent<Player_New>();
        playerBody = gameObject.GetComponent<Rigidbody>();

        player.LeftTarget = LeftAimIcon;
        player.RightTarget = RightAimIcon;

        aimDistance = 3.5f;

        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {

        RecoilReduce();

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
        if (isBulletTime && !playerShoot.CombineOn)
        {
            if (Mathf.Abs(Input.GetAxis("Left X")) <= 1 || Mathf.Abs(Input.GetAxis("Left Y")) <= 1)
            {
                LeftAimIcon.SetActive(true);
                player.LeftTarget = LeftAimIcon;
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

                    Vector3 tempvector = new Vector3(tempx, tempy, 0);
                    LeftAimIcon.transform.localPosition = tempvector * aimDistance;
                }
            }
            if (Recoil != Vector3.zero)
            {
                Vector3 rigimove = Recoil * Time.deltaTime;
                playerBody.MovePosition(transform.position + rigimove);
            }
        }

        //move
        else
        {
            LeftAimIcon.SetActive(false);
            if (Mathf.Abs(Input.GetAxis("Left X")) >= 0.1 || Mathf.Abs(Input.GetAxis("Left Y")) >= 0.1 || Recoil != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0, yRotate, 0);
                Vector3 rigimove = new Vector3(Input.GetAxis("Left X") * WalkSpeed * Time.deltaTime, Input.GetAxis("Left Y") * WalkSpeed * Time.deltaTime, 0) + Recoil * Time.deltaTime;
                playerBody.MovePosition(transform.position + rigimove);
                character.Animator.SetBool("Walk", true);
            }
            else
            {
                character.Animator.SetBool("Walk", false);
            }
            if (Input.GetAxis("Right X") > 0.3)
            {
                if (!isBulletTime)
                    IsFaceRight = true;
            }
            else if (Input.GetAxis("Right X") < -0.3)
            {
                if (!isBulletTime)
                    IsFaceRight = false;
            }
            if (IsFaceRight)
            {
                yRotate = 0;
                Quaternion rigiRotate = Quaternion.Euler(0, yRotate, 0);
                transform.rotation = rigiRotate;
            }
            else
            {
                yRotate = 180;
                Quaternion rigiRotate = Quaternion.Euler(0, yRotate, 0);
                transform.rotation = rigiRotate;
            }
        }
    }

    private void RecoilReduce()
    {
        if (Recoil.x > 0)
        {
            Recoil.x -= Time.deltaTime;
            if (Recoil.x < 0)
                Recoil.x = 0;
        }
        else if (Recoil.x < 0)
        {
            Recoil.x += Time.deltaTime;
            if (Recoil.x > 0)
                Recoil.x = 0;
        }
        if (Recoil.y > 0)
        {
            Recoil.y -= Time.deltaTime;
            if (Recoil.y < 0)
                Recoil.y = 0;
        }
        else if (Recoil.y < 0)
        {
            Recoil.y += Time.deltaTime;
            if (Recoil.y > 0)
                Recoil.y = 0;
        }
    }
}
