using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : Projectile {
    private LineRenderer LazerRenderer;
    public Transform LazerHit;
    public bool IsReloading;
    public float LazeDuration;
    private float timeCounter;

	// Use this for initialization
	void Start () {
        LazerRenderer = GetComponent<LineRenderer>();
        LazerRenderer.useWorldSpace = true;
        timeCounter = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (IsReady)
        {
            LazerRenderer.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.right, out hit))
            {
                if (hit.collider)
                {
                    LazerRenderer.SetPosition(1, hit.point);
                    if (hit.transform.tag == "Minion")
                    {
                        EnemyFollow enemy = hit.transform.gameObject.GetComponent<EnemyFollow>();
                        if (timeCounter >= 1)
                        {
                            enemy.TakeDamage(Damage);
                            timeCounter = 0;
                        }
                    }
                }
            }
            else
            {
                LazerRenderer.SetPosition(1, transform.right * 10);
            }
            LazeDuration -= Time.deltaTime;
            timeCounter += Time.deltaTime;

        }
        else
        {
            Destroy(gameObject);
        }
        if (LazeDuration < 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
