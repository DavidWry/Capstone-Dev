using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class JumpImpact : MonoBehaviour {

    private int damage;

    private Player_New player;
    private Transform player2;
    public GameObject effect;
    private Vector3 offset;
    private Scene scene;
    
    void Start ()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "2_1" || scene.name == "2_2" || scene.name == "3_1" || scene.name == "3_2")
        {
            effect.transform.localScale = new Vector3(2.9f,2.9f,2.9f);
            offset = new Vector3(0, -10.0f, 0);
        }
        else
        {
            effect.transform.localScale = new Vector3(0.15f,0.15f,0.15f);
            offset = new Vector3(0, -0.6f, 0);
        }
        damage = 5;
        player = GetComponent<Player_New>();
        player2 = GameObject.FindGameObjectWithTag("Player").transform;
       
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        Destroy(gameObject, 0.5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Instantiate(effect, player2.position + offset, Quaternion.identity);
            other.gameObject.GetComponent<Player_New>().TakeDamage(damage);
            

        }
    }
}
