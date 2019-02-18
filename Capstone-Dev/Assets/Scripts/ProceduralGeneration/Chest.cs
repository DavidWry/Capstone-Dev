using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour {

    private int health;
    public int currentHealth;
    private DropProbability probability = null;
    private GameManager gameManager = null;
    public Image healthBar;
    // Use this for initialization
    void Start () {
        health = 10;
        currentHealth = 10;
        healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentHealth < 0)
        {
            string tempName = probability.DetermineDrop();
            GameObject itemObj = gameManager.GetItemObj(tempName);
            itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
            itemObj.transform.localScale = new Vector3(30, 30, 30);
            var worldCanvas = GameObject.Find("worldCanvas").transform;
            itemObj.transform.parent = worldCanvas;
            Destroy(gameObject);
        }
        healthBar.fillAmount = currentHealth / health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
