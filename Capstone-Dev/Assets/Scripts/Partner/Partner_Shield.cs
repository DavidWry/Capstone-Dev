using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partner_Shield : MonoBehaviour {
    public GameObject shield;
    Partner_Movement partner;
    bool shieldUp = false;
    bool takeButton = true;
    float Timer = 0;
    public float MaxTime = 5;

    // Use this for initialization
    void Start () {
        partner = GetComponent<Partner_Movement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (partner.skillReady)
        {
            if (Input.GetButtonDown("XButton") && takeButton)
            {
                shieldUp = true;
                GameObject dam = Instantiate(shield, transform);
                dam.transform.localPosition += new Vector3(0, 1, 0);
                takeButton = false;
            }
        }
        if (shieldUp)
        {
            Timer += Time.deltaTime;
            partner.partner_Character.Animator.SetBool("Crouch", true);
            partner.enabled = false;

            if (Timer > MaxTime)
            {
                shieldUp = false;
                partner.partner_Character.Animator.SetBool("Crouch", false);
                partner.enabled = true;
                partner.skillNum--;
                partner.skillTimer = 0;
                Timer = 0;
                takeButton = true;
            }
            if (transform.lossyScale.x <= 20)
            {
                transform.localScale += new Vector3(1, 1, 1) * 10 * Time.deltaTime * 3;
            }
        }
        else
        {
            if (transform.lossyScale.x >= 10)
            {
                transform.localScale -= new Vector3(1, 1, 1) * 10 * Time.deltaTime * 3;
            }
        }
    }
}
