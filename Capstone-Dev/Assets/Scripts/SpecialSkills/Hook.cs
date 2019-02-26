using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public float speed;
    private Rigidbody RBody;
    public GameObject Player;
    private LineRenderer line;
    bool start = false;
    bool drag = false;
    GameObject target;
    float multi = 1;

    // Use this for initialization
    void Start () {
        RBody = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        Player = GameObject.FindWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), Player.GetComponent<Collider>());
        if (Player.GetComponent<Shoot_New>().BulletSizeUp > 5)
        {
            multi = Player.GetComponent<Shoot_New>().BulletSizeUp;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Player)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, Player.transform.position);
            if (NextScene.nowName == "2_1")
                RBody.velocity = transform.right * speed * 20;
            else
                RBody.velocity = transform.right * speed;
            if (start)
            {
                move();
            }
            else if (drag)
            {
                dragBack();
            }
            if (Vector3.Distance(Player.transform.position, transform.position) > 15 * multi)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !drag)
            start = true;
        if (collision.gameObject.tag == "Dummy" && !start)
        {
            drag = true;
            target = collision.gameObject;
        }
    }

    private void move()
    {
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, transform.position, speed * 1.5f * Time.deltaTime * multi);
        if (Vector3.Distance(Player.transform.position, transform.position) < 1.5f * multi)
        {
            Destroy(this.gameObject);
        }
    }

    private void dragBack()
    {
        if (target)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, Player.transform.position, speed * 1.5f * Time.deltaTime * multi);
            transform.position = target.transform.position;
            if (Vector3.Distance(Player.transform.position, target.transform.position) < 1.5f * multi)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
