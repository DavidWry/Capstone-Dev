using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class PickUp : MonoBehaviour {

    public Player Player;

    private bool isLootNearby = false;
    private Loot currentLoot = null;
    private GameManager gameManager = null;
    // Use this for initialization
    void Start () {
        //Player = transform.parent.GetComponent<Player>();
        Player = gameObject.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        
        //get the item
        if (isLootNearby && Input.GetKeyDown(KeyCode.Joystick1Button1) && currentLoot != null)
        {

            //check if we already have the weapon and show it
            bool HasAlreadyWeapon = false;

            //check if looting give a weapon
            if (currentLoot.Item.GiveWeapon)
            {
                //give the right weapon 
                foreach (Weapon weaponInManager in gameManager.WeaponList)
                    if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                    {
                        foreach (Weapon weaponInPlayer in Player.WeaponList)
                            if (weaponInPlayer.WeaponName == weaponInManager.WeaponName)
                                HasAlreadyWeapon = true;

                        if (!HasAlreadyWeapon)
                        {
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            Player.WeaponList.Add(newWeapon);
                        }
                    }
            }

            if (gameManager.ItemLootedAnim != null)
            {
                //show the itemlooted going up
                GameObject newObj = gameManager.ItemLootedAnim;

                //enable it for the very first use
                newObj.SetActive(true);

                Text label = newObj.transform.Find("Label").GetComponent<Text>();

                //show different message
                if (HasAlreadyWeapon)
                {
                    label.text = "Already have :";
                    label.color = Color.red;
                }
                else
                {
                    label.color = Color.white;
                    label.text = "Received :"; //change info on it
                }

                //change info on it
                Text text = newObj.transform.Find("ItemName").GetComponent<Text>();

                //show the right item name
                text.text = currentLoot.Item.Name;

                //default color
                text.color = Color.green;
                if (currentLoot.Item.DamageType == WeaponValueType.Average)
                    text.color = Color.yellow;
                else if (currentLoot.Item.DamageType == WeaponValueType.High)
                    text.color = Color.red;
                else if (currentLoot.Item.DamageType == WeaponValueType.GODLY)
                    text.color = new Color(255f, 0f, 195f, 0f);

                //reshow the whole animation
                newObj.GetComponent<Animator>().SetTrigger("Show");
            }

            //destroy loot
            if (!HasAlreadyWeapon)
            {
                GameObject.Destroy(currentLoot.gameObject);
               // RefreshWeaponUI();
            }

            //clear loot
            currentLoot = null;
        }
        

    }

    public void RefreshLoot(Loot loot)
    {
        //show the loot items on ground
        if (loot != null)
            loot.ShowHide(true);
        else if (currentLoot != null)
            currentLoot.ShowHide(false);

        //check loot
        currentLoot = loot;

        if (loot != null){
            isLootNearby = true;
            gameManager.PressBObject.SetActive(true);
        }
        else {
            isLootNearby = false;
            gameManager.PressBObject.SetActive(false);
        }
    }
    /*
    //refresh all the weapon on top
    void RefreshWeaponUI()
    {
        //clear everything
        foreach (Tds_WeaponMenu vCurMenu in vGameManager.vWeaponMenuList)
            UpdateWeaponMenu(vCurMenu, null);

        int vcpt = 0;
        foreach (Tds_Weapons vCurWeapon in ListWeapons)
        {

            //get the right menu 
            Tds_WeaponMenu vCurMenu = vGameManager.vWeaponMenuList[vcpt];

            //check if we are on the selected weapon
            if (vcpt == vCurWeapIndex)
                vCurMenu.WepPanel.color = Color.yellow;
            else
                vCurMenu.WepPanel.color = Color.white;

            //update the menu
            UpdateWeaponMenu(vCurMenu, vCurWeapon);

            //increase counter
            vcpt++;
        }
    }
    */

    //make a TRUE copy of this weapon to be used
    public Weapon CopyWeapon(Weapon oldWeapon)
    {
        //copy everything for the weapon
        Weapon newWeapon = new Weapon();
        newWeapon.Rebounce = oldWeapon.Rebounce;
        newWeapon.AmmoSize = oldWeapon.AmmoSize;
        newWeapon.Damage = oldWeapon.Damage;
        newWeapon.Name = oldWeapon.Name;
        newWeapon.Projectile = oldWeapon.Projectile;
        newWeapon.WeaponIcon = oldWeapon.WeaponIcon;
        newWeapon.WeaponObj = oldWeapon.WeaponObj;
        newWeapon.AimObj = oldWeapon.AimObj;
        newWeapon.ImpactFX = oldWeapon.ImpactFX;
        newWeapon.ProjectileSpeed = oldWeapon.ProjectileSpeed;
        newWeapon.ShotFX = oldWeapon.ShotFX;
        newWeapon.ClipObj = oldWeapon.ClipObj;
        newWeapon.TimeBetweenShot = oldWeapon.TimeBetweenShot;
        newWeapon.AttackAnimationUsed = oldWeapon.AttackAnimationUsed;
        newWeapon.WeaponType = oldWeapon.WeaponType;
        newWeapon.TimeWaited = oldWeapon.TimeWaited;
        newWeapon.CurrentAmmos = oldWeapon.CurrentAmmos;
        newWeapon.BulletAngleList = oldWeapon.BulletAngleList;

        //return new tds_weapons
        return newWeapon;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Tds_Tile vTile = col.GetComponent<Tds_Tile>();
        Loot loot = col.GetComponent<Loot>();
        /*
        if (vTile != null)
        {

            //ONLY refresh variable current level
            vListCollider.Add(vTile);

            //make the player refresh it's pixel tiles variables
            if (vCharacter != null)
                vCharacter.RefreshVariables(vListCollider);
        }
        */
        if (loot != null){
            //make the player refresh it's pixel tiles variables
            RefreshLoot(loot);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //Tds_Tile vTile = col.GetComponent<Tds_Tile>();
        Loot loot = col.GetComponent<Loot>();

        //check if we have it on the list so we can remove it
        //if (vTile != null)
        //{
        //    if (vListCollider.Contains(vTile))
        //   {
        //       vListCollider.Remove(vTile);

        //make the player refresh it's pixel tiles variables
        //       vCharacter.RefreshVariables(vListCollider);
        //   }
        // }
        if (loot != null){
            loot = null;
            RefreshLoot(loot);
        }
    }
}
