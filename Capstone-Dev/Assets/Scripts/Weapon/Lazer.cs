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
        if (NextScene.nowName == "3_1" || NextScene.nowName == "3_2" || NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3" || NextScene.nowName == "3_3")
        {
            LazerRenderer.widthMultiplier = 15;
        }
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
                            else if (hit.transform.gameObject.GetComponent<EnemySlider>())
                            {
                                hit.transform.gameObject.GetComponent<EnemySlider>().TakeDamage(Damage/3);
                            }
                            else if (hit.transform.gameObject.GetComponent<MiniBoss>())
                            {
                                hit.transform.gameObject.GetComponent<MiniBoss>().TakeDamage(Damage);
                            }
                            timeCounter = 0;
                        }
                    }
                    else if (hit.transform.tag == "Boss")
                    {
                        if (timeCounter >= 0.3)
                        {
                            if (hit.transform.gameObject.GetComponent<Fsmandhp>())
                                hit.transform.gameObject.GetComponent<Fsmandhp>().takedamage(Damage / 3);
                            timeCounter = 0;
                        }
                    }
                    else if (hit.transform.tag == "Chest")
                    {
                        if (timeCounter >= 0.3)
                        {
                            if(hit.transform.gameObject.GetComponent<Chest>())
                                hit.transform.gameObject.GetComponent<Chest>().TakeDamage(Damage / 3);
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
