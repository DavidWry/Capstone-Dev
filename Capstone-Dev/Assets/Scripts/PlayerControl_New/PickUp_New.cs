using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class PickUp_New : MonoBehaviour
{

    public Transform LeftHand;
    public Transform RightHand;
    public float WeaponSizeUp = 1;
    private Player_New player;
    private Shoot_New shoot;
    private bool isLootNearby = false;
    [SerializeField]
    private Loot currentLoot = null;
    private GameManager gameManager = null;
    public float ItemSizeUp = 1;

    void Start()
    {
        player = gameObject.GetComponent<Player_New>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shoot = gameObject.GetComponent<Shoot_New>();
        //LeftHand = transform.GetChild(0);
        //RightHand = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {

        int handToPick = 0;//1=Left; 2=Right
        if (Input.GetKeyDown(KeyCode.JoystickButton4) && !player.IsHit)
        {
            handToPick = 1;
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton5) && !player.IsHit)
        {
            handToPick = 2;
        }
        //get the weapon
        /*
        if (isLootNearby && (handToPick > 0) && currentLoot != null)
        {
            //check if we already have the weapon and show it
            //bool HasAlreadyWeapon = false;

            //check if looting give a weapon
            if (currentLoot.Item.GiveWeapon)
            {
                Debug.Log(currentLoot.Item.WeaponName.ToString());
                //give the right weapon 
                foreach (Weapon weaponInManager in gameManager.WeaponList)
                    if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                    {

                        if (handToPick == 1)
                        {
                            if (player.leftWeapon.Name != "")
                            {
                                string tempName = player.leftWeapon.WeaponName.ToString();
                                GameObject itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                                itemObj.transform.localScale = WeaponSizeUp * itemObj.transform.localScale;
                                if (NextScene.nowName == "2_1")
                                    itemObj.transform.localScale = new Vector3(4, 4, 4);
                                var worldCanvas = GameObject.Find("worldCanvas").transform;
                                itemObj.transform.parent = worldCanvas;

                            }
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.leftWeapon = newWeapon;
                            //Debug.Log(newWeapon.WeaponName);
                            RefreshWeaponUI(1);
                            ChangeWeapon(1, currentLoot.Item.WeaponName);
                        }

                        else if (handToPick == 2)
                        {
                            if (player.rightWeapon.Name != "")
                            {
                                string tempName = player.rightWeapon.WeaponName.ToString();
                                GameObject itemObj = gameManager.GetItemObj(tempName);
                                itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                                itemObj.transform.localScale = WeaponSizeUp * itemObj.transform.localScale;
                                if (NextScene.nowName == "2_1")
                                    itemObj.transform.localScale = new Vector3(4, 4, 4);
                                var worldCanvas = GameObject.Find("worldCanvas").transform;
                                itemObj.transform.parent = worldCanvas;
                            }
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.rightWeapon = newWeapon;
                            RefreshWeaponUI(2);
                            ChangeWeapon(2, currentLoot.Item.WeaponName);
                        }
                        handToPick = 0;
                    }

                //destroy loot
                GameObject.Destroy(currentLoot.gameObject);
                //clear loot
                currentLoot = null;
            }
        }*/

        //switch weapon
        if (handToPick > 0)
        {
            if (player.Slots == 0)
            {
                SwichHand();
            }
            else if (player.Slots > 0)
            {
                if (handToPick == 1 && player.thirdWeapon.Name != "") //Left
                {
                    Weapon temp = player.leftWeapon;
                    //get a copy of this weapon 
                    Weapon newWeapon = CopyWeapon(player.thirdWeapon);

                    //new weapon is already recharge
                    newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                    //found the weapon, add it for the player
                    player.leftWeapon = newWeapon;
                    RefreshWeaponUI(1);
                    ChangeWeapon(1, player.thirdWeapon.WeaponName);

                    player.thirdWeapon = temp;
                }
                else if (handToPick == 2 && player.thirdWeapon.Name != "") //Right
                {
                    Weapon temp = player.rightWeapon;
                    //get a copy of this weapon 
                    Weapon newWeapon = CopyWeapon(player.thirdWeapon);

                    //new weapon is already recharge
                    newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                    //found the weapon, add it for the player
                    player.rightWeapon = newWeapon;
                    RefreshWeaponUI(2);
                    ChangeWeapon(2, player.thirdWeapon.WeaponName);

                    player.thirdWeapon = temp;
                }
                else if (player.thirdWeapon.Name == "")
                {
                    SwichHand();
                }
            }
        }


        //get the item or weapon.
        if (isLootNearby && Input.GetButtonDown("BButton") && currentLoot != null  || Input.GetKeyDown(KeyCode.B))//button B on controller
        {
            SoundManager.PlaySound("Pickup");
            if (!currentLoot.Item.GiveWeapon)
            {
                if (currentLoot.Item.Name == "PurpleCrystal")
                {
                    player.Power += 20;
                }
                else if (currentLoot.Item.Name == "RedCrystal")
                {
                    player.Power += 10;
                }
                //destroy loot
                GameObject.Destroy(currentLoot.gameObject);
                //clear loot
                currentLoot = null;
            }
            else
            {
                if (player.leftWeapon.Name == "")
                {
                    foreach (Weapon weaponInManager in gameManager.WeaponList)
                        if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                        {
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.leftWeapon = newWeapon;
                            RefreshWeaponUI(1);
                            ChangeWeapon(1, currentLoot.Item.WeaponName);
                        }
                }
                else if (player.rightWeapon.Name == "")
                {
                    foreach (Weapon weaponInManager in gameManager.WeaponList)
                        if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                        {
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.rightWeapon = newWeapon;
                            RefreshWeaponUI(2);
                            ChangeWeapon(2, currentLoot.Item.WeaponName);
                        }
                }
                else if (player.thirdWeapon.Name == "" && player.Slots > 0)
                {
                    foreach (Weapon weaponInManager in gameManager.WeaponList)
                        if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                        {
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.thirdWeapon = newWeapon;
                        }
                }
                else if (player.thirdWeapon.Name != "" && player.Slots == 1)
                {
                    string tempName = player.thirdWeapon.WeaponName.ToString();
                    GameObject itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    itemObj.transform.localScale = WeaponSizeUp * itemObj.transform.localScale;
                    if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                        itemObj.transform.localScale = new Vector3(1, 1, 1) * ItemSizeUp * 1.5f;
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                    foreach (Weapon weaponInManager in gameManager.WeaponList)
                        if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                        {
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.thirdWeapon = newWeapon;
                        }
                }
                else if (player.Slots == 0)
                {
                    string tempName = player.rightWeapon.WeaponName.ToString();
                    GameObject itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
                    itemObj.transform.localScale = WeaponSizeUp * itemObj.transform.localScale;
                    if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                        itemObj.transform.localScale = new Vector3(1, 1, 1) * ItemSizeUp * 1.5f;
                    var worldCanvas = GameObject.Find("worldCanvas").transform;
                    itemObj.transform.parent = worldCanvas;
                    foreach (Weapon weaponInManager in gameManager.WeaponList)
                        if (currentLoot.Item.WeaponName.ToString() == weaponInManager.WeaponName.ToString())
                        {
                            //get a copy of this weapon 
                            Weapon newWeapon = CopyWeapon(weaponInManager);

                            //new weapon is already recharge
                            newWeapon.CurrentAmmos = newWeapon.AmmoSize;

                            //found the weapon, add it for the player
                            player.rightWeapon = newWeapon;
                            RefreshWeaponUI(2);
                            ChangeWeapon(2, currentLoot.Item.WeaponName);
                        }
                }
                //destroy loot
                GameObject.Destroy(currentLoot.gameObject);
                //clear loot
                currentLoot = null;
            }
        }

    }

    public void RefreshLoot(Loot loot)
    {

        //show the loot items on ground
        if (loot != null)
        {
            //Debug.Log(loot.Item.ItemName);
            Item tempItem = gameManager.GetItem(loot.Item.ItemName);
            //Debug.Log(tempItem.ItemName);
            loot.InitialiseLoot(tempItem, gameManager);
            loot.ShowHide(true);
        }
        else if (currentLoot != null)
            currentLoot.ShowHide(false);

        //check loot
        currentLoot = loot;

        if (loot != null)
        {
            isLootNearby = true;

        }
        else
        {
            isLootNearby = false;
        }
    }

    //refresh all the weapon on top
    //1=left;2=right
    public void RefreshWeaponUI(int leftOrRight)
    {
        //clear everything
        //gameManager.leftWeaponMenu.UpdateWeaponMenu(null);
        //gameManager.rightWeaponMenu.UpdateWeaponMenu(null);

        if (leftOrRight == 1)
        {
            gameManager.leftWeaponMenu.UpdateWeaponMenu(player.leftWeapon);
        }
        else if (leftOrRight == 2)
        {
            gameManager.rightWeaponMenu.UpdateWeaponMenu(player.rightWeapon);
        }
    }

    //change the weapon on hand
    //1=left;2=right
    public void ChangeWeapon(int leftOrRight, WeaponName weaponName)
    {
        //Find face right or left
        Movement_New playerMovement = gameObject.GetComponent<Movement_New>();
        bool isFaceRight = playerMovement.IsFaceRight;
        float eulerAngel;
        if (isFaceRight)
            eulerAngel = 0;
        else
            eulerAngel = 180;
        GameObject weaponObj = gameManager.GetWeaponObj(weaponName);
        Vector3 tempPosition = new Vector3(0, 0, 0);
        weaponObj = Instantiate(weaponObj, tempPosition, Quaternion.Euler(0, eulerAngel, 0));
        weaponObj.transform.localScale = WeaponSizeUp * weaponObj.transform.localScale;
        shoot.CombineOn = false;

        if (leftOrRight == 1)
        {
            // clear objects on hand

            foreach (Transform child in LeftHand)
            {
                Destroy(child.gameObject);
            }
            weaponObj.transform.parent = LeftHand;
            weaponObj.transform.localPosition = tempPosition;
            weaponObj.transform.eulerAngles = LeftHand.eulerAngles + new Vector3(0, 0, -player.fixLeftAngle);
            if (weaponObj.GetComponent<WeaponColor>())
            {
                weaponObj.GetComponent<WeaponColor>().leftorright = 1;
            }
            player.CombineWeapon();
        }
        else if (leftOrRight == 2)
        {
            // clear objects on hand

            foreach (Transform child in RightHand)
            {
                Destroy(child.gameObject);
            }

            weaponObj.transform.parent = RightHand;
            weaponObj.transform.localPosition = tempPosition;
            weaponObj.transform.eulerAngles = RightHand.eulerAngles + new Vector3(0, 0, -player.fixRightAngle);
            if (weaponObj.GetComponent<WeaponColor>())
            {
                weaponObj.GetComponent<WeaponColor>().leftorright = 2;
            }
            player.CombineWeapon();
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

    private void SwichHand()
    {
        Weapon temp = player.rightWeapon;

        Weapon newWeapon = CopyWeapon(player.leftWeapon);

        //new weapon is already recharge
        newWeapon.CurrentAmmos = player.leftWeapon.CurrentAmmos;

        //found the weapon, add it for the player
        player.rightWeapon = newWeapon;
        RefreshWeaponUI(2);
        ChangeWeapon(2, player.leftWeapon.WeaponName);

        Weapon newWeapon02 = CopyWeapon(temp);

        //new weapon is already recharge
        newWeapon02.CurrentAmmos = temp.CurrentAmmos;

        //found the weapon, add it for the player
        player.leftWeapon = newWeapon02;
        RefreshWeaponUI(1);
        ChangeWeapon(1, temp.WeaponName);
    }

    void OnTriggerEnter(Collider col)
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
        if (loot != null)
        {
            //make the player refresh it's pixel tiles variables
            RefreshLoot(loot);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Item")
        {
            Loot loot = col.GetComponent<Loot>();

            if (loot != null)
            {
                loot = null;
                RefreshLoot(loot);
            }
        }

    }
}
