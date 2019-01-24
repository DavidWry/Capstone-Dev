using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

[System.Serializable]
public class PlayerData {
    public Weapon LeftWeapon;
    public Weapon RightWeapon;
    public List<string> NpcIDs;            //Npc player met.
    public int Hp;
    public int Ap;

    public PlayerData (Player player)
    {
        LeftWeapon = player.leftWeapon;
        RightWeapon = player.rightWeapon;
        NpcIDs = player.NPCIDs;
        Hp = player.HitPoint;
        Ap = player.Power;
    }
}
