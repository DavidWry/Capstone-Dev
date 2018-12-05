using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trackplayer : MonoBehaviour {
    float m_uptimer=0;
    float m_speed = 0;
    float m_currLife = 0;
    float m_MaxLife = 10;
    GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
		
	}

    // Update is called once per frame
 void Update()
    {
        
        if (m_uptimer < 0.5f)
        {
            m_uptimer += Time.deltaTime;
             
        
        }
        else
        {
             
            Vector3 target = (player.transform.position - transform.position).normalized;
            float a = Vector3.Angle(transform.forward, target) / 300    ;
            if (a > 0.1f || a < -0.1f)
            {
                transform.forward = Vector3.Slerp(transform.forward, target, Time.deltaTime / a).normalized;
                
            }
            else
            {
                m_speed += 15 * Time.deltaTime;
                transform.forward = Vector3.Slerp(transform.forward, target, 1).normalized;
            }
           
            transform.position += transform.forward * m_speed * Time.deltaTime;
        }

        m_currLife += Time.deltaTime;
        if (m_currLife > m_MaxLife)
        {
       
        }
    }


}
