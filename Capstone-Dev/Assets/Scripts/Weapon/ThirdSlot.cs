using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class ThirdSlot : MonoBehaviour {

    public Sprite empty;
    public GameObject Glowing;
    private Player_New player;
    private GameObject PlayerObj;
    private Image weaponIcon;
    private GameManager gameManager;
    private string currentName;


    // Use this for initialization
    void Start () {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player_New>();
        weaponIcon = GetComponent<Image>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (player.Slots == 0)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentName != player.thirdWeapon.Name)
        {
            if (player.thirdWeapon.Name == "")
            {
                weaponIcon.sprite = empty;
            }
            else
            {
                weaponIcon.sprite = player.thirdWeapon.WeaponIcon;
            }
            if (transform.childCount == 0)
                Instantiate(Glowing, transform);
        }
        currentName = player.thirdWeapon.Name;
    }

}
