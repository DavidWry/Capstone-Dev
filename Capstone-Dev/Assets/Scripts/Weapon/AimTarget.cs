using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class AimTarget : MonoBehaviour {

    private LineRenderer lineRenderer;
    private GameObject playerObj;
    private Shoot_New shoot;
    private Player_New player;
    private SpriteRenderer sprite;
    private Movement_New movement;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        shoot = playerObj.GetComponent<Shoot_New>();
        player = playerObj.GetComponent<Player_New>();
        sprite = GetComponent<SpriteRenderer>();
        movement = playerObj.GetComponent<Movement_New>();
        lineRenderer.widthMultiplier = shoot.BulletSizeUp;
    }
	
	// Update is called once per frame
	void Update () {       
        lineRenderer.SetPosition(1, transform.position);
        sprite.color = Color.white;
        if (player.isLeftInHand && !player.isRightInHand && player.leftWeapon.WeaponName != WeaponName.Sword && player.leftWeapon.WeaponName != WeaponName.Lazer)
        {
            if (shoot.Left.GetChild(0).childCount > 0)
                lineRenderer.SetPosition(0, shoot.Left.GetChild(0).GetChild(0).position);
            else
            {
                lineRenderer.SetPosition(0, transform.position);
            }
        }
        else if (player.isRightInHand && !player.isLeftInHand && player.rightWeapon.WeaponName != WeaponName.Sword && player.rightWeapon.WeaponName != WeaponName.Lazer)
        {
            if (shoot.Right.GetChild(0).childCount > 0)
                lineRenderer.SetPosition(0, shoot.Right.GetChild(0).GetChild(0).position);
            else
            {
                lineRenderer.SetPosition(0, transform.position);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
        }
        if (movement.isBulletTime)
        {
            lineRenderer.SetPosition(0, transform.position);
        }
        if (shoot.CombineOn)
        {
            lineRenderer.SetPosition(0, shoot.CombineBulPos.position);
            lineRenderer.SetPosition(1, shoot.CombineBulPos.position + shoot.CombineBulPos.right * 5 * shoot.BulletSizeUp);
        }

    }
}
