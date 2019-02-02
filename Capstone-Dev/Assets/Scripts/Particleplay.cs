using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class Particleplay : MonoBehaviour
    {
        GameObject Player1;
        bool currentactive = false;
        float p = 0;
        // Use this for initialization
        void Start()
        {
            Player1 = GameObject.FindWithTag("Player");
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {

            print(Player1.GetComponent<Movement_New>().WalkSpeed);
            float distanceabc = Vector3.Distance(Player1.transform.position, gameObject.transform.position);

            if (distanceabc < 1&&!currentactive)
            {
                currentactive = true;
                Player1.GetComponent<Movement_New>().WalkSpeed-=2;
              
            }
            if (currentactive)
            {
                
                
              
                    Player1.GetComponent<Player_New>().TakeDamage(5.0f*Time.deltaTime);
             

                if (distanceabc > 1)
                {
                    currentactive = false;
                    Player1.GetComponent<Movement_New>().WalkSpeed += 2;
                }
            
            else if (this.gameObject.GetComponent<ParticleSystem>().isStopped)
            {
                p = 0;
                Player1.GetComponent<Movement_New>().WalkSpeed += 2;
                Destroy(this.gameObject);

            }
            }
        }


   
    }
}