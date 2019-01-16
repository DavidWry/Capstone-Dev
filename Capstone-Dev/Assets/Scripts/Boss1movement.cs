using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1movement : MonoBehaviour {
    private GameObject boss;
    private bool supposetomove1=false;
    private float range;
    Vector3 upperright= new Vector3(500, 500, 0);
    Vector3 lowerleft=new Vector3(-500,-500,0);
    Vector3 targetvec;
    bool valid = false;
    int moveref;
    int p = 0;
    float xpos;
    float ypos;
    // Use this for initialization
    void Start () {
        boss = gameObject;
        range = gameObject.GetComponent<Fsmandhp>().range;
      
	}
	
	// Update is called once per frame
	void Update () {
        
        supposetomove1 = gameObject.GetComponent<Fsmandhp>().supposetomove;
        if (supposetomove1&&!valid)
        {
             

            while (!valid)
            {

                
                xpos = Random.Range(-1.0f, 1.0f);
                ypos = Random.Range(-1.0f, 1.0f);
               
                Vector3 movevec = new Vector3(xpos,ypos,0.0f);
            

                movevec = movevec.normalized;
                
                moveref = (int)Random.Range(range * 2, range * 3);
                movevec *= moveref;
             
                
                if (movevec.x < upperright.x && movevec.x > lowerleft.x && movevec.y > lowerleft.y && movevec.y < upperright.x)
                {
                   

                    valid = true;
                    targetvec = movevec.normalized;
                    
                 
                }

            }




        





        }
        if (valid && p < moveref*2 &&supposetomove1)
        {
          
            p++;
            print(targetvec);
            gameObject.transform.parent.GetComponent<Rigidbody>().MovePosition(gameObject.transform.parent.transform.position + targetvec*0.5f);



        }
        else
        {
            if (p > 0)
            {
                gameObject.GetComponent<Fsmandhp>().originalpos = gameObject.transform.parent.transform.position;
            }
            valid = false;
            p = 0;
            gameObject.GetComponent<Fsmandhp>().supposetomove = false;
          
            
        }



    }
}
