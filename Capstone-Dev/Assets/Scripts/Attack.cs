using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Attack : MonoBehaviour {


    public GameObject RightAimIcon = null;
    public List<Weapon> ListWeapons;


    private bool CanAttack = true;
    private int CurrentWeaponIndex = 0;
    private bool BulletTime = false;
    // Use this for initialization
    void Start () {
        RightAimIcon = Instantiate(RightAimIcon, transform);
        RightAimIcon.transform.position = transform.position;
        RightAimIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
        //set the aim icon
        if (Mathf.Abs(Input.GetAxis("Right X")) <= 1 || Mathf.Abs(Input.GetAxis("Right Y")) <= 1)
        {
            
            if (Mathf.Abs(Input.GetAxis("Right X")) > 0.03 || Mathf.Abs(Input.GetAxis("Right Y")) >0.03)
            {
                float tempx = Input.GetAxis("Right X");
                float tempy = Input.GetAxis("Right Y");
                Debug.Log(tempx);
                Debug.Log(tempy);
                Vector3 tempvector = new Vector3(tempx, tempy, 0);
                tempvector = tempvector.normalized;
                RightAimIcon.transform.localPosition = tempvector;
            }
            else
            {
                float tempx;
                if (gameObject.GetComponent<Movement>().IsFaceRight)
                    tempx = 1;
                else
                    tempx = -1;
                float tempy = 0;

                //Debug.Log(tempx);
                Vector3 tempvector = new Vector3(tempx, tempy, 0);
                RightAimIcon.transform.localPosition = tempvector;
            }
        }
        /*
        //reduce the waiting time for the next bullet!
        if (!CanAttack && ListWeapons[CurrentWeaponIndex].TimeWaited > 0f)
        {
            ListWeapons[CurrentWeaponIndex].TimeWaited -= Time.deltaTime;

            //check if we waited enought
            if (ListWeapons[CurrentWeaponIndex].TimeWaited <= 0f)
            {
                CanAttack = true;
                //CanRightAttack = true;
            }
        }

        //bullets time
        if (Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 && Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0)
        {
            ListWeapons[CurrentWeaponIndex].TimeBetweenShot = 0.02f;
            Time.timeScale = 0.1f;
            BulletTime = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            BulletTime = false;
            ListWeapons[CurrentWeaponIndex].TimeBetweenShot = 0.1f;
        }
        */

        //Debug.Log(RightAimIcon.transform.position);
    }
}
