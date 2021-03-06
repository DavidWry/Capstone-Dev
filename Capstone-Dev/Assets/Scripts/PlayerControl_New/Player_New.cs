﻿using System.Collections.Generic;
using HeroEditor.Common;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.EditorScripts;
using UnityEngine;
using System.Xml;

namespace AssemblyCSharp
{
    public class Player_New : MonoBehaviour
    {
        public Weapon leftWeapon;
        public Weapon rightWeapon;
        public Weapon thirdWeapon;
        public GameObject LeftTarget;
        public GameObject RightTarget;
        public int CombineType;
        public List<string> NPCIDs;          //NPCs player met.
        public string CurrentPartner;
        float armor = 1;
        public float fixRightAngle = 0;
        public float fixLeftAngle = 0;
        public float fixAngle = 110;

        public Transform LeftHand;
        public Transform RightHand;
        public Transform Head;
        XmlDocument goldDoc = new XmlDocument();
        string progressionFilePath;
        public List<SpriteRenderer> SpriteRenderers;
        public bool IsHit = false;
        private float FlashTime = 0;
        private float WillFlashTime = 0.05f;
        public Material mat1;
        public Material mat2;
        public Material mat3;
        public SpriteRenderer eyes;

        public float HitPoint;
        public int Power;
        public int Slots = 0;
        public Vector3 Recoil = new Vector3(0, 0, 0);

        public SpriteCollection SpriteCollection;
        public Character Character;
        public FirearmCollection firearmCollection;
        public GameObject FailUI;

        public Sprite BackPack;
        public SpriteRenderer Back;

        private float rotateSpeed;
        private Movement_New playerMovement;
        private Shoot_New playerShoot;
        private PickUp_New playerPick;
        public bool isLeftInHand = true;
        public bool isRightInHand = false;
        public bool isCombine = false;

        // Use this for initialization
        void Start()
        {
            progressionFilePath = Application.dataPath + "/Resources/Progression.xml";
            goldDoc.Load(progressionFilePath);

            HitPoint = 100;
            rotateSpeed = 100f;
            playerMovement = GetComponent<Movement_New>();
            playerShoot = GetComponent<Shoot_New>();
            playerPick = GetComponent<PickUp_New>();
            Character = GetComponent<Character>();
            SpriteRenderers = GetComponent<LayerManager>().Sprites;
            if (goldDoc.DocumentElement.SelectSingleNode("UC3/Completed").InnerText == "true")
            {
                Slots = 1;
            }
            else
            {
                Slots = 0;
                if (GameObject.Find("Switch"))
                GameObject.Find("Switch").SetActive(false);

            }

            if (goldDoc.DocumentElement.SelectSingleNode("UC5/Completed").InnerText == "true")
            {
                armor = 0.9f;
            }
            else { armor = 1; print("aslfkj"); }
        }

        public void Initalize()
        {
            HitPoint = 100;
            rotateSpeed = 100f;
            playerMovement = GetComponent<Movement_New>();
            playerShoot = GetComponent<Shoot_New>();
            playerPick = GetComponent<PickUp_New>();
            Character = GetComponent<Character>();
            SpriteRenderers = GetComponent<LayerManager>().Sprites;
        }

