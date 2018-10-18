using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shoot : MonoBehaviour {

    public Weapon LeftWeapon;
    public Weapon RightWeapon;
    public GameObject Projectile;

    private Transform Left;             //Where the hands should be
    private Transform Right;
    private Transform Center;

    public int LeftAmmo;                //Deal with reload
    public int RightAmmo;
    public int CombineAmmo;
    private float LeftReloadTime;
    private float RightReloadTime;
    private float CombineReloadTime;

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

        Left = transform.Find("Left");
        Right = transform.Find("Right");
        Center = transform.Find("Center");

        LeftAmmo = LeftWeapon.AmmoSize;
        RightAmmo = RightWeapon.AmmoSize;
        LeftReloadTime = 0;
        RightReloadTime = 0;
        CombineReloadTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //Check inputs.
        
		if(Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0)            //Push left trigger to shoot on left.
        {
            IsLeftShooting = true;
        }
        else
        {
            IsLeftShooting = false;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0)          //Push right trigger to shoot on right.
        {
            IsRightShooting = true;
        }
        else
        {
            IsRightShooting = false;
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

        //Shoot

        LeftShoot();
        RightShoot();
        CombineShoot();

        //Deal with the cool down time and Reload.

        if (LeftAmmo > 0)
        {
            if (!CanLeftShoot)
            {
                LeftWaitedTime += Time.deltaTime;
                if (LeftWaitedTime >= LeftWeapon.TimeBetweenShot)
                {
                    CanLeftShoot = true;
                    LeftWaitedTime = 0f;
                }
            }
        }
        else
        {
            LeftReloadTime += Time.deltaTime;
            if (LeftReloadTime >= LeftWeapon.ReloadTime)
            {
                LeftAmmo = LeftWeapon.AmmoSize;
                LeftReloadTime = 0;
            }
        }

        if (RightAmmo > 0)
        {
            if (!CanRightShoot)
            {
                RightWaitedTime += Time.deltaTime;
                if (RightWaitedTime >= RightWeapon.TimeBetweenShot)
                {
                    CanRightShoot = true;
                    RightWaitedTime = 0f;
                }
            }
        }
        else
        {
            RightReloadTime += Time.deltaTime;
            if (RightReloadTime >= RightWeapon.ReloadTime)
            {
                RightAmmo = RightWeapon.AmmoSize;
                RightReloadTime = 0;
            }
        }
    }

    private void LeftShoot()
    {
        if (IsLeftShooting && CanLeftShoot)
        {
            //Creat projectile
            GameObject NewProj = Instantiate(Projectile);
            NewProj.transform.position = Left.position;
            //Change state according to the weapon
            Projectile Proj = NewProj.GetComponent<Projectile>();
            Proj.IsReady = true;
            Proj.Speed = LeftWeapon.Speed;
            Proj.Duration = LeftWeapon.Duration;
            //Deal with reload
            LeftAmmo--;
            CanLeftShoot = false;
        }
    }

    private void RightShoot()
    {
        if (IsRightShooting && CanRightShoot)
        {
            //Creat projectile
            GameObject NewProj = Instantiate(Projectile);
            NewProj.transform.position = Right.position;
            //Change state according to the weapon
            Projectile Proj = NewProj.GetComponent<Projectile>();
            Proj.IsReady = true;
            Proj.Speed = RightWeapon.Speed;
            Proj.Duration = RightWeapon.Duration;
            //Deal with reload
            RightAmmo--;
            CanRightShoot = false;
        }
    }

    private void CombineShoot()
    {
        if (IsCombineShooting && CanCombineShoot)
        {
            CanCombineShoot = false;
        }
    }
}
