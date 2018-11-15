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

        public Transform LeftHand;
        public Transform RightHand;

        private float rotateSpeed;
        private Movement playerMovement;

        // Use this for initialization
        void Start()
        {
            rotateSpeed = 100f;
            playerMovement = GetComponent<Movement>();
        }

        // Update is called once per frame
        void Update()
        {
            if (RightTarget != null)
            {
                if (playerMovement.IsFaceRight)
                {
                    Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                    //LeftHand.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(Input.GetAxis("Right Y"), Input.GetAxis("Right X")) * 180 / Mathf.PI);
                }
                else
                {
                    Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                    Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
                    LeftHand.transform.Rotate(180, 0, 0);
                }
            }
        }
    }

}