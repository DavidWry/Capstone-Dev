using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    public class Projectile : MonoBehaviour
    {

        public bool IsReady = false;
        public int Damage = 1;
        public int Rebounce = 0;
        public float Speed = 1f;
        public bool Thrust;
        public float SlowDown = 0;
        public bool Pierce = false;
        public bool Sheild = false;
        public bool Scale = false;
        public bool OnTarget = false;
        public bool Boom = false;
        public GameObject Impact;
        public GameManager GameManage;
        public float Duration = 1;
        private float LifeTime;
        public float stun = 0;
        public bool round = false;
        private float dam = 0;

        private Rigidbody RBody;
        private CapsuleCollider Collider;

        // Use this for initialization
        void Start()
        {
            RBody = GetComponent<Rigidbody>();
            Collider = GetComponent<CapsuleCollider>();
            LifeTime = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            LifeTime += Time.deltaTime;
            if (round)
            {
                dam = 270 * Time.deltaTime;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + dam);
                if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                    RBody.velocity = transform.right * Speed * 20;
                else
                    RBody.velocity = transform.right * Speed;
            }
            if (IsReady)
            {
                IsReady = false;
                if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                    RBody.velocity = transform.right * Speed * 20;
                else
                    RBody.velocity = transform.right * Speed;
            }
            if (SlowDown > 0)
            {
                Speed -= SlowDown * Time.deltaTime;
                if (Speed < 0)
                {
                    Speed = 0;
                }
                if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                    RBody.velocity = transform.right * Speed * 20;
                else
                    RBody.velocity = transform.right * Speed;
            }
            if (Scale)
            {
                transform.localScale += new Vector3(0.5f, 0.5f, 0) * Time.deltaTime;
            }
            if (LifeTime >= Duration)
            {
                Destroy(gameObject);
                if (SlowDown > 0)
                {
                    GameObject ImpactObject = Instantiate(Impact, transform.position, transform.rotation);
                    if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                        ImpactObject.transform.localScale *= 20;
                }
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Minion")
            {
                if (collision.gameObject.GetComponent<EnemySuicideBomber>())
                {
                    collision.gameObject.GetComponent<EnemySuicideBomber>().TakeDamage(Damage);
                    if (stun > 0)
                    {
                        collision.gameObject.GetComponent<EnemySuicideBomber>().Stun(stun);
                    }
                }
                else if (collision.gameObject.GetComponent<EnemyRangedSpear>())
                {
                    collision.gameObject.GetComponent<EnemyRangedSpear>().TakeDamage(Damage);
                    if (stun > 0)
                    {
                        collision.gameObject.GetComponent<EnemyRangedSpear>().Stun(stun);
                    }
                }
                else if (collision.gameObject.GetComponent<EnemyRangedStomp>())
                {
                    collision.gameObject.GetComponent<EnemyRangedStomp>().TakeDamage(Damage);
                    if (stun > 0)
                    {
                        collision.gameObject.GetComponent<EnemyRangedStomp>().Stun(stun);
                    }
                }
                else if (collision.gameObject.GetComponent<NewEnemyJumper>())
                {
                    collision.gameObject.GetComponent<NewEnemyJumper>().TakeDamage(Damage);
                    if (stun > 0)
                    {
                        collision.gameObject.GetComponent<NewEnemyJumper>().Stun(stun);
                    }
                }
                else if(collision.gameObject.GetComponent<EnemySlider>())
                {
                    collision.gameObject.GetComponent<EnemySlider>().TakeDamage(Damage);
                    if (stun > 0)
                    {
                        collision.gameObject.GetComponent<EnemySlider>().Stun(stun);
                    }

                }
                else if (collision.gameObject.GetComponent<MiniBoss>())
                {
                    collision.gameObject.GetComponent<MiniBoss>().TakeDamage(Damage);
                    if (stun > 0)
                    {
                        collision.gameObject.GetComponent<MiniBoss>().Stun(stun);
                    }

                }
                else if(Thrust)
                {
                    //collision.gameObject.GetComponent<Rigidbody>().velocity *= -1;
                }
                Dead(collision);
            }
            else if (collision.gameObject.tag == "Obstacle")
            {
                Dead(collision);
            }
            else if (collision.gameObject.tag == "Chest")
            {
                collision.GetComponent<Chest>().TakeDamage(Damage);
                Dead(collision);
            }
            else if (collision.gameObject.tag == "EnemyProjectile" && Sheild)
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Dummy")
            {
                collision.GetComponent<Dummy>().TakeDamage(Damage);
                Dead(collision);
            }
            else if (collision.gameObject.tag == "Boss")
            {
                collision.GetComponent<Fsmandhp>().takedamage(Damage);
                Dead(collision);
            }
            else if (collision.gameObject.tag == "Projectile" && Boom)
            { 
                if (!collision.GetComponent<Projectile>().Boom)
                    Dead(collision);
            }
        }

        private void Dead(Collider collision)
        {
            if (Impact != null)
            {
                if (!OnTarget)
                {
                    GameObject ImpactObject = Instantiate(Impact, transform.position, transform.rotation);
                    if (NextScene.nowName == "2_1" || NextScene.nowName == "2_2" || NextScene.nowName == "2_3" || NextScene.nowName == "3_1" || NextScene.nowName == "3_2")
                        ImpactObject.transform.localScale = ImpactObject.transform.localScale * 16;
                }
                else
                {
                    if (collision.tag == "Minion" || collision.tag == "Dummy")
                    {
                        GameObject ImpactObject = Instantiate(Impact, collision.transform.position, collision.transform.rotation);
                    }
                }
            }
            if (!Pierce)
            {
                Destroy(gameObject);
            }
        }
    }
}
