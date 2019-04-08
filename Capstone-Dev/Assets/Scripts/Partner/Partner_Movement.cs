using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public class Partner_Movement : MonoBehaviour {

    public Character partner_Character;
    public List<SpriteRenderer> sprites;
    public GameObject player;

    float radius = 60;
    bool movingTo = true;
    float speed = 120;
    Vector3 target;
    float waitTime = 5;
    float Timer = 0;
    public bool skillReady = false;


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
            skillReady = false;
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
            if (Vector3.Distance(transform.position, player.transform.position) > radius * 3)
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
        float alpha = 0;
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a += Time.deltaTime * 3;
                if (color.a > 0.95)
                    color.a = 0.95f;
                alpha = color.a;
                sprite.color = color;
            }
        }
        if (alpha > 0.8f)
        {
            skillReady = true;
        }
    }
}