        private void Update()
        { 
            if (IsHit)
            {
                FlashTime += Time.deltaTime;
                if (FlashTime < WillFlashTime)
                {
                    foreach (SpriteRenderer spt in SpriteRenderers)
                    {
                        if (spt != null)
                            spt.material = mat1;
                    }
                }
                else
                {
                    foreach (SpriteRenderer spt in SpriteRenderers)
                    {
                        if (spt != null)
                            spt.material = mat2;
                    }
                    FlashTime = 0;
                    IsHit = false;
                    eyes.material = mat3;
                }
            }
            if (HitPoint <= 0)
            {
                Destroy(gameObject);
                Instantiate(FailUI);
 
            }
            else
            {
                if (HitPoint > 100)
                    HitPoint = 100;
            }
            if (Power > 100)
            {
                Power = 100;
            }
            else if (Power == 100)
            {
                playerShoot.SkillReady = true;
            }
            playerMovement.Recoil += Recoil;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (playerShoot.IsLeftShooting)
            {
                isLeftInHand = true;
                isRightInHand = false;
            }
            else if (playerShoot.IsRightShooting)
            {
                isRightInHand = true;
                isLeftInHand = false;
            }
            else if (playerShoot.IsCombineShooting)
            {
                isCombine = true;
            }
            else if (isCombine)
            {
                isCombine = false;
                isLeftInHand = true;
            }
            if (playerMovement.isBulletTime)
            {
                if (RightTarget != null)
                {
                    if (!playerMovement.IsFaceRight)
                    {
                        Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        LeftHand.transform.eulerAngles = new Vector3(0, 0, angle - fixAngle);

                        lookDirection = RightTarget.transform.position - RightHand.transform.position;
                        angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                        lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        RightHand.transform.eulerAngles = new Vector3(0, 0, angle);
                        LeftHand.transform.Rotate(180, 0, 0);
                        RightHand.transform.Rotate(180, 0, 0);
                    }
                    else
                    {
                        Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                        Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        LeftHand.transform.eulerAngles = new Vector3(0, 0, angle + fixAngle);

                        lookDirection = RightTarget.transform.position - RightHand.transform.position;
                        angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                        lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        RightHand.transform.eulerAngles = new Vector3(0, 0, angle);
                    }
                }
            }
            else
            {
                if (RightTarget != null)
                {
                    if (playerShoot.CombineOn && playerShoot.CombineTag)
                    {
                        isLeftInHand = true;
                    }
                    if (playerMovement.IsFaceRight)
                    {
                        if (isLeftInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            //Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            //LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, angle + fixAngle);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, - fixRightAngle);
                            HeadLookAround(true);
                        }
                        else if (isRightInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - RightHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            //Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            //RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, angle);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, 0);
                            HeadLookAround(true);
                        }
                    }
                    else
                    {
                        if (isLeftInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            //Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            //LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, angle - fixAngle);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, fixRightAngle);
                            HeadLookAround(false);
                            LeftHand.transform.Rotate(180, 0, 0);
                            RightHand.transform.Rotate(0, 180, 0);
                            Head.transform.Rotate(0, 180, 0);
                        }
                        else if (isRightInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - RightHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            //Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            //RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, angle);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, 0);
                            HeadLookAround(false);
                            RightHand.transform.Rotate(180, 0, 0);
                            LeftHand.transform.Rotate(0, 180, 0);
                            Head.transform.Rotate(0, 180, 0);
                        }
                    }
                }
            }
        }

