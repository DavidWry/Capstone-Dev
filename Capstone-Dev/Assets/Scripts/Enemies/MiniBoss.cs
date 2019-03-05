using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public class MiniBoss : MonoBehaviour {

    public float HP = 100;
    float deadTime = 0;
    Character character;
    detectRanged detecter;
    public bool isalive = true;
    float stunTime = 0;

	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        if (GetComponent<detectRanged>())
        {
            detecter = GetComponent<detectRanged>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (HP <= 0)
        {
            if (isalive)
            {
                detecter.Rotatearm.enabled = false;
                detecter.enabled = false;
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

    public void Stun(float time)
    {
        detecter.Rotatearm.enabled = false;
        detecter.enabled = false;
        character.Animator.SetBool("Idel", true);
        StartCoroutine(WaitAfterStun(time));
    }

    private IEnumerator WaitAfterStun(float time)
    {
        yield return new WaitForSeconds(time);
        detecter.Rotatearm.enabled = true;
        detecter.enabled = true;
        character.Animator.SetBool("Idel", false);
    }
}
