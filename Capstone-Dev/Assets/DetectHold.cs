using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHold : MonoBehaviour
{

    [SerializeField]
    float distance = 50;
    [SerializeField]
    float minDistance = 1.5f;
    [SerializeField]
    float coefficient = 5;
    [SerializeField]
    float idleTime = 2;

    GameObject player;
    bool attacking = false;
    Vector3 Original;
    Animator anim;
    bool knock = false;
    bool returnOriginal = false;
    bool directionFlag = false;
    public Component[] SpriteRenderers;
    public Material mat1;
    public Material mat2;
    bool wander = true;
    float flashtime = 0;
    float timeCount = 0;
    Vector3 newvec;
    public bool ishit = false;
    // Use this for initialization
    void Start()
    {
        Original = gameObject.transform.position;
        newvec = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0);
        anim = gameObject.GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spt in SpriteRenderers)
            spt.color = Color.white;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ishit)
        {
            flashtime += Time.deltaTime;
           

                
                if (flashtime < 0.2f)
                {
                foreach (SpriteRenderer spt in SpriteRenderers)
                {
                    spt.material = mat1;
                }
            }
               
                else
                {

                foreach (SpriteRenderer spt in SpriteRenderers)
                {
                    spt.material = mat2;
                }

                flashtime = 0;
                    ishit = false;

                }

            }
        
        if (player)
        {
            if (wander)
            {
                if (Vector3.Distance(gameObject.transform.position, Original) > minDistance)
                {
                    Vector3 thisvec = (Original - gameObject.transform.position).normalized;
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
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SlashMeleePairedMod"))
                        {

                         
                            // Avoid any reload.
                       
                        anim.SetBool("Run", true);
                             
                            gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec * Time.deltaTime * coefficient);
                           
                        }
                        else
                        {
                            anim.ResetTrigger("Slashmod");
                        }
                    }
                    else
                    {
                        anim.SetBool("Run", false);
                        anim.SetTrigger("Slashmod");

                        
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
                else
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SlashMeleePairedMod"))
                    {
                        if (!directionFlag)
                        {
                            var scale = transform.localScale;
                            scale.x = Mathf.Abs(scale.x);

                            if (gameObject.transform.position.x > Original.x)
                                scale.x *= 1;
                            else
                                scale.x *= -1;

                            transform.localScale = scale;
                            directionFlag = true;
                        }

                        anim.SetBool("Run", true);

                        if (!wander)
                        {


                            gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec * Time.deltaTime * coefficient);
                        }

                    }
                }
            }

        }

    }

    void knockback(Vector3 dirVec)
    {
     
        gameObject.GetComponent<Rigidbody>().AddForce(-dirVec * 10000);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {

            knockback((other.transform.position - gameObject.transform.position).normalized);
            ishit = true;
           

        }

        

    }
}
