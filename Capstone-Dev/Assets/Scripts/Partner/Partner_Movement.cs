using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public class Partner_Movement : MonoBehaviour {

    public Character partner_Character;
    public List<SpriteRenderer> sprites;
    public GameObject player;
    public int MaxSkillCount = 1;
    public float CoolDownTime = 5;
    public GameObject Pop;

    public int skillNum;
    public float radius = 60;
    bool movingTo = true;
    float speed = 120;
    Vector3 target;
    float waitTime = 5f;
    float Timer = 0;
    public float skillTimer = 0;
    public bool skillReady = false;


	// Use this for initialization
	void Start () {
        partner_Character = GetComponent<Character>();
        sprites = partner_Character.LayerManager.Sprites;
        player = GameObject.FindWithTag("Player");
        target = RandomPosition();
        skillNum = MaxSkillCount;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
		if (movingTo)
        {
            skillReady = false;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            partner_Character.Animator.SetBool("Run", true);
            if (Vector3.Distance(transform.position, target) < 5)
            {
                movingTo = false;
                speed = 120;
            }
            if (Vector3.Distance(transform.position, target) < 15 && skillNum > 0)
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
            if (skillNum > 0)
                FromTransform();
            else if (skillNum == 0)
                ToTransform();
            target = RandomPosition();
            if (Timer > waitTime)
            {
                Timer = 0;
                movingTo = true;
            }
            else
            {
                Timer += Time.deltaTime;
                if (Timer < 1.0f && skillReady)
                {
                    Pop.SetActive(true);
                }
                else
                {
                    Pop.SetActive(false);
                }
            }
            if (Vector3.Distance(transform.position, player.transform.position) > radius * 3)
            {
                Timer = 0;
                movingTo = true;
            }
            if (Vector3.Distance(transform.position, player.transform.position) > radius * 5)
            {
                speed = 1200;
            }
            partner_Character.Animator.SetBool("Run", false);
        }
        skillTimer += Time.deltaTime;
        if (skillTimer > CoolDownTime)
        {
            if (skillNum < MaxSkillCount)
            {
                skillNum++;
            }
            skillTimer = 0;
        }
        if (skillNum == 0)
        {
            skillReady = false;
        }
        }

    }


    Vector3 RandomPosition()
    {
        if (player != null)
        {
            Vector3 position = player.transform.position;
            Vector3 randomWithinCircle = Random.insideUnitCircle * radius;
            position += randomWithinCircle;
            return position;
        }
        else
        {
            return transform.position;
        }
    }

    void ToTransform()
    {
        Pop.SetActive(false);
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a -= Time.deltaTime;
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
                if (color.a > 0.9)
                {
                    skillReady = true;
                }
            }
        }
    }
}
