using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int Damage = 1;
    public float speed = 0.1f;
    private float lifetime = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime * speed;
        lifetime += Time.deltaTime;
        if (lifetime > 0.5f)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Minion")
        {
            if (other.gameObject.GetComponent<EnemyFollow>())
            {
                other.gameObject.GetComponent<EnemyFollow>().TakeDamage(Damage);
            }
        }
    }

}
