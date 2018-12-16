using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public GameObject FloatNum;
    public bool Shoot;
    public float btwShoot;
    public GameObject Proj;
    private float times;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Shoot)
        {
            times += Time.deltaTime;
            if (times > btwShoot)
            {
                GameObject proj = Instantiate(Proj);
                proj.transform.position = transform.position;
                proj.transform.eulerAngles = new Vector3(0, 0, 0);
                proj.tag = "EnemyProjectile";
                Projectile Proj0 = proj.GetComponent<Projectile>();
                Proj0.IsReady = true;
                Proj0.Damage = 0;
                Proj0.Speed = 10;
                Proj0.Duration = 5;
                times = 0;
            }
        }
	}

    public void TakeDamage(int Damage)
    {
        GameObject FloatObj = Instantiate(FloatNum, transform.position, transform.rotation);
        FloatObj.GetComponent<FloatingNumbers>().Damage = Damage;
    }
}
