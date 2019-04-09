using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partner_Trans : MonoBehaviour {

    Partner_Movement partner;
    //Projectile Reated

    GameObject Player;
    public GameObject Effect;

    // Use this for initialization
    void Start () {
        partner = GetComponent<Partner_Movement>();
        Player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (partner.skillReady)
        {
            if (Input.GetButtonDown("XButton"))
            {
                StartCoroutine(Teleporter());
            }
        }
    }

    private IEnumerator Teleporter()
    {
        Instantiate(Effect, Player.transform);
        Instantiate(Effect, transform);
        yield return new WaitForSeconds(0.4f);
        Player.GetComponent<Rigidbody>().MovePosition(transform.position);
    }
}
