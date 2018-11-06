using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    public enum ItemName { Potion, Pistol, Lazer, Ak47, Shotgun, DemoGun1 };
    public enum WeaponValueType { Low, Average, High, GODLY };

    [System.Serializable]
    public class Item
    {
        //check what we give 
        public string Name = "";
        public ItemName ItemName = ItemName.Potion;
        public Sprite ItemIcon = null;                      //how it will look like when dropping on ground
        public bool GiveWeapon = false;
        public bool Usable = true;                          //if spawn debris = false, we just spawn a item on ground 
        public WeaponValueType DamageType = WeaponValueType.Low;
        public WeaponValueType AmmoType = WeaponValueType.Low;
        public WeaponName WeaponName = WeaponName.Pistol;
        public GameObject itemObj;

        //make a TRUE copy of this weapon to be used
        public Item CopyItem(Item oldItem)
        {
           // Debug.Log("B");
            //copy everything for the weapon
            Item newItem = new Item();
            newItem.ItemName = oldItem.ItemName;
            newItem.Name = oldItem.Name;
            newItem.GiveWeapon = oldItem.GiveWeapon;
            newItem.ItemIcon = oldItem.ItemIcon;
            newItem.WeaponName = oldItem.WeaponName;
            newItem.Usable = oldItem.Usable;
            newItem.AmmoType = oldItem.AmmoType;
            newItem.DamageType = oldItem.DamageType;
            newItem.itemObj = oldItem.itemObj;

            //return new tds_weapons
            return newItem;
        }
    }
}
