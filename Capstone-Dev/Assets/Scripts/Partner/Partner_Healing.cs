using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partner_Healing : MonoBehaviour {
    Partner_Movement partner;
    public GameObject HealthPack;

    // Use this for initialization
    void Start () {
        partner = GetComponent<Partner_Movement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (partner.skillReady)
        {
            if (Input.GetButtonDown("XButton"))
            {
                partner.partner_Character.Animator.Play("ThrowSupply");
                GameObject NewProj = Instantiate(HealthPack);
                NewProj.transform.position = transform.position;
                Vector3 lookDirection = partner.player.transform.position - transform.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                NewProj.transform.eulerAngles = new Vector3(0, 0, angle);
                partner.skillNum --;
                partner.skillTimer = 0;
            }
        }
    }
}
