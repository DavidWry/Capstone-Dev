using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class UI_SkillIcon : MonoBehaviour {

    public List<Sprite> Icons;
    private Player_New player;
    private Image image;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<Player_New>();
        image = gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (player.CombineType)
        {
            case 0:
                image.sprite = Icons[0];
                break;
            case 11:
                image.sprite = Icons[11];
                break;
            case 12:
                image.sprite = Icons[1];
                break;
            case 13:
                image.sprite = Icons[2];
                break;
            case 14:
                image.sprite = Icons[3];
                break;
            case 15:
                image.sprite = Icons[4];
                break;
            case 23:
                image.sprite = Icons[5];
                break;
            case 24:
                image.sprite = Icons[6];
                break;
            case 25:
                image.sprite = Icons[7];
                break;
            case 34:
                image.sprite = Icons[8];
                break;
            case 35:
                image.sprite = Icons[9];
                break;
            case 45:
                image.sprite = Icons[10];
                break;
            case 22:
                image.sprite = Icons[12];
                break;
            case 33:
                image.sprite = Icons[13];
                break;
            case 44:
                image.sprite = Icons[14];
                break;
        }
    }
}
