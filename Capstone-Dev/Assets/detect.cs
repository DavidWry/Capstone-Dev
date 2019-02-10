using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detect : MonoBehaviour {
    float distance = 50;
    GameObject player;
    bool attacking=false;
    Vector3 Original;
    Animator anim;
    bool knock=false;
    bool returnOriginal = false;
    bool directionFlag = false;
    float minDistance = 1.5f;
    float coefficient = 5;
    bool wander=true;
    float idleTime = 2;
    float timeCount = 0;
    Vector3 newvec = new Vector3(Random.Range(-15,15),Random.Range(-15,15),0);
    // Use this for initialization
    void Start () {
        Original = gameObject.transform.position;
        
        anim = gameObject.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (wander)
        {
          
                if (Vector3.Distance(gameObject.transform.position, Original) < minDistance)
                {
                    Vector3 thisvec = (player.transform.position - gameObject.transform.position).normalized;
                    gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + thisvec * Time.deltaTime * coefficient);
               
                }
                else
            {
                 
                if (timeCount < idleTime)
                {
                    timeCount += Time.deltaTime;
                }
                else
                {
                    timeCount = 0;
                    newvec = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0);
                    Original += newvec;
                }
            }
           

        }



        if (!returnOriginal)
        
{

            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) < distance && Vector3.Distance(gameObject.transform.position, Original) < distance)
            {
                wander = false;
                timeCount = 0;
                if (!directionFlag)
                {
                    var scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x);
                    if (player.transform.position.x > transform.position.x)
                        scale.x *= -1;
                    transform.localScale = scale;
                }
                Vector3 unitvec = (player.transform.position - gameObject.transform.position).normalized;
                if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance)
                {
                    anim.SetBool("Run", true);
                    gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec*Time.deltaTime* coefficient);
                }
                else
                {
                    anim.SetBool("Run", false);
                    anim.SetTrigger("Slash");
                }
            }

            else
            {
                returnOriginal = true;

            }

        }
        else
        {
            Vector3 unitvec = (Original - gameObject.transform.position).normalized;
           
            
            if (Vector3.Distance(gameObject.transform.position, Original) < minDistance)
            {
                directionFlag = false;
                anim.SetBool("Run", false);
                returnOriginal = false;
                wander = true;
            }
            else{
                if (!directionFlag)
                {
                    var scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x);

                    if (gameObject.transform.position.x > Original.x)
                        scale.x = 1;
                    else
                        scale.x = -1;
                   
                    transform.localScale = scale;
                    directionFlag = true;
                }
                    gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec * Time.deltaTime* coefficient);
                    
                
                }

        }
    }

  void knockback(Vector3 dirVec)
    {
        print(dirVec);
        gameObject.GetComponent<Rigidbody>().AddForce(-dirVec*10000);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
           
            knockback((other.transform.position - gameObject.transform.position).normalized);
        }
    }

 
}
