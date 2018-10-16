using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{

    public Item Item;
    private Ray ray;
    private RaycastHit hit;
    private bool showItem = false;
    private GameManager gameManager;
    private bool cursorIsAbove = false;

    Transform damageLabel;
    Transform damageValue;
    Transform ammoLabel;
    Transform ammoValue;
    Transform panel;
    Transform nameValue;

    void Start()
    {
        showItem = false;
    }

    public Sprite GetAssociateValue(WeaponValueType type)
    {
        Sprite icon = gameManager.LowIcon;

        if (type == WeaponValueType.Average)
            icon = gameManager.MediumIcon;
        else if (type == WeaponValueType.High)
            icon = gameManager.HighIcon;
        else if (type == WeaponValueType.GODLY)
            icon = gameManager.GodlyIcon;

        return icon;
    }

    //loot has the complete list for showing the right result on it's own
    public void InitialiseLoot(Item newItem, GameManager newGameManager)
    {
        gameManager = newGameManager;
        //get the items
        Item = newItem;

        //show or hide info about weapons
        damageLabel = transform.Find("DmgLabel");
        damageValue = transform.Find("DmgValue");
        ammoLabel = transform.Find("AmmoLabel");
        ammoValue = transform.Find("AmmoValue");
        panel = transform.Find("Panel");
        nameValue = transform.Find("Name");

        //item name
        nameValue.GetComponent<Text>().text = newItem.Name;

        //potions infos here

        //check if it's a weapon or a potion
        if (newItem.GiveWeapon)
        {
            //item name
            damageValue.GetComponent<Image>().sprite = GetAssociateValue(newItem.DamageType);
            ammoValue.GetComponent<Image>().sprite = GetAssociateValue(newItem.AmmoType);
        }

        //disable everything by default
        damageLabel.gameObject.SetActive(false);
        damageValue.gameObject.SetActive(false);
        ammoLabel.gameObject.SetActive(false);
        ammoValue.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        nameValue.gameObject.SetActive(false);

        //doens't show the item at first
        showItem = false;
    }

    public void ShowHide(bool show)
    {
        bool CanShow = false;
        if (show || cursorIsAbove)
            CanShow = true;

        //make sure we show/hide correctly (with player above items or with cursor)
        nameValue.gameObject.SetActive(CanShow);
        panel.gameObject.SetActive(CanShow);

        if (Item.GiveWeapon)
        {
            damageLabel.gameObject.SetActive(CanShow);
            damageValue.gameObject.SetActive(CanShow);
            ammoLabel.gameObject.SetActive(CanShow);
            ammoValue.gameObject.SetActive(CanShow);
        }

        //replace old value
        showItem = CanShow;

    }

    void OnMouseOver()
    {
        cursorIsAbove = true;

        if (!showItem)
            ShowHide(!showItem);
    }

    void OnMouseExit()
    {
        cursorIsAbove = false;

        if (showItem)
            ShowHide(!showItem);
    }
}
