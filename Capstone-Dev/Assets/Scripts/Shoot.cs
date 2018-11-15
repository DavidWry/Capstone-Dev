using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shoot : MonoBehaviour {

    private Player Player;
    private GameManager gameManager = null;

    public Lazer LeftLazer;
    public Lazer RightLazer;

    public Transform Left;             //Where the hands should be
    public Transform Right;
    private Transform Center;

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
    private float CombinedTime;         //How long has been pulled two triggers.

    [SerializeField]
    private float TimeBeforeCombine = 0.5f;    //Don't combine if player is not keeping it.
    [SerializeField]
    private float TimePrepareCombine = 1.0f;   //Preparing time.
    [SerializeField]
    private float TimeTopCombine = 5.0f;       //Top time to combine.




	// Use this for initialization
	void Start () {

        Player = gameObject.GetComponent<Player>();

        IsLeftShooting = false;
        IsRightShooting = false;
        IsCombineShooting = false;

        CanLeftShoot = true;
        CanRightShoot = true;
        CanCombineShoot = true;

        LeftWaitedTime = 0f;
        RightWaitedTime = 0f;
        CombinedTime = 0f;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        LeftReloadTime = 0;
        RightReloadTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //Check inputs.
		if(Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 && Player.leftWeapon.Name!="")            //Push left trigger to shoot on left.
        {
            IsLeftShooting = true;
        }
        else
        {
            IsLeftShooting = false;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && Player.rightWeapon.Name != "")          //Push right trigger to shoot on right.
        {
            IsRightShooting = true;
        }
        else
        {
            IsRightShooting = false;
        }
        if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 && Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 && 
            Player.leftWeapon.Name != "" && Player.rightWeapon.Name != "")  //Push both trigger to combine.
        {
            float rx = Input.GetAxis("Left X");
            float ry = Input.GetAxis("Left Y");
            Vector3 pz = new Vector3(rx, ry, 0);
            float rtx = Input.GetAxis("Right X");
            float rty = Input.GetAxis("Right Y");
            Vector3 ptz = new Vector3(rtx, rty, 0);
            bool combineAngle = false;

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
        }

        //Shoot

        LeftShoot();
        RightShoot();
        CombineShoot();

        //Deal with the cool down time and Reload.

        if (Player.leftWeapon.CurrentAmmos > 0)
        {
            if (!CanLeftShoot)
            {
                LeftWaitedTime += Time.deltaTime;
                if (LeftWaitedTime >= Player.leftWeapon.TimeBetweenShot)
                {
                    CanLeftShoot = true;
                    LeftWaitedTime = 0f;
                }
            }
        }
        else
        {
            LeftReloadTime += Time.deltaTime;
            if (LeftReloadTime >= Player.leftWeapon.ReloadTime)
            {
                Player.leftWeapon.CurrentAmmos = Player.leftWeapon.AmmoSize;
                LeftReloadTime = 0;
            }
        }

        if (Player.rightWeapon.CurrentAmmos > 0)
        {
            if (!CanRightShoot)
            {
                RightWaitedTime += Time.deltaTime;
                if (RightWaitedTime >= Player.rightWeapon.TimeBetweenShot)
                {
                    CanRightShoot = true;
                    RightWaitedTime = 0f;
                }
            }
        }
        else
        {
            RightReloadTime += Time.deltaTime;
            if (RightReloadTime >= Player.rightWeapon.ReloadTime)
            {
                Player.rightWeapon.CurrentAmmos = Player.rightWeapon.AmmoSize;
                RightReloadTime = 0;
            }
        }
    }

    private void LeftShoot()
    {
        if (IsLeftShooting && CanLeftShoot)
        {
            if (Player.leftWeapon.IsMultiBullets)
            {
                //create as many shot with the specific angle
                foreach (float Angle in Player.leftWeapon.BulletAngleList)
                {
                    //create the projectile
                    GameObject MultiNewProj = Instantiate(Player.leftWeapon.Projectile);
                    MultiNewProj.transform.position = Left.position;

                    //calculate the new angle for every shot
                    Quaternion Temp = Left.rotation;
                    Temp.z += Angle;
                    MultiNewProj.transform.rotation = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Speed = Player.leftWeapon.ProjectileSpeed;
                    Proj.Duration = Player.leftWeapon.Duration;
                    Proj.Thrust = Player.leftWeapon.IsThrust;
                }
            }
            else if (Player.leftWeapon.IsLazer)
            {
                if (LeftLazer == null)
                {
                    GameObject NewLazer = Instantiate(Player.leftWeapon.Projectile, Left);
                    NewLazer.transform.position = Left.position;

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = Player.leftWeapon.Duration;

                    LeftLazer = Laz;
                }
            }
            else
            {
                //Creat projectile
                GameObject NewProj = Instantiate(Player.leftWeapon.Projectile);
                NewProj.transform.position = Left.position;
                NewProj.transform.rotation = Left.rotation;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = Player.leftWeapon.ProjectileSpeed;
                Proj.Duration = Player.leftWeapon.Duration;
            }
            //Deal with reload
            Player.leftWeapon.CurrentAmmos--;
            gameManager.leftWeaponMenu.UpdateWeaponMenu(Player.leftWeapon);
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
            if (Player.rightWeapon.IsMultiBullets)
            {
                //create as many shot with the specific angle
                foreach (float Angle in Player.rightWeapon.BulletAngleList)
                {
                    //create the projectile
                    GameObject MultiNewProj = Instantiate(Player.rightWeapon.Projectile);
                    MultiNewProj.transform.position = Right.position;

                    //calculate the new angle for every shot
                    Quaternion Temp = Right.rotation;
                    Temp.z += Angle;
                    MultiNewProj.transform.rotation = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Speed = Player.rightWeapon.ProjectileSpeed;
                    Proj.Duration = Player.rightWeapon.Duration;
                    Proj.Thrust = Player.rightWeapon.IsThrust;
                }
            }
            else if (Player.rightWeapon.IsLazer)
            {
                if (RightLazer == null)
                {
                    GameObject NewLazer = Instantiate(Player.rightWeapon.Projectile, Right);
                    NewLazer.transform.position = Right.position;

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = Player.rightWeapon.Duration;

                    RightLazer = Laz;
                }
            }
            else
            {
                //Creat projectile
                GameObject NewProj = Instantiate(Player.rightWeapon.Projectile);
                NewProj.transform.position = Right.position;
                NewProj.transform.rotation = Right.rotation;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = Player.rightWeapon.ProjectileSpeed;
                Proj.Duration = Player.rightWeapon.Duration;
                Proj.Thrust = Player.rightWeapon.IsThrust;
            }
            //Deal with reload
            Player.rightWeapon.CurrentAmmos--;
            gameManager.rightWeaponMenu.UpdateWeaponMenu(Player.rightWeapon);
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
