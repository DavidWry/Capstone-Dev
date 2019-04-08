using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Partner_Throw : MonoBehaviour {

    Partner_Movement partner;
    public GameObject ThrowingStar;
    //Projectile Reated
    [SerializeField]
    private float speed = 100;
    [SerializeField]
    private float Duration = 3;
    [SerializeField]
    private int Damage = 20;

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
                GameObject NewProj = Instantiate(ThrowingStar);
                NewProj.transform.position = transform.position;
                Vector3 lookDirection = partner.player.transform.position - transform.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                NewProj.transform.eulerAngles = new Vector3(0, 0, angle + Random.Range(-10, 10));
                //Change state according to the weapon
                Projectile Proj = NewProj.GetComponent<Projectile>();
                Proj.IsReady = true;
                Proj.Speed = speed;
                Proj.Duration = Duration;
                Proj.Damage = Damage;
                Proj.Pierce = true;
            }
        }
	}
}
