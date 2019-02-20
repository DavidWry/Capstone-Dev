using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class WeaponColor : MonoBehaviour {

    public int leftorright;
    GameObject slide;
    private GameObject playerObj;
    private Shoot_New shoot;
    private Player_New player;
    private SpriteRenderer sprite;
    private Movement_New movement;

    // Use this for initialization
    void Start () {
        if (transform.Find("SlideTransform").GetChild(0))
            slide = transform.Find("SlideTransform").GetChild(0).gameObject;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        shoot = playerObj.GetComponent<Shoot_New>();
        player = playerObj.GetComponent<Player_New>();
        sprite = slide.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (slide != null && player)
        {
            if (leftorright == 1)
            {
                float ratio = (float)player.leftWeapon.CurrentAmmos / (float)player.leftWeapon.AmmoSize;
                if (player.leftWeapon.CurrentAmmos == 0)
                {
                    sprite.color = Color.red;
                }
                else if (ratio < 0.3)
                {
                    sprite.color = Color.magenta;
                }
                else
                {
                    sprite.color = Color.green;
                }
            }
            else if (leftorright == 2)
            {
                float ratio = (float)player.rightWeapon.CurrentAmmos / (float)player.rightWeapon.AmmoSize;
                if (player.rightWeapon.CurrentAmmos == 0)
                {
                    sprite.color = Color.red;
                }
                else if (ratio < 0.3)
                {
                    sprite.color = Color.magenta;
                }
                else
                {
                    sprite.color = Color.green;
                }
            }
        }
    }
}
