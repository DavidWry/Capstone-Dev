using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class Xiplayer : MonoBehaviour {
        public GameObject player1;
        Rigidbody playerbody;
        int xishu = -18000;
        bool zaixiqi = false;
        bool invin = false;
        public GameObject spark;
        // Use this for initialization
        void Start() {
            player1 = GameObject.FindGameObjectWithTag("Player");
            playerbody = player1.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update() {
            Vector3 newvec = (this.transform.position - player1.transform.position);

            if (zaixiqi)
            {



                newvec = newvec.normalized;

                playerbody.AddForce(newvec * 5.5f * Mathf.Sqrt(2)*90);

            }



        }
        void OnTriggerEnter(Collider other)
        {


            Vector3 newvec = (this.transform.position - player1.transform.position);
            newvec = newvec.normalized;
            if (other.gameObject == player1)
            {

                player1.GetComponent<Player>().TakeDamage(5);
                playerbody.AddForce(newvec * xishu);
                invin = true;
            }

        }
        void OnTriggerStay(Collider other)
        {
            Vector3 newvec = (this.transform.position - player1.transform.position);
            newvec = newvec.normalized;
            if (other.gameObject == player1)
            {


                playerbody.AddForce(newvec * xishu);
                invin = true;
            }


        }


        void xi()
        {

            if (zaixiqi == false)
            {
                zaixiqi = true;
            }
            else
            {

                zaixiqi = false;
            }


        }
    }
}