using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserbump : MonoBehaviour {
    float curtime=0;
    bool done = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        curtime += Time.deltaTime;
        if (!done && this.gameObject.transform.localScale.x < 0.8f) {

            this.gameObject.transform.localScale -= new Vector3(-0.02f, 0, 0);
        }


        if (curtime>2&&this.gameObject.transform.localScale.x < 2.3f&&!done)
        {
            this.gameObject.transform.localScale = new Vector3(2.3f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);

            done = true;
        }

        if (done&& this.gameObject.transform.localScale.x >0)
        {
            this.gameObject.transform.localScale -= new Vector3(0.1f, 0, 0);




        }

        if(done&& this.gameObject.transform.localScale.x <= 0)
        {

            Destroy(this.gameObject.transform.parent.gameObject);

        }



	}



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<Player>().TakeDamage(20);//same
                
                }


    }
}
