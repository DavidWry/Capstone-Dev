using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

[System.Serializable]
public class PlayerData {
    public Weapon LeftWeapon;
    public Weapon RightWeapon;
    public Weapon ThirdWeapon;
    public List<string> NpcIDs;            //Npc player met.
    public float Hp;
    public int Ap;

    public PlayerData (Player_New player)
    {
        LeftWeapon = player.leftWeapon;
        RightWeapon = player.rightWeapon;
        ThirdWeapon = player.thirdWeapon;
        NpcIDs = player.NPCIDs;
        Hp = player.HitPoint;
        Ap = player.Power;
    }
}
