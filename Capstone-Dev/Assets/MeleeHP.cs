using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
public class MeleeHP : MonoBehaviour {

    public float HP = 100;
    float deadTime = 0;
    Character character;
    public bool isalive = true;
    float stunTime = 0;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<Character>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            if (isalive)
            {
                character.Animator.SetBool("Run", false);
                character.Animator.SetBool("DieFront", true);
            }
            isalive = false;
            deadTime += Time.deltaTime;
        }
        if (deadTime > 1f)
            Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }
    
    
}
