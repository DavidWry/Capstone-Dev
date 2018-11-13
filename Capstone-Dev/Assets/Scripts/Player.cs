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

        // Use this for initialization
        void Start()
        {
            //LeftHand = transform.GetChild(0);
            //RightHand = transform.GetChild(1);
            rotateSpeed = 100f;
        }

        // Update is called once per frame
        void Update()
        {
            if (RightTarget != null)
            {
                Vector3 lookDirection = RightTarget.transform.position - LeftHand.transform.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                LeftHand.transform.rotation = Quaternion.Slerp(LeftHand.transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
            }
        }
    }

}