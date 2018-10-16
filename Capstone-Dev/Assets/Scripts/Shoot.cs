using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shoot : MonoBehaviour {

    public Weapon LeftWeapon;
    public Weapon RightWeapon;
    private bool LeftHandOn;            //Make sure there is a weapon in hand.
    private bool RightHandOn;
    private bool IsLeftShooting;
    private bool IsRightShooting;
    private bool IsCombineShooting;
    private bool CanLeftShoot;
    private bool CanRightShoot;
    private bool CanCombineShoot;
    private float LeftWaitedTime;       //How long have been waited.
    private float RightWaitedTime;
    private float CombinedTime;         //How long has been pulled two triggers.

    private float TimeBeforeCombine = 0.5f;    //Don't combine if player is not keeping it.
    private float TimePrepareCombine = 1.0f;   //Preparing time.
    private float TimeTopCombine = 5.0f;       //Top time to combine.




	// Use this for initialization
	void Start () {
        IsLeftShooting = false;
        IsRightShooting = false;
        IsCombineShooting = false;

        CanLeftShoot = true;
        CanRightShoot = true;
        CanCombineShoot = true;

        LeftWaitedTime = 0f;
        RightWaitedTime = 0f;
        CombinedTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0)            //Push left trigger to shoot on left.
        {
            IsLeftShooting = true;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0)          //Push right trigger to shoot on right.
        {
            IsRightShooting = true;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0)  //Push both trigger to combine.
        {
            CombinedTime += Time.deltaTime;
            if(CombinedTime > TimeBeforeCombine && CombinedTime <= TimeTopCombine)     //Make sure it's not combining if player is not keeping the angle.
            {
                IsLeftShooting = false;
                IsRightShooting = false;
                if (CombinedTime > TimePrepareCombine)       //Take time to prepare.
                {
                    IsCombineShooting = true;
                }
            }
            else if (CombinedTime > TimeTopCombine)          //Cancel combine if the time is too much.
            {
                IsLeftShooting = true;
                IsRightShooting = true;
                IsCombineShooting = false;
            }
        }
        else
        {
            CombinedTime = 0;
        }
        LeftShoot();
        RightShoot();
        CombineShoot();

        //Deal with the cool down time.

        if (!CanLeftShoot && IsLeftShooting)
        {
            LeftWaitedTime += Time.deltaTime;
            if (LeftWaitedTime == LeftWeapon.TimeBetweenShot)
            {
                CanLeftShoot = true;
                LeftWaitedTime = 0f;
            }
        }

        if (!CanRightShoot && IsRightShooting)
        {
            RightWaitedTime += Time.deltaTime;
            if (RightWaitedTime == RightWeapon.TimeBetweenShot)
            {
                CanLeftShoot = true;
                LeftWaitedTime = 0f;
            }
        }
    }

    private void LeftShoot()
    {
        if (IsLeftShooting && CanLeftShoot)
        {

        }
    }

    private void RightShoot()
    {
        if (IsRightShooting && CanRightShoot)
        {

        }
    }

    private void CombineShoot()
    {
        if (IsCombineShooting && CanLeftShoot)
        {

        }
    }
}
