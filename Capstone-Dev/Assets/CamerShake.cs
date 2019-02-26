using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         Vector3 originalPos = transform.position;
         float elapsed = 0.0f;

         while(elapsed < duration)
         {
              float x = Random.Range(-1f, 1f) * magnitude;
              float y = Random.Range(-1f, 1f) * magnitude;


             transform.position = new Vector3(x, y, originalPos.z);
             elapsed += Time.deltaTime;

             yield return null;

         }
         transform.position = originalPos;
     }


}
