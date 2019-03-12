using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Meleehealthbar : MonoBehaviour {

    // Use this for initialization
    Image healthBar;
    public GameObject fsm;
    // Use this for initialization
    void Start()
    {
        healthBar = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (fsm.GetComponent<MeleeHP>().HP / 100.0f);
        if (healthBar.fillAmount <= 0)
        {
            Destroy(healthBar);
        }
    }
}
