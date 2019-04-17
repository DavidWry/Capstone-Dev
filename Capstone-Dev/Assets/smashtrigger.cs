using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class smashtrigger : MonoBehaviour
    {
        Animator anim;
        GameObject player;
        bool stab;
        bool slam;
        // Use this for initialization
        void Start()
        {
            anim = gameObject.transform.parent.GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            stab = this.gameObject.transform.parent.GetComponent<boss2stab>().stab1;
            slam = this.gameObject.transform.parent.GetComponent<boss2slam>().slam1;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "row5"&&stab)
            {
                player.GetComponent<Player_New>().TakeDamage(30);
            }
            else if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "row4"&&slam)
            {
                player.GetComponent<Player_New>().TakeDamage(50);
            }

        }


    }
}