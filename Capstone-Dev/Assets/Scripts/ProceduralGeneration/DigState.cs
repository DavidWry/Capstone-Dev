using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;
namespace AssemblyCSharp
{
    [System.Serializable]
    public class DigState
    {
        public float turnRatio = 0.05f;
        public float branchRatio = 0.05f;
        public Vector2 position = new Vector2(0, 0);
        public Vector2 direction = new Vector2(0, 0);
        // Use this for initialization
        void Start()
        {

        }


    }
}
