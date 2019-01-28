using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Data;
using Assets.HeroEditor.Common.Enums;
using HeroEditor.Common;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.EditorScripts;
using HeroEditor.Common.Enums;
using UnityEngine;
using AssemblyCSharp;

namespace AssemblyCSharp
{

    public class Player_New : MonoBehaviour
    {
        public Weapon leftWeapon;
        public Weapon rightWeapon;
        public GameObject LeftTarget;
        public GameObject RightTarget;
        public int CombineType;
        public List<string> NPCIDs;          //NPCs player met.

        public float fixRightAngle = 0;
        public float fixLeftAngle = 0;
        public float fixAngle = 110;

        public Transform LeftHand;
        public Transform RightHand;

        public int HitPoint;
        public int Power;

        public SpriteCollection SpriteCollection;
        public Character Character;
        public FirearmCollection firearmCollection;

        private float rotateSpeed;
        private Movement_New playerMovement;
        private Shoot_New playerShoot;
        private bool isLeftInHand = true;
        private bool isRightInHand = false;
        private bool isCombine = false;

        // Use this for initialization
        void Start()
        {
            HitPoint = 100;
            rotateSpeed = 100f;
            playerMovement = GetComponent<Movement_New>();
            playerShoot = GetComponent<Shoot_New>();
            Character = GetComponent<Character>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Character.EquipFirearm(SpriteCollection.Firearms1H[6].Sprites, firearmCollection.Firearms[0], false);
            }
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

            if (HitPoint <= 0)
            {
                Destroy(gameObject);
                Debug.Log("Dead");
            }
            if (Power > 100)
            {
                Power = 100;
            }
            else if (Power == 100)
            {
                playerShoot.SkillReady = true;
            }

            if (playerMovement.isBulletTime)
            {
                if (LeftTarget != null && RightTarget != null)
                {
                    Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    LeftHand.transform.eulerAngles = new Vector3(0, 0, angle + fixLeftAngle);

                    lookDirection = LeftTarget.transform.position - RightHand.transform.position;
                    angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                    lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (RightTarget != null)
                {
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
                        }
                        else if (isRightInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - RightHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            //Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            //RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, angle);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, 0);
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
                            LeftHand.transform.Rotate(180, 0, 0);
                            RightHand.transform.Rotate(0, 180, 0);
                        }
                        else if (isRightInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - RightHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            //Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            //RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, angle);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, 0);
                            RightHand.transform.Rotate(180, 0, 0);
                            LeftHand.transform.Rotate(0, 180, 0);
                        }
                    }
                }
            }
        }

        public void CombineWeapon()
        {
            if (leftWeapon.WeaponName == rightWeapon.WeaponName)
            {
                CombineType = 0;
            }
            else
            {
                if (leftWeapon.WeaponName == WeaponName.Pistol || rightWeapon.WeaponName == WeaponName.Pistol)
                {
                    if (leftWeapon.WeaponName == WeaponName.Ak47 || rightWeapon.WeaponName == WeaponName.Ak47)
                    {
                        CombineType = 12;
                        playerShoot.CombineTag = false;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                    {
                        CombineType = 13;
                        playerShoot.CombineTag = true;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 14;
                        playerShoot.CombineTag = false;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 15;
                        playerShoot.CombineTag = false;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Ak47 || rightWeapon.WeaponName == WeaponName.Ak47)
                {
                    if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                    {
                        CombineType = 23;
                        playerShoot.CombineTag = true;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 24;
                        playerShoot.CombineTag = true;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 25;
                        playerShoot.CombineTag = true;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                {
                    if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 34;
                        playerShoot.CombineTag = true;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 35;
                        playerShoot.CombineTag = true;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                {
                    if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 45;
                        playerShoot.CombineTag = true;
                    }
                }
            }
        }
        //take damage with hp.
        public void TakeDamage(int Damage)
        {
            HitPoint -= Damage;
        }

        public void SavePlayerData()
        {
            //SaveSystem.SavePlayer(this);
        }

        public void LoadPlayerData()
        {
            PlayerData data = SaveSystem.LoadPlayer();

            //Loading process will depend on what kind of situation.
        }

        public void ChangeWeapon()
        {
            Character.EquipFirearm(SpriteCollection.Firearms2H[0].Sprites, firearmCollection.Firearms[0], true);
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
        }
    }
}
