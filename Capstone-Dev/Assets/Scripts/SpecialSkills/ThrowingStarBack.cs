using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ThrowingStarBack : MonoBehaviour {

    private GameObject PlayerObj;
    private int hurtTimes = 0;
    private float lifeTime = 0;
    public Collider star;
    private bool back = false;
    private float speed;
    public GameObject Impact;

	// Use this for initialization
	void Start () {
        PlayerObj = GameObject.FindWithTag("Player");
        speed = GetComponent<Projectile>().Speed;
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;
        if ((Mathf.Round(Input.GetAxisRaw("LeftTrigger")) > 0 || Mathf.Round(Input.GetAxisRaw("RightTrigger")) > 0) && lifeTime > 1.2f)
        {
            star.enabled = false;
            back = true;
        }
        if (back && PlayerObj != null)
        {
            if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3"||NextScene.nowName == "3_1" || NextScene.nowName == "3_2" || NextScene.nowName == "3_3")
                transform.position = Vector3.MoveTowards(transform.position, PlayerObj.transform.position, speed * 50 * Time.deltaTime);
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerObj.transform.position, speed * 5 * Time.deltaTime);
            }
        }
        if (lifeTime > 5f)
        {
            if (PlayerObj)
                if (PlayerObj.GetComponent<Shoot_New>().CombineOn)
                {
                    Destroy(gameObject);
                }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            GameObject text = Instantiate(Impact);
            text.transform.position = (transform.position + collision.transform.position) / 2;
            text.transform.rotation = transform.rotation;
            if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3"||NextScene.nowName == "3_1" || NextScene.nowName == "3_2" || NextScene.nowName == "3_3")
                text.transform.localScale *= 16;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && lifeTime > 1f)
        {
            if (PlayerObj)
            {
                PlayerObj.GetComponent<Shoot_New>().combine_15.SetActive(true);
                PlayerObj.GetComponent<Shoot_New>().CombineTag_15 = true;
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Minion" || other.tag == "Boss")
        {
            hurtTimes++;
        }
    }
}
