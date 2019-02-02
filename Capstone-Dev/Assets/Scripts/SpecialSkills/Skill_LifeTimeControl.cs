using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_LifeTimeControl : MonoBehaviour {

    public Image bar;
    public float FullTime;
    private float currentTime;
    public float AddTime;
    public float coefficient;
    private float moreTime;

	// Use this for initialization
	void Start () {
        currentTime = FullTime;
        moreTime = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        bar.fillAmount = currentTime / FullTime;
        currentTime -= Time.deltaTime * moreTime;

        if (currentTime <= 0)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);
            currentTime += AddTime;
            moreTime += coefficient;
            if (currentTime > FullTime)
            {
                currentTime = FullTime;
            }
        }
    }
}
