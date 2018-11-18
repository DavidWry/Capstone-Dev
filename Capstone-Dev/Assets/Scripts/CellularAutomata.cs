using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class CellularAutomata : MonoBehaviour {

    private GameManager gameManager;
    private int levelWidth;
    private int levelHeight;
    private int tileSize;
    private int[,] landArray;
    private int[,] newLandArray;
    private int[,] edgeArray;
    private bool isSimultaneous;
    private float landRatio;
    private int iteration;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        levelWidth=(int)Random.Range(40, 60);
        levelHeight = (int)Random.Range(40, 60);
        tileSize = 256;
        landArray = new int[levelWidth, levelHeight];
        newLandArray = new int[levelWidth, levelHeight];
        edgeArray = new int[levelWidth, levelHeight];
        isSimultaneous = false;
        landRatio = 0.5f;
        iteration = 8;

        Generate();
        Draw();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void Generate() {
        //initialize
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (Random.value < landRatio)
                    landArray[i, j] = 1;
                else
                    landArray[i, j] = 0;

                //Debug.Log(landArray[i,j]);
            }
        }
        

        //change value
        for (int i = 0; i < iteration; i++) {

            for (int w = 0; w < levelWidth; w++) {
                for (int h = 0; h < levelHeight; h++) {
                  
                        if (!isSimultaneous)
                            landArray[w, h] = DetermineCell(w,h,1);
                        else
                            newLandArray[w, h] = DetermineCell(w, h, 1);
                }
            }
            if (isSimultaneous) {
                for (int w = 0; w < levelWidth; w++) {
                    for (int h = 0; h < levelHeight; h++) {
                        landArray[w, h] = newLandArray[w, h];
                    }
                }
            }


        }
    }

    int DetermineCell(int row, int col, int targetCellNum) {
        int found = 0;
        int cellNum = 0;

        //calculate four corners
        if (row == 0 && col == 0)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int y = 0; y <= 1; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 2)
                cellNum = 1;
        }
        else if (row == 0 && col == levelHeight - 1)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 2)
                cellNum = 1;
        }

        else if (row == levelWidth - 1 && col == 0)
        {
            for (int x = -1; x <= 0; x++)
            {
                for (int y = 0; y <= 1; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 2)
                cellNum = 1;
        }

        else if (row == levelWidth - 1 && col == levelHeight - 1)
        {
            for (int x = -1; x <= 0; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 2)
                cellNum = 1;
        }
        //calculate four edges
        else if (row == 0 && col > 0 && col < levelHeight - 1)
        {
            for (int x = 0; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 3)
                cellNum = 1;
        }
        else if (row == levelWidth - 1 && col > 0 && col < levelHeight - 1)
        {
            for (int x = -1; x <= 0; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 3)
                cellNum = 1;
        }
        else if (row > 0 && row < levelWidth - 1 && col == 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = 0; y <= 1; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 3)
                cellNum = 1;
        }
        else if (row > 0 && row < levelWidth - 1 && col == levelHeight - 1)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 0; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            if (found > 3)
                cellNum = 1;
        }
        //calculate main area
        else
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (landArray[row + x, col + y] == targetCellNum)
                        found++;
                }
            }
            //Debug.Log("found"+found);
            if (found > 4)
                cellNum=1;
            //Debug.Log(cellNum);
        }
        
        return cellNum;
        
    }

    void Draw() {

        GameObject tile0 =gameManager.GetTile("Tile_0");
        GameObject tile1 = gameManager.GetTile("Tile_1");


        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (landArray[i, j] == 0)
                {
                    Instantiate(tile0,new Vector3(i*(float)tileSize/100,j* (float)tileSize /100,0), transform.rotation);
                }
                else
                {
                    Instantiate(tile1, new Vector3(i * (float)tileSize /100, j * (float)tileSize /100, 0), transform.rotation);
                }
            }
        }
    }

    void ChangeEdge() {
        for (int row = 0; row < levelWidth; row++)
        {
            for (int col = 0; col < levelHeight; col++)
            {
                //calculate four corners
                if (row == 0 && col == 0)
                {
                    for (int x = 0; x <= 1; x++)
                    {
                        for (int y = 0; y <= 1; y++)
                        {
                           
                        }
                    }
                   
                }
                else if (row == 0 && col == levelHeight - 1)
                {
                    for (int x = 0; x <= 1; x++)
                    {
                        for (int y = -1; y <= 0; y++)
                        {
                            ;
                        }
                    }
                   
                }

                else if (row == levelWidth - 1 && col == 0)
                {
                    for (int x = -1; x <= 0; x++)
                    {
                        for (int y = 0; y <= 1; y++)
                        {
                           
                        }
                    }
                   
                }

                else if (row == levelWidth - 1 && col == levelHeight - 1)
                {
                    for (int x = -1; x <= 0; x++)
                    {
                        for (int y = -1; y <= 0; y++)
                        {
                            
                        }
                    }
                   
                }
                //calculate four edges
                else if (row == 0 && col > 0 && col < levelHeight - 1)
                {
                    for (int x = 0; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            
                        }
                    }
                   
                }
                else if (row == levelWidth - 1 && col > 0 && col < levelHeight - 1)
                {
                    for (int x = -1; x <= 0; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                           
                        }
                    }
                   
                }
                else if (row > 0 && row < levelWidth - 1 && col == 0)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = 0; y <= 1; y++)
                        {

                        }
                    }
                   
                }
                else if (row > 0 && row < levelWidth - 1 && col == levelHeight - 1)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 0; y++)
                        {
                           
                        }
                    }
                  
                }
                //calculate main area
                else
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                          
                        }
                    }
                    //Debug.Log("found"+found);
                    
                    //Debug.Log(cellNum);
                }
            }//end for col
        }//end for row
    }//end Changeedge
}
