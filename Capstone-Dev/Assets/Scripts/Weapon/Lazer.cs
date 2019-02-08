using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

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
                        if (timeCounter >= 0.3)
                        {
                            if (hit.transform.gameObject.GetComponent<EnemySuicideBomber>())
                            {
                                hit.transform.gameObject.GetComponent<EnemySuicideBomber>().TakeDamage(Damage/3);
                            }
                            else if (hit.transform.gameObject.GetComponent<EnemyRangedSpear>())
                            {
                                hit.transform.gameObject.GetComponent<EnemyRangedSpear>().TakeDamage(Damage/3);
                            }
                            else if (hit.transform.gameObject.GetComponent<EnemyRangedStomp>())
                            {
                                hit.transform.gameObject.GetComponent<EnemyRangedStomp>().TakeDamage(Damage/3);
                            }
                            else if (hit.transform.gameObject.GetComponent<NewEnemyJumper>())
                            {
                                hit.transform.gameObject.GetComponent<NewEnemyJumper>().TakeDamage(Damage/3);
                            }
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
