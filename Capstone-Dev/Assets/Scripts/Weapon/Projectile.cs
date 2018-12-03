using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public bool IsReady = false;
    public int Damage = 1;
    public int Rebounce = 0;
    public float Speed = 1f;
    public bool Thrust;
    public bool Pierce = false;
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
        if (collision.gameObject.tag == "Minion")
        {
            if (collision.gameObject.GetComponent<EnemyFollow>())
            {
                collision.gameObject.GetComponent<EnemyFollow>().TakeDamage(Damage);
            }
            if (Thrust)
            {
                //collision.gameObject.GetComponent<Rigidbody>().velocity *= -1;
            }
            Dead();
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            Dead();
        }
        else if (collision.gameObject.tag == "Chest")
        {
            Dead();
            collision.GetComponent<Chest>().TakeDamage(Damage);
        }
    }

    private void Dead()
    {
        if (Impact != null)
        {
            GameObject ImpactObject = Instantiate(Impact, transform.position, transform.rotation);
        }
        if (!Pierce)
        {
            Destroy(gameObject);
        }
    }
}
