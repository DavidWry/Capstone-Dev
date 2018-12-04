using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    public class Player : MonoBehaviour
    {
        public Weapon leftWeapon;
        public Weapon rightWeapon;
        public GameObject LeftTarget;
        public GameObject RightTarget;
        public int CombineType;

        public Transform LeftHand;
        public Transform RightHand;

        public int HitPoint;
        public int Power;

        private float rotateSpeed;
        private Movement playerMovement;
        private Shoot playerShoot;
        private bool isLeftInHand = true;
        private bool isRightInHand = false;
        private bool isCombine = false;

        // Use this for initialization
        void Start()
        {
            HitPoint = 100;
            rotateSpeed = 100f;
            playerMovement = GetComponent<Movement>();
            playerShoot = GetComponent<Shoot>();
        }

        // Update is called once per frame
        void Update()
        {
            if(playerShoot.IsLeftShooting)
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
                    LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

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
                            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else if (isRightInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - RightHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            LeftHand.transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                    }
                    else
                    {
                        if (isLeftInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                            RightHand.transform.eulerAngles = new Vector3(0, 0, 0);
                            LeftHand.transform.Rotate(180, 0, 0);
                            RightHand.transform.Rotate(0, 180, 0);
                        }
                        else if (isRightInHand)
                        {
                            Vector3 lookDirection = RightTarget.transform.position - RightHand.transform.position;
                            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            RightHand.transform.rotation = Quaternion.Slerp(RightHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
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
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Shotgun || rightWeapon.WeaponName == WeaponName.Shotgun)
                    {
                        CombineType = 14;
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 15;
                    }
                }
                else if (leftWeapon.WeaponName == WeaponName.Ak47 || rightWeapon.WeaponName == WeaponName.Ak47)
                {
                    if (leftWeapon.WeaponName == WeaponName.Lazer || rightWeapon.WeaponName == WeaponName.Lazer)
                    {
                        CombineType = 23;
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
                    }
                    else if (leftWeapon.WeaponName == WeaponName.Sword || rightWeapon.WeaponName == WeaponName.Sword)
                    {
                        CombineType = 35;
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
    }
}