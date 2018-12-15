using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumbers : MonoBehaviour {

    public float MoveSpeed;
    public float Damage;
    public float LifeTime;
    public TextMesh DisplayNumber;

    private float lifeTime;
	// Use this for initialization
	void Start () {
        lifeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        DisplayNumber.characterSize = Damage / 100 * 0.5f + 1;
        DisplayNumber.text = "" + Damage;
        transform.position = new Vector3(transform.position.x, transform.position.y + (MoveSpeed * Time.deltaTime), transform.position.z);
        lifeTime += Time.deltaTime;
        if (lifeTime >= LifeTime)
        {
            Destroy(gameObject);
        }
    }
}
