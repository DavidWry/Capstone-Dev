using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CamerShake : MonoBehaviour
{
    //Transform m_camera;

     private void Start()
     {
        // m_camera = Camera.main.transform;

     }
     private void Update()
     {

     }

     public IEnumerator Shake(float duration, float magnitude)
     {
        Transform ori = FindObjectOfType<Player_New>().gameObject.transform;

         //Vector3 ori = transform.position;
         float elapsed = 0.0f;

         while(elapsed <= duration)
         {
            Vector2 rand = Random.insideUnitCircle;
            float x = rand.x * magnitude * (1f - (elapsed / duration));//Random.Range(-1f, 1f) * magnitude;
             float y = rand.y * magnitude * (1f - (elapsed / duration));//Random.Range(-1f, 1f) * magnitude;


             transform.position = new Vector3(ori.position.x + x, ori.position.y + y, transform.position.z);
             elapsed += Time.deltaTime;

             yield return null;

         }
         transform.position = new Vector3(ori.position.x, ori.position.y, transform.position.z);
     }


}
