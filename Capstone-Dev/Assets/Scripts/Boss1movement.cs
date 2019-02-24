using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1movement : MonoBehaviour
{
    private GameObject boss;
    private bool supposetomove1 = false;
    private float range;
    Vector3 upperright = new Vector3(100, 140, 0);
    Vector3 lowerleft = new Vector3(0, 0, 0);
    Vector3 targetvec;
    bool valid = false;
    int moveref;
    int p = 0;
    float xpos;
    float ypos;
    // Use this for initialization
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {

    }
}
