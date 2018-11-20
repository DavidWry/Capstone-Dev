﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shoot : MonoBehaviour {

    private Player player;
    private GameManager gameManager = null;

    public Lazer LeftLazer;
    public Lazer RightLazer;

    public Transform Left;             //Where the hands should be
    public Transform Right;
    public Transform Center;

    //public int LeftAmmo;                //Deal with reload
    //public int RightAmmo;
    private float LeftReloadTime;
    private float RightReloadTime;

    private bool LeftHandOn;            //Make sure there is a weapon in hand.
    private bool RightHandOn;
    public bool IsLeftShooting;
    public bool IsRightShooting;
    public bool IsCombineShooting;
    private bool CanLeftShoot;
    private bool CanRightShoot;
    private bool CanCombineShoot;
    private float LeftWaitedTime;       //How long have been waited.
    private float RightWaitedTime;
    private float CombineWaitedime;
    private float CombinedTime;         //How long has been pulled two triggers.

    [SerializeField]
    private float TimeBeforeCombine = 0.5f;    //Don't combine if player is not keeping it.
    [SerializeField]
    private float TimePrepareCombine = 1.0f;   //Preparing time.
    [SerializeField]
    private float TimeTopCombine = 5.0f;       //Top time to combine.

    //For Combine Shooting
    private float CombineBtw = 1.0f;
    bool combineAngle = false;
    //25-7
    private float CombineBtw_25 = 0.2f;
    private float CombineSpeed_25 = 15f;
    private float CombineDuration_25 = 0.5f;

	// Use this for initialization
	void Start () {

        player = gameObject.GetComponent<Player>();

        IsLeftShooting = false;
        IsRightShooting = false;
        IsCombineShooting = false;

        CanLeftShoot = true;
        CanRightShoot = true;
        CanCombineShoot = true;

        LeftWaitedTime = 0f;
        RightWaitedTime = 0f;
        CombineWaitedime = 0f;
        CombinedTime = 0f;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        LeftReloadTime = 0;
        RightReloadTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //Check inputs.
		if(Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 && player.leftWeapon.Name!="")            //Push left trigger to shoot on left.
        {
            IsLeftShooting = true;
        }
        else
        {
            IsLeftShooting = false;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && player.rightWeapon.Name != "")          //Push right trigger to shoot on right.
        {
            IsRightShooting = true;
        }
        else
        {
            IsRightShooting = false;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 && 
            player.leftWeapon.Name != "" && player.rightWeapon.Name != "")  //Push both trigger to combine.
        {
            float rx = Input.GetAxis("Left X");
            float ry = Input.GetAxis("Left Y");
            Vector3 pz = new Vector3(rx, ry, 0);
            float rtx = Input.GetAxis("Right X");
            float rty = Input.GetAxis("Right Y");
            Vector3 ptz = new Vector3(rtx, rty, 0);
            combineAngle = false;

            CombinedTime += Time.deltaTime;
            if (Mathf.Abs(Vector3.Distance(ptz, pz)) < .6f && CombinedTime > TimeBeforeCombine)
            {
                combineAngle = true;
            }
            else if (Mathf.Abs(Vector3.Distance(ptz, pz)) > 1.2f)
            {
                combineAngle = false;
                CombinedTime = 0;
            }
            else
            {
                combineAngle = false;
            }
            if (combineAngle)
            {

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

        }
        else
        {
            CombinedTime = 0;
            CombineWaitedime = 0f;
        }

        //Shoot

        LeftShoot();
        RightShoot();
        CombineShoot();

        //Deal with the cool down time and Reload.

        if (player.leftWeapon.CurrentAmmos > 0)
        {
            if (!CanLeftShoot)
            {
                LeftWaitedTime += Time.deltaTime;
                if (LeftWaitedTime >= player.leftWeapon.TimeBetweenShot)
                {
                    CanLeftShoot = true;
                    LeftWaitedTime = 0f;
                }
            }
        }
        else
        {
            LeftReloadTime += Time.deltaTime;
            if (LeftReloadTime >= player.leftWeapon.ReloadTime)
            {
                player.leftWeapon.CurrentAmmos = player.leftWeapon.AmmoSize;
                LeftReloadTime = 0;
            }
        }

        if (player.rightWeapon.CurrentAmmos > 0)
        {
            if (!CanRightShoot)
            {
                RightWaitedTime += Time.deltaTime;
                if (RightWaitedTime >= player.rightWeapon.TimeBetweenShot)
                {
                    CanRightShoot = true;
                    RightWaitedTime = 0f;
                }
            }
        }
        else
        {
            RightReloadTime += Time.deltaTime;
            if (RightReloadTime >= player.rightWeapon.ReloadTime)
            {
                player.rightWeapon.CurrentAmmos = player.rightWeapon.AmmoSize;
                RightReloadTime = 0;
            }
        }
        if (!CanCombineShoot && combineAngle)
        {
            CombineWaitedime += Time.deltaTime;
            if (CombineWaitedime >= CombineBtw)
            {
                CanCombineShoot = true;
                CombineWaitedime = 0f;
            }
        }
    }

    private void LeftShoot()
    {
        if (IsLeftShooting && CanLeftShoot)
        {
            if (player.leftWeapon.IsMultiBullets)
            {
                //create as many shot with the specific angle
                foreach (float Angle in player.leftWeapon.BulletAngleList)
                {
                    //create the projectile
                    GameObject MultiNewProj = Instantiate(player.leftWeapon.Projectile);
                    MultiNewProj.transform.position = Left.position;

                    //calculate the new angle for every shot
                    Vector3 Temp = Left.eulerAngles;
                    Temp.z += Angle;
                    MultiNewProj.transform.eulerAngles = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Speed = player.leftWeapon.ProjectileSpeed;
                    Proj.Duration = player.leftWeapon.Duration;
                    Proj.Thrust = player.leftWeapon.IsThrust;
                }
            }
            else if (player.leftWeapon.IsLazer)
            {
                if (LeftLazer == null)
                {
                    GameObject NewLazer = Instantiate(player.leftWeapon.Projectile, Left);
                    NewLazer.transform.position = Left.position;

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = player.leftWeapon.Duration;

                    LeftLazer = Laz;
                }
            }
            else
            {
                //Creat projectile
                GameObject NewProj = Instantiate(player.leftWeapon.Projectile);
                NewProj.transform.position = Left.position;
                NewProj.transform.rotation = Left.rotation;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = player.leftWeapon.ProjectileSpeed;
                Proj.Duration = player.leftWeapon.Duration;
            }
            //Deal with reload
            if (!player.leftWeapon.IsShortRange)
            {
                player.leftWeapon.CurrentAmmos--;
            }
            gameManager.leftWeaponMenu.UpdateWeaponMenu(player.leftWeapon);
            CanLeftShoot = false;
        }
        else if (!IsLeftShooting && LeftLazer != null)
        {
            LeftLazer.IsReady = false;
        }
    }

    private void RightShoot()
    {
        if (IsRightShooting && CanRightShoot)
        {

            //create the projectile if it can shoot multi-bullets
            if (player.rightWeapon.IsMultiBullets)
            {
                //create as many shot with the specific angle
                foreach (float Angle in player.rightWeapon.BulletAngleList)
                {
                    //create the projectile
                    GameObject MultiNewProj = Instantiate(player.rightWeapon.Projectile);
                    MultiNewProj.transform.position = Right.position;

                    //calculate the new angle for every shot
                    Quaternion Temp = Right.rotation;
                    Temp.z += Angle;
                    MultiNewProj.transform.rotation = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Speed = player.rightWeapon.ProjectileSpeed;
                    Proj.Duration = player.rightWeapon.Duration;
                    Proj.Thrust = player.rightWeapon.IsThrust;
                }
            }
            else if (player.rightWeapon.IsLazer)
            {
                if (RightLazer == null)
                {
                    GameObject NewLazer = Instantiate(player.rightWeapon.Projectile, Right);
                    NewLazer.transform.position = Right.position;

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = player.rightWeapon.Duration;

                    RightLazer = Laz;
                }
            }
            else if (player.rightWeapon.IsShortRange)
            {
                //Creat projectile
                GameObject NewProj = Instantiate(player.rightWeapon.Projectile);
                NewProj.transform.position = Right.position;
                NewProj.transform.rotation = Right.rotation;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = player.rightWeapon.ProjectileSpeed;
                Proj.Duration = player.rightWeapon.Duration;
                Proj.Thrust = player.rightWeapon.IsThrust;
            }
            else
            {
                //Creat projectile
                GameObject NewProj = Instantiate(player.rightWeapon.Projectile);
                NewProj.transform.position = Right.position;
                NewProj.transform.rotation = Right.rotation;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = player.rightWeapon.ProjectileSpeed;
                Proj.Duration = player.rightWeapon.Duration;
                Proj.Thrust = player.rightWeapon.IsThrust;
            }
            //Deal with reload
            if (!player.rightWeapon.IsShortRange)
            {
                player.rightWeapon.CurrentAmmos--;
            }
            gameManager.rightWeaponMenu.UpdateWeaponMenu(player.rightWeapon);
            CanRightShoot = false;
        }
        else if (!IsRightShooting && RightLazer != null)
        {
            RightLazer.IsReady = false;
        }
    }

    // 1 - Pistol 2 - Ak47 3 - Lazer 4 - Shotgun 5 - Sword

    private void CombineShoot()
    {
        if (IsCombineShooting && CanCombineShoot)
        {
            switch (player.CombineType)
            {
                case 0:
                    break;
                case 12: CombineShoot_12();
                    break;
                case 13:
                    CombineShoot_13();
                    break;
                case 14:
                    CombineShoot_14();
                    break;
                case 15:
                    CombineShoot_15();
                    break;
                case 23:
                    CombineShoot_23();
                    break;
                case 24:
                    CombineShoot_24();
                    break;
                case 25:
                    CombineShoot_25();
                    CombineBtw = CombineBtw_25;
                    break;
                case 34:
                    CombineShoot_34();
                    break;
                case 35:
                    CombineShoot_35();
                    break;
                case 45:
                    CombineShoot_45();
                    break;
            }
            CanCombineShoot = false;
        }
    }

    private void CombineShoot_12()
    {

    }
    private void CombineShoot_13()
    {

    }
    private void CombineShoot_14()
    {

    }
    private void CombineShoot_15()
    {

    }
    private void CombineShoot_23()
    {

    }
    private void CombineShoot_24()
    {

    }
    private void CombineShoot_25()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[7]);
        NewProj.transform.position = Center.position;
        NewProj.transform.rotation = Right.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_25;
        Proj.Duration = CombineDuration_25;
    }
    private void CombineShoot_34()
    {

    }
    private void CombineShoot_35()
    {

    }
    private void CombineShoot_45()
    {

    }
}
