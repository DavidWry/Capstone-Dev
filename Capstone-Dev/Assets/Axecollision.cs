using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

namespace AssemblyCSharp
{
    public class Axecollision : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
         
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player"&& gameObject.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.GetComponent<slashing>().slashin)
            {



                 

                Vector3 newvec = -(gameObject.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.position - other.transform.position);

                
                newvec = newvec.normalized;

                other.GetComponent<Rigidbody>().AddForce(newvec * 5.5f * Mathf.Sqrt(2) * 8000);
                other.GetComponent<Player_New>().TakeDamage(15);
    







        }
        }
    }
}