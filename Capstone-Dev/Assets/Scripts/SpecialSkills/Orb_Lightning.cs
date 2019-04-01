using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_Lightning : MonoBehaviour {

    public GameObject Lightnnig;
    public float distance = 9.18f;
    float timeBtw = 0;
    int Damage = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeBtw += Time.deltaTime;
	}

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Minion" && timeBtw >= 0.5f)
        {
            if (collision.gameObject.GetComponent<EnemySuicideBomber>())
            {
                collision.gameObject.GetComponent<EnemySuicideBomber>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<EnemyRangedSpear>())
            {
                collision.gameObject.GetComponent<EnemyRangedSpear>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<EnemyRangedStomp>())
            {
                collision.gameObject.GetComponent<EnemyRangedStomp>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<NewEnemyJumper>())
            {
                collision.gameObject.GetComponent<NewEnemyJumper>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<EnemySlider>())
            {
                collision.gameObject.GetComponent<EnemySlider>().TakeDamage(Damage);
            }
            else if (collision.gameObject.GetComponent<MiniBoss>())
            {
                collision.gameObject.GetComponent<MiniBoss>().TakeDamage(Damage);
            }

            DrawLightning(collision);
        }
    }

    void DrawLightning(Collider other)
    {
            Vector3 start = transform.parent.position;
            Vector3 end = other.gameObject.transform.position;
            float newdistance = Vector3.Distance(start, end);
            float scale = newdistance / distance;
            Vector3 lookDirection = start - end;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            Vector3 position = (start + end) / 2;
            GameObject atack = Instantiate(Lightnnig);
            atack.transform.position = position;
            atack.transform.eulerAngles = new Vector3(0, 0, angle);
            float y = Random.Range(0.8f, 1.1f);
            int posORnag = Random.Range(0, 2);
            posORnag = posORnag * 2 - 1;
            atack.transform.localScale = new Vector3(-1, y * posORnag, 1) * scale;
            timeBtw = 0;
    }
}
