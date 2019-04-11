using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.HeroEditor.Common.CharacterScripts
{
    public class detectRanged : MonoBehaviour
    {  //ranged movement
        public Character characterRanged;
        [SerializeField]
        float distance = 15;                 //range for detection        
        [SerializeField]
        float minDistance = 1.5f;            // 常数一枚,检查这个怪是否到既定的点   
        [SerializeField]
        float idleTime = 2;                  // wander既定点后停留时间
        [SerializeField]
        float Speed = 5;               //系数
        [SerializeField]
        public bool directionFlag = false;
        GameObject player;                          
        Vector3 Original;                    //original( 出范围回这个点周围的一个点，wander会变)
        Animator anim;
        bool returnOriginal = false;         //是否在返回原点路上
               //角色朝向(scale正负)

        bool wander = true;                  //wander flag
        float timeCount = 0;                 //计时
        Vector3 newvec;                      //随机向量，用于wander找下一个点
        public rotatearm Rotatearm;

        //Related to shooting.
        bool shooting = true;
        float shootingTime = 0;
        [SerializeField]
        float shootLenth = 0.5f;
        [SerializeField]
        float shootBtw = 1f;
        public bool ishit = false;
        float flashtime = 0;
        public Component[] SpriteRenderers;
        public Material mat1;
        public Material mat2;
        public Material mat3;
        public List<Color> colorlist;
        int colorcount = 0;
        // Use this for initialization
        void Start()
        {
            Original = gameObject.transform.position; //初始原点
            newvec = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0);
            anim = gameObject.GetComponentInChildren<Animator>();
            characterRanged = gameObject.GetComponent<Character>();
            player = GameObject.FindGameObjectWithTag("Player");
            SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer spt in SpriteRenderers)
                colorlist.Add(spt.color);

        }

        // Update is called once per frame
        void Update()
        {

            if (ishit)
            {
                flashtime += Time.deltaTime;



                if (flashtime < 0.2f)
                {
                    foreach (SpriteRenderer spt in SpriteRenderers)
                    {
                        spt.color = Color.white;
                        spt.material = mat1;
                    }
                }

                else
                {

                    foreach (SpriteRenderer spt in SpriteRenderers)
                    {
                        spt.color = colorlist[colorcount];
                        colorcount++;

                        if (spt.name != "Eyes")
                        {
                            spt.material = mat2;
                        }
                        else
                        {
                            spt.material = mat3;
                        }
                    }
                    colorcount = 0;
                    flashtime = 0;
                    ishit = false;

                }

            }
            if (player)
            {
            if (wander)
            {
                    Rotatearm.enabled = false;
                if (Vector3.Distance(gameObject.transform.position, Original) > minDistance)
                {  //wander时如果离原点过远(原点变化过后)，像原点前进。
                    Vector3 thisvec = (Original - gameObject.transform.position).normalized;
                    gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + thisvec * Time.deltaTime * Speed);

                }
                else
                {  //如果到达原点附近，计时

                    if (timeCount < idleTime)
                    {
                        timeCount += Time.deltaTime;
                    }
                    else
                    {
                        timeCount = 0;
                        newvec = new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0); //恢复计时 更换原点
                        Original += newvec;
                    }
                }
            }

            if (!returnOriginal)

            { //如果不在返回远点的路上(会把wander盖掉 无视)

                if (Vector3.Distance(player.transform.position, gameObject.transform.position) < distance && Vector3.Distance(gameObject.transform.position, Original) < distance)
                {
                    //如果玩家和怪的距离较劲且怪和原点距离较劲，停止wander恢复计时器
                    wander = false;
                    timeCount = 0;
                    if (!directionFlag)
                    {
                        var scale = transform.localScale;
                            var childscale = transform.Find("HCanvas").localScale;
                            
                            scale.x = Mathf.Abs(scale.x);
                            if (player.transform.position.x < transform.position.x)
                            {
                                scale.x = -15;
                                childscale.x =-0.0208f;
                            }
                            else
                            {
                                scale.x = 15;
                                childscale.x = 0.0208f;
                            }
                        transform.localScale = scale;
                            transform.Find("HCanvas").localScale=childscale;
                            //更改怪物朝向
                        }
                    Vector3 unitvec = (player.transform.position - gameObject.transform.position).normalized;//玩家到怪之间的单位向量
                    if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) > minDistance)
                    {
                            Rotatearm.enabled = true;
                        //anim.SetBool("Run", true);
                        //在怪到玩家边上之前， ，移动，打（这个是枪，因为设定的条件 所以走到玩家边上的时候也就不打了 懒得改了）
                        //gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec * Time.deltaTime * Speed);
                        if (shooting)
                        {
                                anim.SetBool("Run", false);
                                StartCoroutine(characterRanged.Firearm.Fire.Fire());
                                shootingTime += Time.deltaTime;
                        }
                            else
                            {
                                anim.SetBool("Run", true);
                                gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec * Time.deltaTime * Speed);
                                shootingTime += Time.deltaTime;
                            }
                        if (shootingTime > shootLenth && shooting)
                            {
                                shooting = false;
                                shootingTime = 0;
                            }
                        else if (shootingTime > shootBtw && !shooting)
                            {
                                shooting = true;
                                shootingTime = 0;
                            }
                    }
                }
                else
                {
                    returnOriginal = true; //距离果园返回远点flag更改
                }
            }
            else
            {  //returnoriginal flag是true,返回远点
                Vector3 unitvec = (Original - gameObject.transform.position).normalized;
                if (Vector3.Distance(gameObject.transform.position, Original) < 100000)
                {
                        Original = gameObject.transform.position;
                    //到远点了 重置变量
                    directionFlag = false;
                    anim.SetBool("Run", false);
                    returnOriginal = false;


                    wander = true;
                }
                else
                { //没到远点，往原点走

                    if (!directionFlag)
                    {
                        var scale = transform.localScale;
                        scale.x = Mathf.Abs(scale.x);

                        if (gameObject.transform.position.x > Original.x)
                                scale.x = -15;
                            else
                            scale.x = 15;

                        transform.localScale = scale;
                        directionFlag = true;
                    }
                    anim.SetBool("Run", true);
                    if (!wander)
                    {
                        gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.transform.position + unitvec * Time.deltaTime * Speed);
                    }
                }
            }

            }

        }

        void knockback(Vector3 dirVec)
        { //击退
            print(dirVec);
            gameObject.GetComponent<Rigidbody>().AddForce(-dirVec * 10000);

        }

        private void OnTriggerEnter(Collider other)
        {  //被打叫上边击退的function
            /*
            if (other.tag == "Projectile")
            {
                knockback((other.transform.position - gameObject.transform.position).normalized);
                ishit = true;
            }*/
        }
    }
}