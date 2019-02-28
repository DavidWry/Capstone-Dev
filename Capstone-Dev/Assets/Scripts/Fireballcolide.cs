using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class Fireballcolide : MonoBehaviour {
        public GameObject explosion;
        public GameObject player1;
        private float remainingtime;
        // Use this for initialization
        void Start() {
            remainingtime = Random.Range(4, 8);
            player1 = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update() {
            remainingtime -= Time.deltaTime;
            if (remainingtime <= 0)
            {

                Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                if (Vector3.Distance(player1.transform.position, gameObject.transform.parent.transform.position) < 75)
                {
                    player1.GetComponent<Player_New>().TakeDamage(15);


                }
                Destroy(gameObject.transform.parent.gameObject);
               
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {

                Instantiate(explosion, other.transform.position, Quaternion.identity);
                Destroy(gameObject.transform.parent.gameObject);
                 
                player1.GetComponent<Player_New>().TakeDamage(15);
            }
        }
    }
}