using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

[System.Serializable]
public class PlayerData {
    public string LeftWeapon;
    public string RightWeapon;
    public string ThirdWeapon;
    public List<string> NpcIDs;            //Npc player met.
    public float Hp;
    public int Ap;

    public PlayerData (Player_New player)
    {
        LeftWeapon = player.leftWeapon.Name;
        RightWeapon = player.rightWeapon.Name;
        ThirdWeapon = player.thirdWeapon.Name;
        NpcIDs = player.NPCIDs;
        Hp = player.HitPoint;
        Ap = player.Power;
    }
}
