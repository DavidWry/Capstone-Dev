using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class PickUp : MonoBehaviour {

    private Player Player;

    private bool isLootNearby = false;
    private Loot currentLoot = null;
    private GameManager gameManager = null;
    // Use this for initialization
    void Start () {
        //Player = transform.parent.GetComponent<Player>();
        Player = gameObject.GetComponent<Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        int handToPick = 0;//1=Left; 2=Right
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            handToPick = 1;
            

        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton5)) {
            handToPick = 2;
           
        }
        //get the item
        if (isLootNearby && (handToPick>0) && currentLoot != null)
        {
            //check if we already have the weapon and show it
            //bool HasAlreadyWeapon = false;

            //check if looting give a weapon
            if (currentLoot.Item.GiveWeapon)
            {
                
                //give the right weapon 
                foreach (Weapon weaponInManager in gameManager.WeaponList)
                    if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                    {
                        if (handToPick == 1)
                        {
                            if (Player.leftWeapon.Name != "")
                            {
                                string tempName = Player.leftWeapon.WeaponName.ToString();
                                GameObject itemObj = gameManager.GetItemObj(tempName);
                                itemObj=Instantiate(itemObj, transform.position, Quaternion.Euler(0, 0, 0));
                                var worldCanvas = GameObject.Find("worldCanvas").transform;
                                itemObj.transform.parent = worldCanvas;

                            }
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            Player.leftWeapon=newWeapon;
                            //Debug.Log(newWeapon.WeaponName);
                            RefreshWeaponUI(1);
                        }

                        else if (handToPick == 2)
                        {
                            if (Player.rightWeapon.Name != "")
                            {
                                string tempName = Player.rightWeapon.WeaponName.ToString();
                                GameObject itemObj = gameManager.GetItemObj(tempName);
                                itemObj = Instantiate(itemObj, transform.position, Quaternion.Euler(0, 0, 0));
                                var worldCanvas = GameObject.Find("worldCanvas").transform;
                                itemObj.transform.parent = worldCanvas;

                            }
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            Player.rightWeapon = newWeapon;
                            RefreshWeaponUI(2);
                        }


                        handToPick = 0;
                    }
            }

            //destroy loot
           GameObject.Destroy(currentLoot.gameObject);
           // 
            

            //clear loot
            currentLoot = null;
        }
        

    }

    public void RefreshLoot(Loot loot)
    {
        
        //show the loot items on ground
        if (loot != null)
        {
            Item tempItem = gameManager.GetItem(ItemName.Pistol);
            //Debug.Log("enter loot");
            loot.InitialiseLoot(tempItem, gameManager);
            loot.ShowHide(true);
        }
        else if (currentLoot != null)
            currentLoot.ShowHide(false);

        //check loot
        currentLoot = loot;

        if (loot != null){
            isLootNearby = true;
            
        }
        else {
            isLootNearby = false;
            
        }
    }
    
    //refresh all the weapon on top
    //1=left;2=right
    void RefreshWeaponUI(int leftOrRight)
    {
        //clear everything
        //gameManager.leftWeaponMenu.UpdateWeaponMenu(null);
        //gameManager.rightWeaponMenu.UpdateWeaponMenu(null);

        if (leftOrRight == 1)
        {
            gameManager.leftWeaponMenu.UpdateWeaponMenu(Player.leftWeapon);
        }
        else if (leftOrRight == 2)
        {
            gameManager.rightWeaponMenu.UpdateWeaponMenu(Player.rightWeapon);
        }
    }

    


    //make a TRUE copy of this weapon to be used
    public Weapon CopyWeapon(Weapon oldWeapon)
    {
        //copy everything for the weapon
        Weapon newWeapon = new Weapon();
        newWeapon.Name = oldWeapon.Name;
        newWeapon.WeaponName = oldWeapon.WeaponName;
        newWeapon.WeaponType = oldWeapon.WeaponType;
        newWeapon.WeaponIcon = oldWeapon.WeaponIcon;
        newWeapon.WeaponObj = oldWeapon.WeaponObj;
        newWeapon.AimObj = oldWeapon.AimObj;
        newWeapon.Projectile = oldWeapon.Projectile;
        newWeapon.IsMultiBullets = oldWeapon.IsMultiBullets;
        newWeapon.BulletAngleList = oldWeapon.BulletAngleList;
        newWeapon.TimeBetweenShot = oldWeapon.TimeBetweenShot;
        newWeapon.TimeWaited = oldWeapon.TimeWaited;
        newWeapon.ImpactFX = oldWeapon.ImpactFX;
        newWeapon.ShotFX = oldWeapon.ShotFX;
        newWeapon.Damage = oldWeapon.Damage;
        newWeapon.Rebounce = oldWeapon.Rebounce;
        newWeapon.AmmoSize = oldWeapon.AmmoSize;
        newWeapon.CurrentAmmos = oldWeapon.CurrentAmmos;
        newWeapon.ClipObj = oldWeapon.ClipObj;
        newWeapon.AttackAnimationUsed = oldWeapon.AttackAnimationUsed;
        newWeapon.ReloadTime = oldWeapon.ReloadTime;
        newWeapon.Duration = oldWeapon.Duration;
        newWeapon.ProjectileSpeed = oldWeapon.ProjectileSpeed;
        newWeapon.IsThrust = oldWeapon.IsThrust;
        newWeapon.IsLazer = oldWeapon.IsLazer;

        return newWeapon;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("enter");
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
