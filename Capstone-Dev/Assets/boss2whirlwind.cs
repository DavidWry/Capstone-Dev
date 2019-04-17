using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AssemblyCSharp
{
    public class boss2whirlwind : MonoBehaviour {

        public GameObject windParticle;
        public GameObject player;
        public ParticleSystem windParticleSystem;
        public Animator bossAnimator;
        float timeCount = 0;
        Vector3 nextSpot;
        float distance = 5;
        // Use this for initialization
        void Start()
        {
            bossAnimator = gameObject.GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (player)
            {
                if (gameObject.GetComponentInParent<boss2behalf>().hp > 0)
                    if (windParticleSystem.isPlaying)
                {
                    if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position, gameObject.transform.parent.gameObject.transform.position) < distance)
                    {

                        //  player.GetComponent<Player_New>().TakeDamage(40*Time.deltaTime);

                    }
                    if (timeCount == 0)
                    {
                        nextSpot = new Vector3(0, 0, 0);

                        timeCount = 1;
                    }

                    else
                    {
                        Vector3 addition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0) * Time.deltaTime;
                        print(addition);
                        nextSpot += addition;

                        gameObject.transform.parent.GetComponent<Rigidbody2D>().MovePosition(gameObject.transform.position+nextSpot);

                    }

                    if (Vector3.Distance(gameObject.transform.parent.transform.position, player.transform.position) < distance)
                    {
                        player.GetComponent<Player_New>().TakeDamage(50 * Time.deltaTime);

                    }

                }
                else
                {
                    nextSpot = new Vector3(0, 0, 0);

                    timeCount = 0;
                }
            }
        }

        public void whrilStart()
        {
            windParticleSystem.Play();

        }

        public void whrilEnd()
        {
            windParticleSystem.Stop();

        }

        public void disableParticle()
        {
            GameObject[] respawns = GameObject.FindGameObjectsWithTag("Particle");
            foreach(GameObject respawn in respawns)
            {
                print(respawn.name);
                Destroy(respawn);
            }

                
        }
        public void destroyThis()
        {
            Destroy(gameObject.transform.parent.gameObject);

        }

        void OnTriggerEnter(Collider other)
        {
            if (windParticleSystem.isPlaying && other.tag != "Player")
            {
                nextSpot *= -1;
            }
        
        }
        private void OnTriggerStay(Collider other)
        {
            if (windParticleSystem.isPlaying && other.tag== "Player")
            {
                player.GetComponent<Player_New>().TakeDamage(25 * Time.deltaTime);
            }
        }

    }
}