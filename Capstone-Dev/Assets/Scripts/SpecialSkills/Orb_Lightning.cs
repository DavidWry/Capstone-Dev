using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_Lightning : MonoBehaviour {

    public GameObject Lightnnig;
    public float distance = 9.18f;
    float timeBtw = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeBtw += Time.deltaTime;
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && timeBtw >= 0.5f)
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
}
