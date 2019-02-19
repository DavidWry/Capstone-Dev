using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour {

    private int health;
    public int currentHealth;
    private DropProbability probability = null;
    private GameManager gameManager = null;
    public Image healthBar;
    public string sceneName;
    // Use this for initialization
    void Start () {
        health = 10;
        currentHealth = 10;
        if (sceneName == "2_1")
            healthBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
        probability = gameObject.GetComponent<DropProbability>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sceneName = SceneManager.GetActiveScene().name;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentHealth < 0)
        {
            string tempName = probability.DetermineDrop();
            GameObject itemObj = gameManager.GetItemObj(tempName);
            itemObj = Instantiate(gameManager.GetItemObj(tempName), transform.position, Quaternion.Euler(0, 0, 0));
            if (sceneName == "2_1") {
                if (itemObj.transform.localScale.x == 2) {//is gun
                    itemObj.transform.localScale = new Vector3(30, 30, 30);
                }
                else
                    itemObj.transform.localScale = new Vector3(60, 60, 60);
            }
            
            var worldCanvas = GameObject.Find("worldCanvas").transform;
            itemObj.transform.parent = worldCanvas;
            Destroy(gameObject);
        }
        if(sceneName=="2_1")
            healthBar.fillAmount = currentHealth / health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
