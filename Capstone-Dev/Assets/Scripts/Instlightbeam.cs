using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instlightbeam : MonoBehaviour {
    float jiange=0.8f;
    float timing = 0;
    float angle = 0;
    public GameObject lightbeam;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        timing += Time.deltaTime;
        if (timing >= jiange &&angle<360)
        {
            Vector3 tempvec = new Vector3(angle - 90, 90 ,90);
            GameObject lightb = Instantiate(lightbeam, this.gameObject.transform.position, Quaternion.Euler(tempvec));
            lightb.transform.localEulerAngles = tempvec;
            lightb.transform.parent = this.gameObject.transform;
            angle += 30;
            timing = 0;
        }



       

	}


    public void resetjg()
    {
        gameObject.SetActive(true);
        angle = 0;
    
    }
}
