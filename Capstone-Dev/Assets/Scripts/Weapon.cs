using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    public enum WeaponName { Pistol, ZombieHand, EnergyRifle, Ak47, Shotgun, DemoGun1 };

    public enum WeaponType { Melee, Ranged };

    [System.Serializable]
    public class Weapon
    {

        public string Name = "";
        public WeaponName WeaponName;                          //link the weapon for every NPC & Player by this field
        public WeaponType WeaponType = WeaponType.Ranged;      //Melee = (knife, hand, club...) Ranged (all sort of rifle, pistol)
        //public bool Is2Handed = false;                          //check if we hold the weapon with 2 hand or not (Pistol or Rifle))
        public Sprite WeaponIcon = null;                       //show the right weapon at the bottom
        public GameObject WeaponObj = null;                    //Has the weapon + bullets position 
        public GameObject AimObj = null;                       //where the projectile come from
        public GameObject Projectile = null;                   //Projectile used in this weapon
        public bool IsMultiBullets = false;
        public List<float> BulletAngleList;                    //create as many bullet in the list with the specific angle (shotgun)
        public float TimeBetweenShot = 0.5f;
        public float TimeWaited = 0f;
        public GameObject ImpactFX = null;                     //spawn something when impacting wall, npc. (explosion, spark...)
        public GameObject ShotFX = null;                       //little spark when shooting 
        public int Damage = 1;                                    //here we handle the dmg which will be transfered to the projectile
        public int Rebounce = 0;                                //how many time the bullets will bounche on wall until destroyed
        public int AmmoSize = 5;                               //how many bullets are in this gun before recharging.
        public int CurrentAmmos = 0;
        public GameObject ClipObj = null;                      //leave a clip on the ground when reloading
        public string AttackAnimationUsed = "";                 //each weapon can have their own shooting animation on the character. Put them here 
        public float ReloadTime = 3.0f;                         //Each weapon has its own reload time.
        //For Projectiles
        public float Duration = 3.0f;
        public float ProjectileSpeed = 5.0f;                    //how fast will the projectile go
        public bool IsThrust = false;                              //Like a shot gun, true means will push enemy backward a little bit.
        public bool IsLazer = false;                              //If it's true, projectiles will be lazer.
    }
}