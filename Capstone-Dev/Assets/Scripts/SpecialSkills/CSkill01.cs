using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CSkill01 : MonoBehaviour {

    public GameObject Hook;
    private GameObject CurrentHook;
    private Shoot_New shoot;
    private Player_New player;
    private float timer = 0;

	// Use this for initialization
	void Start () {
        shoot = GetComponent<Shoot_New>();
        player = GetComponent<Player_New>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("XButton") && CurrentHook == null)
        {
            GameObject MultiNewProj = Instantiate(Hook);
            MultiNewProj.transform.position = shoot.Center.position;
            if (player.isLeftInHand)
            {
                MultiNewProj.transform.eulerAngles = new Vector3(shoot.Left.eulerAngles.x, shoot.Left.eulerAngles.y, shoot.Left.eulerAngles.z - player.fixLeftAngle);
            }
            else if (player.isRightInHand)
            {
                MultiNewProj.transform.eulerAngles = new Vector3(shoot.Right.eulerAngles.x, shoot.Right.eulerAngles.y, shoot.Right.eulerAngles.z - player.fixRightAngle);
            }
            CurrentHook = MultiNewProj;
            if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2")
                MultiNewProj.transform.localScale *= 15;
        }
        if (CurrentHook != null)
        {
            timer += Time.deltaTime;
            if (timer > 4.0f)
            {
                Destroy(CurrentHook);
                CurrentHook = null;
                timer = 0;
            }
        }
	}
}
