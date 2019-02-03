using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Shoot_New : MonoBehaviour
{

    private Player_New player;
    private GameManager gameManager = null;
    private Movement_New movement;
    public float BulletSizeUp = 1;

    public Lazer LeftLazer;
    public Lazer RightLazer;

    public Transform Left;             //Where the hands should be
    public Transform Right;
    public Transform Center;

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
    [SerializeField]
    private float CombinedTime;         //How long has been pulled two triggers.

    [SerializeField]
    private float TimeBeforeCombine = 0.5f;    //Don't combine if player is not keeping it.
    [SerializeField]
    private float TimePrepareCombine = 1.0f;   //Preparing time.
    [SerializeField]
    private float TimeTopCombine = 6.0f;       //Top time to combine.

    //For Combine Shooting
    public bool CombineTag = false;            //Two Kinds of "combine". True means real "Combine", false means actrually "seperate".
    public bool SkillReady = false;
    public bool GoldenFinger = false;
    private float CombineBtw = 1.0f;
    bool combineAngle = false;
    bool rotateAngle = false;
    public int CombineAmmos = 0;
    public bool CombineOn = false;
    public Transform CombineBulPos;
    public int currentAmmo = 0;
    //12-1
    private float CombineBtw_12 = 1f;
    private float CombineSpeed_12 = 10f;
    private float CombineDuration_12 = 2f;
    private int CombineDamage_12 = 50;
    //13-2
    private float CombineBtw_13 = 0.8f;
    private float CombineSpeed_13 = 20f;
    private float CombineDuration_13 = 5f;
    private int CombineDamage_13 = 50;
    //14-3
    private float CombineBtw_14 = 0.2f;
    private float CombineSpeed_14 = 0f;
    private float CombineDuration_14 = 15f;
    private int CombineDamage_14 = 50;
    //15-4
    private float CombineBtw_15 = 0.2f;
    private float CombineSpeed_15 = 8f;
    private float CombineDuration_15 = 0.8f;
    private int CombineDamage_15 = 5;
    //23-5
    private float CombineBtw_23 = 0.2f;
    //24-6
    private float CombineBtw_24 = 1f;
    private float CombineSpeed_24 = 10f;
    private float CombineDuration_24 = 2f;
    private int CombineDamage_24 = 20;
    //25-7
    private float CombineBtw_25 = 0.15f;
    private float CombineSpeed_25 = 15f;
    private float CombineDuration_25 = 0.5f;
    private int CombineDamage_25 = 5;
    //34-8
    private float CombineBtw_34 = 1.5f;
    private float CombineSpeed_34 = 0f;
    private float CombineDuration_34 = 5f;
    private int CombineDamage_34 = 10;
    //35-9
    private float CombineBtw_35 = 1.5f;
    private float CombineSpeed_35 = 5f;
    private float CombineDuration_35 = 2f;
    private int CombineDamage_35 = 10;
    //45-10
    private float CombineBtw_45 = 10f;
    private float CombineSpeed_45 = 1f;
    private float CombineDuration_45 = 10f;
    private int CombineDamage_45 = 5;

    //Camera zoom;
    public GameObject CameraObj;
    private bool zoomOn;
    private float preZoom;
    private int zoomSpeed = 5;

    //New combine Type
    public GameObject LeftWeaponObj;
    public GameObject RightWeaponObj;

    // Use this for initialization
    void Start()
    {
        CameraObj = GameObject.Find("Main Camera");
        preZoom = CameraObj.GetComponent<Camera>().fieldOfView;

        player = gameObject.GetComponent<Player_New>();
        movement = gameObject.GetComponent<Movement_New>();

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

        player.leftWeapon = gameManager.WeaponList[0];
    }

    // Update is called once per frame
    void Update()
    {
        //Check inputs.
        if (Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 && player.leftWeapon.Name != "")            //Push left trigger to shoot on left.
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
        if (player.leftWeapon.Name != "" && player.rightWeapon.Name != "" && (SkillReady || GoldenFinger))  //Hold "Y" to combine.
        {
            if (Input.GetButton("YButton") && (SkillReady || GoldenFinger) && !CombineOn)
            {
                CombinedTime += Time.deltaTime;
                if (CombinedTime > TimeBeforeCombine)
                {
                    CombineOn = true;
                    CombinedTime = 0;
                    SkillReady = false;
                    currentAmmo = CombineAmmos;
                }
            }
            else
            {
                if (CombinedTime > 0)
                {
                    CombinedTime -= Time.deltaTime;
                    if (CombinedTime < 0)
                        CombinedTime = 0;
                }
            }
        }
        if (CombineOn)
        {
            if (CombineTag)
            {
                if (LeftWeaponObj != null && RightWeaponObj != null)
                {
                    LeftWeaponObj.SetActive(false);
                    RightWeaponObj.SetActive(false);
                    string weaponName = FindWeaponName();
                    player.ChangeWeapon(weaponName);
                }
                if (Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0 || Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0)
                {
                    IsCombineShooting = true;
                }
                else
                {
                    IsCombineShooting = false;
                }
                IsLeftShooting = false;
                IsRightShooting = false;
                if (player.CombineType == 13)
                {
                    zoomOn = true;
                }
            }
            else
            {
                player.ChangeBackPack();
                if (Input.GetButtonDown("XButton"))
                {
                    IsCombineShooting = true;
                }
                else
                {
                    IsCombineShooting = false;
                }
            }

            if (Input.GetButtonDown("YButton"))
            {
                CombineOn = false;
                CombinedTime = 0;
            }
            if (currentAmmo == 0)
            {
                CombineOn = false;
            }
        }
        else
        {
            CombineWaitedime = 0f;
            IsCombineShooting = false;
            zoomOn = false;
            if (player.Power < 100 && SkillReady)
            {
                SkillReady = false;
            }
            if (LeftWeaponObj != null && RightWeaponObj != null)
            {
                LeftWeaponObj.SetActive(true);
                RightWeaponObj.SetActive(true);
                player.EmptyWeapon();
                player.EmptyBackPack();
            }
        }

    }

    private void LateUpdate()
    {        
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
        if (!CanCombineShoot)
        {
            CombineWaitedime += Time.deltaTime;
            if (CombineWaitedime >= CombineBtw)
            {
                CanCombineShoot = true;
                CombineWaitedime = 0f;
            }
        }

        //Camera Zoom
        CameraZoom();

        //get weapon object
        getWeapon();
    }

    private void CameraZoom()
    {
        if (zoomOn)
        {
            if (CameraObj.GetComponent<Camera>().fieldOfView < preZoom + 15f)
            {
                CameraObj.GetComponent<Camera>().fieldOfView += zoomSpeed * Time.deltaTime;
            }
        }
        else if (CameraObj.GetComponent<Camera>().fieldOfView > preZoom)
        {
            CameraObj.GetComponent<Camera>().fieldOfView -= zoomSpeed * Time.deltaTime;
        }
    }

    private void getWeapon()
    {
        if (Left.childCount != 0)
        {
            LeftWeaponObj = Left.GetChild(0).gameObject;
        }
        if (Right.childCount != 0)
        {
            RightWeaponObj = Right.GetChild(0).gameObject;
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
                    MultiNewProj.transform.position = Left.GetChild(0).GetChild(0).position;
                    MultiNewProj.transform.localScale = BulletSizeUp * MultiNewProj.transform.localScale;

                    //calculate the new angle for every shot
                    Vector3 Temp = Left.eulerAngles;
                    Temp.z += Angle;
                    MultiNewProj.transform.eulerAngles = Temp + new Vector3(0, 0, -player.fixLeftAngle);
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Damage = player.leftWeapon.Damage;
                    Proj.Speed = player.leftWeapon.ProjectileSpeed;
                    Proj.Duration = player.leftWeapon.Duration;
                    Proj.Thrust = player.leftWeapon.IsThrust;
                }
                GameObject Shot = Instantiate(player.leftWeapon.ShotFX, Left.GetChild(0).GetChild(0));
                Shot.transform.position = Left.GetChild(0).GetChild(0).position;
                //movement.Recoil = -Left.transform.right * 0.2f;
            }
            else if (player.leftWeapon.IsLazer)
            {
                if (LeftLazer == null)
                {
                    GameObject NewLazer = Instantiate(player.leftWeapon.Projectile, Left);
                    NewLazer.transform.position = Left.GetChild(0).GetChild(0).position;
                    NewLazer.transform.eulerAngles = Left.eulerAngles + new Vector3(0, 0, -player.fixLeftAngle);

                    Lazer Laz = NewLazer.GetComponent<Lazer>();
                    Laz.IsReady = true;
                    Laz.LazeDuration = player.leftWeapon.Duration;

                    LeftLazer = Laz;
                }
            }
            else
            {
                //Creat Impact
                GameObject Shot = Instantiate(player.leftWeapon.ShotFX, Left.GetChild(0).GetChild(0));
                Shot.transform.position = Left.GetChild(0).GetChild(0).position;
                //Creat projectile
                GameObject NewProj = Instantiate(player.leftWeapon.Projectile);
                NewProj.transform.position = Left.GetChild(0).GetChild(0).position;
                NewProj.transform.eulerAngles = Left.eulerAngles + new Vector3(0, 0, -player.fixLeftAngle);
                NewProj.transform.localScale = BulletSizeUp * NewProj.transform.localScale;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Damage = player.leftWeapon.Damage;
                Proj.Speed = player.leftWeapon.ProjectileSpeed;
                Proj.Duration = player.leftWeapon.Duration;
                Proj.Thrust = player.leftWeapon.IsThrust;
            }
            //Deal with reload
            if (!player.leftWeapon.IsShortRange)
            {
                player.leftWeapon.CurrentAmmos--;
            }
            gameManager.leftWeaponMenu.UpdateWeaponMenu(player.leftWeapon);
            CanLeftShoot = false;
            var offset = -0.1f * player.LeftHand.parent.InverseTransformDirection(player.Character.Firearm.FireTransform.right);
            StartCoroutine(AnimateOffset(player.LeftHand, offset, player.LeftHand.localPosition, spring: true));
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
                    MultiNewProj.transform.position = Right.GetChild(0).GetChild(0).position;
                    MultiNewProj.transform.localScale = BulletSizeUp * MultiNewProj.transform.localScale;

                    //calculate the new angle for every shot
                    Vector3 Temp = Right.eulerAngles + new Vector3(0, 0, -player.fixRightAngle);
                    Temp.z += Angle;
                    MultiNewProj.transform.eulerAngles = Temp;
                    //give state
                    Projectile Proj = MultiNewProj.GetComponent<Projectile>();
                    Proj.IsReady = true;
                    Proj.Damage = player.rightWeapon.Damage;
                    Proj.Speed = player.rightWeapon.ProjectileSpeed;
                    Proj.Duration = player.rightWeapon.Duration;
                    Proj.Thrust = player.rightWeapon.IsThrust;
                }
                //movement.Recoil = - Right.transform.right * 0.2f;
                GameObject Shot = Instantiate(player.rightWeapon.ShotFX, Right.GetChild(0).GetChild(0));
                Shot.transform.position = Right.GetChild(0).GetChild(0).position;
            }
            else if (player.rightWeapon.IsLazer)
            {
                if (RightLazer == null)
                {
                    GameObject NewLazer = Instantiate(player.rightWeapon.Projectile, Right);
                    NewLazer.transform.position = Right.GetChild(0).GetChild(0).position;
                    NewLazer.transform.eulerAngles = Right.eulerAngles + new Vector3(0, 0, -player.fixRightAngle);

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
                NewProj.transform.position = Right.GetChild(0).GetChild(0).position;
                NewProj.transform.eulerAngles = Right.eulerAngles + new Vector3(0, 0, -player.fixRightAngle);
                NewProj.transform.localScale = BulletSizeUp * NewProj.transform.localScale;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Damage = player.rightWeapon.Damage;
                Proj.Speed = player.rightWeapon.ProjectileSpeed;
                Proj.Duration = player.rightWeapon.Duration;
                Proj.Thrust = player.rightWeapon.IsThrust;
            }
            else
            {
                //Creat impact
                GameObject Shot = Instantiate(player.rightWeapon.ShotFX, Right.GetChild(0).GetChild(0));
                Shot.transform.position = Right.GetChild(0).GetChild(0).position;
                //Creat projectile
                GameObject NewProj = Instantiate(player.rightWeapon.Projectile);
                NewProj.transform.position = Right.GetChild(0).GetChild(0).position;
                NewProj.transform.eulerAngles = Right.eulerAngles + new Vector3(0, 0, -player.fixRightAngle);
                NewProj.transform.localScale = BulletSizeUp * NewProj.transform.localScale;
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Damage = player.rightWeapon.Damage;
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
            var offset = -0.1f * player.RightHand.parent.InverseTransformDirection(-Right.up);
            StartCoroutine(AnimateOffset(player.RightHand, offset, player.RightHand.localPosition, spring: true));
        }
        else if (!IsRightShooting && RightLazer != null)
        {
            RightLazer.IsReady = false;
        }
    }

    // 1 - Pistol 2 - Ak47 3 - Lazer 4 - Shotgun 5 - Sword
    private string FindWeaponName()
    {
        switch (player.CombineType)
        {
            case 0:
                return null;
            case 12:
                return "RL-800";
            case 13:
                return "SR-MDK";
            case 14:
                return null;
            case 15:
                return "RRII";
            case 23:
                return "LR-100";
            case 24:
                return "OblivionPlasmaGun";
            case 25:
                return "LR-500";
            case 34:
                return null;
            case 35:
                return "RL-DIY200";
            case 45:
                return null;
        }
        return null;
    }

    private void CombineShoot()
    {
        if (IsCombineShooting && CanCombineShoot)
        {
            player.Power = 0;
            switch (player.CombineType)
            {
                case 0:
                    break;
                case 12:
                    CombineShoot_12();
                    CombineBtw = CombineBtw_12;
                    break;
                case 13:
                    CombineShoot_13();
                    CombineBtw = CombineBtw_13;
                    break;
                case 14:
                    CombineShoot_14();
                    CombineBtw = CombineBtw_14;
                    break;
                case 15:
                    CombineShoot_15();
                    CombineBtw = CombineBtw_14;
                    break;
                case 23:
                    CombineShoot_23();
                    CombineBtw = CombineBtw_23;
                    break;
                case 24:
                    CombineShoot_24();
                    CombineBtw = CombineBtw_24;
                    break;
                case 25:
                    CombineShoot_25();
                    CombineBtw = CombineBtw_25;
                    break;
                case 34:
                    CombineShoot_34();
                    CombineBtw = CombineBtw_34;
                    break;
                case 35:
                    CombineShoot_35();
                    CombineBtw = CombineBtw_35;
                    break;
                case 45:
                    CombineShoot_45();
                    CombineBtw = CombineBtw_45;
                    break;
            }
            CanCombineShoot = false;
            currentAmmo--;
            var offset = -0.25f * player.LeftHand.parent.InverseTransformDirection(player.Character.Firearm.FireTransform.right);
            StartCoroutine(AnimateOffset(player.LeftHand, offset, player.LeftHand.localPosition, spring: true, duration: 0.1f));
        }
    }

    private void CombineShoot_12()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[1]);
        NewProj.transform.position = CombineBulPos.position;
        NewProj.transform.rotation = CombineBulPos.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_12;
        Proj.Duration = CombineDuration_12;
        Proj.Damage = CombineDamage_12;
    }
    private void CombineShoot_13()
    {
        zoomOn = true;
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[2]);
        NewProj.transform.position = CombineBulPos.position;
        NewProj.transform.rotation = CombineBulPos.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_13;
        Proj.Duration = CombineDuration_13;
        Proj.Damage = CombineDamage_13;
    }
    private void CombineShoot_14()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[3]);
        NewProj.transform.position = Center.position;
        NewProj.transform.eulerAngles = new Vector3(0, 0, - 90);
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_14;
        Proj.Duration = CombineDuration_14;
        Proj.Damage = CombineDamage_14;
        Proj.Boom = true;
    }
    private void CombineShoot_15()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[4]);
        NewProj.transform.position = Center.position;
        NewProj.transform.rotation = Right.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_15;
        Proj.Duration = CombineDuration_15;
        Proj.Damage = CombineDamage_15;

        GameObject NewProj02 = Instantiate(gameManager.CombineProjectile[4]);
        NewProj02.transform.position = Center.position;
        NewProj02.transform.rotation = Left.rotation;
        //Change state according to the weapon
        Projectile Proj02 = NewProj02.GetComponent<Projectile>();
        Proj02.IsReady = true;
        Proj02.Speed = CombineSpeed_15;
        Proj02.Duration = CombineDuration_15;
        Proj02.Damage = CombineDamage_15;
        movement.Recoil = -(Right.transform.right);
    }
    private void CombineShoot_23()
    {
        if (movement.Recoil == Vector3.zero)
        {
            sheildUp();
        }
        movement.Recoil = Left.transform.right * 3;
    }
    private void CombineShoot_24()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[6]);
        NewProj.transform.position = CombineBulPos.position;
        NewProj.transform.rotation = CombineBulPos.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_24;
        Proj.Duration = CombineDuration_24;
        Proj.Damage = CombineDamage_24;
    }
    private void CombineShoot_25()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[7]);
        NewProj.transform.position = CombineBulPos.position;
        NewProj.transform.rotation = CombineBulPos.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_25;
        Proj.Duration = CombineDuration_25;
        Proj.Damage = CombineDamage_25;
    }
    private void CombineShoot_34()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[8]);
        NewProj.transform.position = Center.position;
        NewProj.transform.rotation = Right.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_34;
        Proj.Duration = CombineDuration_34;
        Proj.Damage = CombineDamage_34;
        Proj.Scale = true;
        Proj.OnTarget = true;
        Proj.Pierce = true;
    }
    private void CombineShoot_35()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[9]);
        NewProj.transform.position = CombineBulPos.position;
        NewProj.transform.rotation = CombineBulPos.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_35;
        Proj.Duration = CombineDuration_35;
        Proj.Damage = CombineDamage_35;
        Proj.Sheild = true;
        Proj.Scale = true;
    }
    private void CombineShoot_45()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[10]);
        NewProj.transform.position = Center.position;
        NewProj.transform.rotation = CombineBulPos.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = CombineSpeed_45;
        Proj.Duration = CombineDuration_45;
        Proj.Damage = CombineDamage_45;
        Proj.Sheild = true;
    }
    private void sheildUp()
    {
        GameObject NewProj = Instantiate(gameManager.CombineProjectile[0], transform);
        NewProj.transform.position = Center.position;
        NewProj.transform.rotation = Right.rotation;
        //Change state according to the weapon
        Projectile Proj = NewProj.GetComponent<Projectile>();
        Proj.IsReady = true;
        Proj.Speed = 0;
        Proj.Duration = 5f;
        Proj.Damage = 1000;
        Proj.Sheild = true;
    }

    private void useSkill()
    {
        player.Power = 0;
        SkillReady = false;
    }

    private static IEnumerator AnimateOffset(Transform target, Vector3 offset, Vector3 origin, bool spring = false, float duration = 0.05f)
    {
        var state = 0f;
        var startTime = Time.time;

        while (state < 1)
        {
            state = (Time.time - startTime) / duration;

            if (state <= 1)
            {
                target.localPosition = origin + offset * (spring ? Mathf.Sin(state * Mathf.PI) : state);
                yield return null;
            }
            else
            {
                target.localPosition = spring ? origin : origin + offset;
                break;
            }
        }
    }
}

