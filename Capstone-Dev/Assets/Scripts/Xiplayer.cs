using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class Xiplayer : MonoBehaviour {
        public GameObject player1;
        Rigidbody playerbody;
        public Material mat1;
        public Material mat2;
        public ParticleSystem p1;
        int xishu = -1000;
        bool zaixiqi = false;
        bool invin = false;
        bool hitflash = false;
        float flashtime = 0;
        public GameObject spark;
        // Use this for initialization
        void Start() {
            NextScene.nowName = "2_3";
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                var emission = p1.emission;
                emission.rateOverTime = 0;
                player1 = GameObject.FindGameObjectWithTag("Player");
                playerbody = player1.GetComponent<Rigidbody>();
            }
        }

        // Update is called once per frame
        void Update() {
            if (player1)
            {
                
                Vector3 newvec = (this.transform.position - player1.transform.position);

                if (zaixiqi&& Vector3.Distance(transform.parent.transform.position, player1.transform.position)<200)
                {

                   

                    newvec = newvec.normalized;

                    playerbody.AddForce(newvec * 5.5f * Mathf.Sqrt(2) * 120);


                }
               
                if (hitflash)
                {
                    flashtime += Time.deltaTime;
                    if (flashtime < .1f)
                    {
                        gameObject.GetComponent<SpriteRenderer>().material = mat1;

                    }
                    else if (flashtime < 0.2f)
                    {
                        gameObject.GetComponent<SpriteRenderer>().material = mat2;
                    }
                    else if (flashtime < 0.3f)
                    {
                        gameObject.GetComponent<SpriteRenderer>().material = mat1;
                    }
                    else
                    {

                        gameObject.GetComponent<SpriteRenderer>().material = mat2;

                        hitflash = false;
                        flashtime = 0;


                    }
                }

            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (player1)
            {

                Vector3 newvec = (this.transform.position - player1.transform.position);
            newvec = newvec.normalized;
         
                if (other.gameObject == player1)
                {

                    player1.GetComponent<Player_New>().TakeDamage(5);
                    playerbody.AddForce(newvec * xishu*40);
                    invin = true;
                }
                if (other.gameObject.tag == "Projectile")
                {
                    hitflash = true;

                }
            }
        }
        void OnTriggerStay(Collider other)
        {
            if (player1)
            {
                Vector3 newvec = (this.transform.position - player1.transform.position);
                newvec = newvec.normalized;
                if (other.gameObject == player1)
                {


                    playerbody.AddForce(newvec * xishu*40);
                    invin = true;
                }

            }
        }


        void xi()
        {

            if (zaixiqi == false)
            {
                zaixiqi = true;
                var emission = p1.emission;
                emission.rateOverTime = 25;
                p1.Play();
            }
            else
            {

                zaixiqi = false;
                p1.Stop();
                p1.Clear();
            }


        }
    }
}