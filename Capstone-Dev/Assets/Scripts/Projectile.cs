using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public bool IsReady = false;
    public int Damage = 1;
    public int Rebounce = 0;
    public float Speed = 1f;
    public bool Thrust;
    public GameObject Impact;
    public GameManager GameManage;
    public float Duration = 1;
    private float LifeTime;

    private Rigidbody RBody;
    private CapsuleCollider Collider;

	// Use this for initialization
	void Start () {
        RBody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        LifeTime = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        LifeTime += Time.deltaTime;
        if (IsReady)
        {
            IsReady = false;
            RBody.velocity = transform.right * Speed;
        }
        if (LifeTime >= Duration)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (Thrust)
            {
                //collision.gameObject.GetComponent<Rigidbody>().velocity *= -1;
            }
            Destroy(gameObject);
        }
    }
}
