using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStar : MonoBehaviour {

    public TrailRenderer trail;

	// Use this for initialization
	void Start () {
        if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3")
            trail.widthMultiplier = 16;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, 1800f) * Time.deltaTime);
	}
}