        public void CombineWeapon()
        {
            if (leftWeapon.WeaponName == rightWeapon.WeaponName)
            {
                if (leftWeapon.WeaponName == WeaponName.Pistol)
                {
                    CombineType = 11;
                    playerShoot.CombineTag = true;
                    playerShoot.CombineAmmos = 30;
                }
                else if (leftWeapon.WeaponName == WeaponName.Ak47)
                {
                    CombineType = 22;
                    playerShoot.CombineTag = true;
                    playerShoot.CombineAmmos = 80;
                }
                else if (leftWeapon.WeaponName == WeaponName.Lazer)
                {
                    CombineType = 33;
                    playerShoot.CombineTag = false;
                    playerShoot.CombineAmmos = 1;
                }
                else if (leftWeapon.WeaponName == WeaponName.Shotgun)
                {
                    CombineType = 44;
                    playerShoot.CombineTag = true;
                    playerShoot.CombineAmmos = 50;
                }
                else if (leftWeapon.WeaponName == WeaponName.Sword)
                {
                    CombineType = 55;
                    playerShoot.CombineTag = true;
                    playerShoot.CombineTag_55 = true;
                    playerShoot.CombineAmmos = 50;
                }
            }
            else
            {
                if (leftWeapon.WeaponName == WeaponName.Pistol || rightWeapon.WeaponName == WeaponName.Pistol)
                {
                    if (leftWeapon.WeaponName == WeaponName.Ak47 || rightWeapon.WeaponName == WeaponName.Ak47)
                    {
                        CombineType = 12;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 50;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                    {
                        CombineType = 13;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 10;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 14;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 20;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 15;
                        playerShoot.CombineAmmos = 20;
                        playerShoot.CombineTag = true;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Ak47 || rightWeapon.WeaponName == WeaponName.Ak47)
                {
                    if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                    {
                        CombineType = 23;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 500;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 24;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 30;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 25;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 50;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                {
                    if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 34;
                        playerShoot.CombineTag = false;
                        playerShoot.CombineAmmos = 20;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 35;
                        playerShoot.CombineTag = true;
                        playerShoot.CombineAmmos = 20;
                        playerShoot.CombineTag_35 = true;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                {
                    if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 45;
                        playerShoot.CombineTag = false;
                        playerShoot.CombineAmmos = 20;
                    }
                }
            }
        }
        //take damage with hp.
        public void TakeDamage(float Damage)
        {
            HitPoint -= (Damage * armor);
            //SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            IsHit = true;
        }

        public void SavePlayerData()
        {
            SaveSystem.SavePlayer(this);
        }

        public void LoadPlayerData()
        {
            PlayerData data = SaveSystem.LoadPlayer();
            GameManager gameManager;
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            foreach (Weapon currentWeapon in gameManager.WeaponList)
            {
                if (currentWeapon.Name == data.LeftWeapon)
                {
                    leftWeapon = currentWeapon;
                    leftWeapon.CurrentAmmos = leftWeapon.AmmoSize;
                }
                if (currentWeapon.Name == data.RightWeapon)
                {
                    rightWeapon = currentWeapon;
                    rightWeapon.CurrentAmmos = rightWeapon.AmmoSize;
                }
                if (currentWeapon.Name == data.ThirdWeapon)
                {
                    thirdWeapon = currentWeapon;
                }
            }

            HitPoint = data.Hp;
            Power = data.Ap;
            NPCIDs = data.NpcIDs;

            playerPick.ChangeWeapon(1, leftWeapon.WeaponName);
            playerPick.ChangeWeapon(2, rightWeapon.WeaponName);

            playerPick.RefreshWeaponUI(1);
            playerPick.RefreshWeaponUI(2);
            //Loading process will depend on what kind of situation.

            CurrentPartner = data.Partner;
            if (CurrentPartner != "")
            {
                NPCManager npcManager = FindObjectOfType<NPCManager>();
                npcManager.PartnerSetUp(CurrentPartner);
            }
        }

        public void ChangeWeapon(string weaponName)
        {
            List<Sprite> weaponSprites = new List<Sprite>();
            foreach (SpriteGroupEntry spriteGroupEntry in SpriteCollection.Firearms2H)
            {
                if (spriteGroupEntry.Name == weaponName)
                {
                    weaponSprites = spriteGroupEntry.Sprites;
                }
            }
            if (weaponSprites.Count > 0)
            {
                Character.EquipFirearm(weaponSprites, firearmCollection.Firearms[0], true);
            }
            else
            {
                foreach (SpriteGroupEntry spriteGroupEntry in SpriteCollection.Firearms1H)
                {
                    if (spriteGroupEntry.Name == weaponName)
                    {
                        weaponSprites = spriteGroupEntry.Sprites;
                    }
                }
                if (weaponSprites.Count > 0)
                {
                    Character.EquipFirearm(weaponSprites, firearmCollection.Firearms[0], false);
                }
                else
                {
                    Sprite weaponSprites02 = null;
                    Sprite weaponSprites03 = null;
                    foreach (SpriteGroupEntry spriteGroupEntry in SpriteCollection.MeleeWeapon1H)
                    {
                        if (spriteGroupEntry.Name == weaponName)
                        {
                            weaponSprites02 = spriteGroupEntry.Sprite;
                        }
                    }
                    foreach (SpriteGroupEntry spriteGroupEntry in SpriteCollection.MeleeWeaponTrail1H)
                    {
                        if (spriteGroupEntry.Name == weaponName)
                        {
                            weaponSprites03 = spriteGroupEntry.Sprite;
                        }
                    }
                    if (weaponSprites02 != null && weaponSprites03 != null)
                    {
                        Character.EquipMeleeWeapon(weaponSprites02, weaponSprites03, false);
                    }
                    else
                        EmptyWeapon();
                }
            }
        }

        public void EmptyWeapon()
        {
            List<Sprite> Emptys = new List<Sprite>();
            Sprite empty = null;
            for (int i = 0; i < 7; i++)
            {
                Emptys.Add(empty);
            }
            Character.EquipFirearm(Emptys, firearmCollection.Firearms[0], false);
            Character.EquipMeleeWeapon(empty, empty, false);
            AnimeBack();
        }

        public void Throw()
        {
            Character.Animator.Play("ThrowSupply");
        }

        public void Jab()
        {
            Character.Animator.Play("JabMelee1H");
        }

        public void AnimeBack()
        {
            Character.Animator.Play("ReadyRightHandOnly");
        }

        public void ChangeBackPack()
        {
            Back.sprite = BackPack;
        }

        public void EmptyBackPack()
        {
            Back.sprite = null;
        }

        public void HeadLookAround(bool right)
        {
            if (right)
            {
                Vector3 lookDirection = RightTarget.transform.position - Head.transform.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                Head.transform.eulerAngles = new Vector3(0, 0, angle / 3);
            }
            else
            {
                Vector3 lookDirection = - RightTarget.transform.position + Head.transform.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                Head.transform.eulerAngles = new Vector3(0, 0, angle / 3);
            }
        }

        public void Emoji(string face)
        {
            Character.SetExpression(face);
        }
    }
}
