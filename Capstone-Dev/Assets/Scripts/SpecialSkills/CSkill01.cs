using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSkill01 : MonoBehaviour {

    public GameObject Hook;
    private GameObject CurrentHook;
    private Shoot_New shoot;

	// Use this for initialization
	void Start () {
        shoot = GetComponent<Shoot_New>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("XButton") && CurrentHook == null)
        {
            GameObject MultiNewProj = Instantiate(Hook);
            MultiNewProj.transform.position = shoot.Center.position;
            MultiNewProj.transform.rotation = shoot.Right.rotation;
            CurrentHook = MultiNewProj;
        }
	}
}
