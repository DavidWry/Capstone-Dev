using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public class Partner_Movement : MonoBehaviour {

    Character partner_Character;
    public List<SpriteRenderer> sprites;
    GameObject player;

    float radius = 100;
    bool movingTo = true;
    float speed = 120;
    Vector3 target;
    float waitTime = 3;
    float Timer = 0;


	// Use this for initialization
	void Start () {
        partner_Character = GetComponent<Character>();
        sprites = partner_Character.LayerManager.Sprites;
        player = GameObject.FindWithTag("Player");
        target = RandomPosition();
    }
	
	// Update is called once per frame
	void Update () {
		if (movingTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            partner_Character.Animator.SetBool("Run", true);
            if (Vector3.Distance(transform.position, target) < 5)
                movingTo = false;
            if (Vector3.Distance(transform.position, target) < 15)
            {
                FromTransform();
            }
            else
            {
                ToTransform();
            }
        }
        else
        {
            FromTransform();
            target = RandomPosition();
            if (Timer > waitTime)
            {
                Timer = 0;
                movingTo = true;
            }
            else
            {
                Timer += Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, player.transform.position) > radius)
            {
                Timer = 0;
                movingTo = true;
            }
            partner_Character.Animator.SetBool("Run", false);
        }
    }

    Vector3 RandomPosition()
    {
        Vector3 position = player.transform.position;
        Vector3 randomWithinCircle = Random.insideUnitCircle * radius;
        position += randomWithinCircle;
        return position;
    }

    void ToTransform()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a -= Time.deltaTime * 3;
                if (color.a < 0)
                    color.a = 0;
                sprite.color = color;
            }
        }
    }

    void FromTransform()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a += Time.deltaTime * 3;
                if (color.a > 1)
                    color.a = 1;
                sprite.color = color;
            }
        }
    }
}
