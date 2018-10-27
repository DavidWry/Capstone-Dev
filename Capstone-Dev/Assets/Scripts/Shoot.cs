using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shoot : MonoBehaviour {

    public Weapon LeftWeapon;
    public Weapon RightWeapon;
    public GameObject Projectile;

    public Lazer LeftLazer;
    public Lazer RightLazer;

    private Transform Left;             //Where the hands should be
    private Transform Right;
    private Transform Center;

    public int LeftAmmo;                //Deal with reload
    public int RightAmmo;
    private float LeftReloadTime;
    private float RightReloadTime;

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

    [SerializeField]
    private float TimeBeforeCombine = 0.5f;    //Don't combine if player is not keeping it.
    [SerializeField]
    private float TimePrepareCombine = 1.0f;   //Preparing time.
    [SerializeField]
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
            if (LeftWeapon.IsMultiBullets)
            {
                //create as many shot with the specific angle
                foreach (float Angle in LeftWeapon.BulletAngleList)
                {
                    //create the projectile
                    GameObject MultiNewProj = Instantiate(LeftWeapon.Projectile);
                    MultiNewProj.transform.position = Left.position;

                    //calculate the new angle for every shot
                    Quaternion Temp = Left.rotation;
                    Temp.z += Angle;
                    MultiNewProj.transform.rotation = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Speed = LeftWeapon.ProjectileSpeed;
                    Proj.Duration = LeftWeapon.Duration;
                    Proj.Thrust = LeftWeapon.IsThrust;
                }
            }
            else if (LeftWeapon.IsLazer)
            {
                if (LeftLazer == null)
                {
                    GameObject NewLazer = Instantiate(LeftWeapon.Projectile);
                    NewLazer.transform.position = Left.position;

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = LeftWeapon.Duration;

                    LeftLazer = Laz;
                }
            }
            else
            {
                //Creat projectile
                GameObject NewProj = Instantiate(LeftWeapon.Projectile);
                NewProj.transform.position = Left.position;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = LeftWeapon.ProjectileSpeed;
                Proj.Duration = LeftWeapon.Duration;
            }
            //Deal with reload
            LeftAmmo--;
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
            if (RightWeapon.IsMultiBullets)
            {
                //create as many shot with the specific angle
                foreach (float Angle in RightWeapon.BulletAngleList)
                {
                    //create the projectile
                    GameObject MultiNewProj = Instantiate(RightWeapon.Projectile);
                    MultiNewProj.transform.position = Right.position;

                    //calculate the new angle for every shot
                    Quaternion Temp = Right.rotation;
                    Temp.z += Angle;
                    MultiNewProj.transform.rotation = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Speed = RightWeapon.ProjectileSpeed;
                    Proj.Duration = RightWeapon.Duration;
                    Proj.Thrust = RightWeapon.IsThrust;
                }
            }
            else if (RightWeapon.IsLazer)
            {
                if (RightLazer == null)
                {
                    GameObject NewLazer = Instantiate(RightWeapon.Projectile);
                    NewLazer.transform.position = Right.position;

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = RightWeapon.Duration;

                    RightLazer = Laz;
                }
            }
            else
            {
                //Creat projectile
                GameObject NewProj = Instantiate(RightWeapon.Projectile);
                NewProj.transform.position = Right.position;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = RightWeapon.ProjectileSpeed;
                Proj.Duration = RightWeapon.Duration;
                Proj.Thrust = RightWeapon.IsThrust;
            }
            //Deal with reload
            RightAmmo--;
            CanRightShoot = false;
        }
        else if (!IsRightShooting && RightLazer != null)
        {
            RightLazer.IsReady = false;
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
