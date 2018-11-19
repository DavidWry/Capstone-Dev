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
    private int[,] cellState;
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
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                landArray[i, j] = 1;//for generate
            }
        }
        newLandArray = new int[levelWidth, levelHeight];
        edgeArray = new int[levelWidth, levelHeight];
        cellState = new int[levelWidth, levelHeight];
        isSimultaneous = false;
        landRatio = 0.5f;
        iteration = 8;

        //generate up to 2 level platforms
        for (int i = 0; i < 2; i++)
        {
            Generate();
            Draw(i);
            ChangeEdge();
            DrawEdge();
        }
    }
	
    void Generate() {
        //initialize
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (landArray[i, j] == 1)
                {
                    if (Random.value < landRatio)
                        landArray[i, j] = 1;
                    else
                        landArray[i, j] = 0;
                }
                //Debug.Log(landArray[i,j]);
            }
        }
        

        //change value
        for (int i = 0; i < iteration; i++) {

            for (int w = 0; w < levelWidth; w++) {
                for (int h = 0; h < levelHeight; h++) {

                    if (!isSimultaneous)
                    {
                        landArray[w, h] = DetermineCell(w, h, 1);
                    }
                    else
                    {
                        newLandArray[w, h] = DetermineCell(w, h, 1);
                    }
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

    void Draw(int iteration) {
        if (iteration == 0)//no need to redraw tile_5 when draw second level
        {
            GameObject tile0 = gameManager.GetTile("Tile_0");
            GameObject tile5 = gameManager.GetTile("Tile_5");


            for (int i = 0; i < levelWidth; i++)
            {
                for (int j = 0; j < levelHeight; j++)
                {
                    if (landArray[i, j] == 0)
                    {
                        Instantiate(tile5, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 5;
                    }
                    else
                    {
                        Instantiate(tile5, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 5;
                    }
                }
            }
        }
    }

    void ChangeEdge() {
        for (int row = 0; row < levelWidth; row++)
        {
            for (int col = 0; col < levelHeight; col++)
            {
                if (landArray[row, col] == 1)
                {
                    //calculate four corners
                    if (row == 0 && col == 0)
                    {
                        int bitMask = 32 * landArray[row + 1, col] + 16 * landArray[row, col + 1] + 1 * landArray[row + 1, col + 1];
                        edgeArray[row, col] = bitMask;
                    }
                    else if (row == 0 && col == levelHeight - 1)
                    {
                        int bitMask = 32 * landArray[row + 1, col] + 64 * landArray[row, col - 1] + 2 * landArray[row + 1, col - 1];
                        edgeArray[row, col] = bitMask;
                    }

                    else if (row == levelWidth - 1 && col == 0)
                    {
                        int bitMask = 128 * landArray[row - 1, col] + 8 * landArray[row - 1, col + 1] + 16 * landArray[row, col + 1];
                        edgeArray[row, col] = bitMask;
                    }

                    else if (row == levelWidth - 1 && col == levelHeight - 1)
                    {
                        int bitMask = 128 * landArray[row - 1, col] + 4 * landArray[row - 1, col - 1] + 64 * landArray[row, col - 1];
                        edgeArray[row, col] = bitMask;
                    }
                    //calculate four edges
                    else if (row == 0 && col > 0 && col < levelHeight - 1)
                    {
                        int bitMask = 16 * landArray[row, col + 1] + 1 * landArray[row + 1, col + 1] +
                            64 * landArray[row, col - 1] + 2 * landArray[row + 1, col - 1]
                            + 32 * landArray[row + 1, col];
                        edgeArray[row, col] = bitMask;
                    }
                    else if (row == levelWidth - 1 && col > 0 && col < levelHeight - 1)
                    {
                        int bitMask = 16 * landArray[row, col + 1] + 8 * landArray[row - 1, col + 1] +
                             64 * landArray[row, col - 1] + 4 * landArray[row - 1, col - 1]
                             + 128 * landArray[row - 1, col];
                        edgeArray[row, col] = bitMask;
                    }
                    else if (row > 0 && row < levelWidth - 1 && col == 0)
                    {
                        int bitMask = 32 * landArray[row + 1, col] + 128 * landArray[row - 1, col] +
                             8 * landArray[row-1, col + 1] + 16 * landArray[row, col + 1] + 1 * landArray[row + 1, col + 1];
                        edgeArray[row, col] = bitMask;
                    }
                    else if (row > 0 && row < levelWidth - 1 && col == levelHeight - 1)
                    {
                        int bitMask = 32 * landArray[row + 1, col] + 128 * landArray[row - 1, col] +
                            4 * landArray[row - 1, col - 1] + 64 * landArray[row, col - 1] + 2 * landArray[row + 1, col - 1];
                        edgeArray[row, col] = bitMask;
                    }
                    //calculate main area
                    else
                    {
                        int bitMask = 32 * landArray[row + 1, col] + 128 * landArray[row - 1, col] +
                             4 * landArray[row - 1, col - 1] + 64 * landArray[row, col - 1] + 2 * landArray[row + 1, col - 1] +
                             8 * landArray[row - 1, col + 1] + 16 * landArray[row, col + 1] + 1 * landArray[row + 1, col + 1];
                        edgeArray[row, col] = bitMask;
                    }
                }//end if platform
            }//end for col
        }//end for row
    }//end Changeedge

    void DrawEdge() {
        GameObject tile1 = gameManager.GetTile("Tile_1");
        GameObject tile2 = gameManager.GetTile("Tile_2");
        GameObject tile3 = gameManager.GetTile("Tile_3");
        GameObject tile4 = gameManager.GetTile("Tile_4");
        GameObject tile5 = gameManager.GetTile("Tile_5");
        GameObject tile6 = gameManager.GetTile("Tile_6");
        GameObject tile7 = gameManager.GetTile("Tile_7");
        GameObject tile8 = gameManager.GetTile("Tile_8");
        GameObject tile9 = gameManager.GetTile("Tile_9");
        GameObject tile10 = gameManager.GetTile("Tile_10");
        GameObject tile11 = gameManager.GetTile("Tile_11");
        GameObject tile12 = gameManager.GetTile("Tile_12");
        GameObject tile13 = gameManager.GetTile("Tile_13");
        GameObject tile14 = gameManager.GetTile("Tile_14");
        GameObject tile15 = gameManager.GetTile("Tile_15");
        GameObject tile16 = gameManager.GetTile("Tile_16");
        GameObject tile17 = gameManager.GetTile("Tile_17");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                //left-top corner
                if (edgeArray[i, j]/96==1 && 
                    (edgeArray[i, j] % 96 == 2 || edgeArray[i, j] % 96 == 3 ||
                    edgeArray[i, j] % 96 == 6 || edgeArray[i, j] % 96 == 7 ||
                    edgeArray[i, j] % 96 == 10 || edgeArray[i, j] % 96 == 11 ||
                    edgeArray[i, j] % 96 == 14 || edgeArray[i, j] % 96 == 15
                    ))
                {
                    
                    Instantiate(tile1, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 1;
                    landArray[i, j] = 0;
                }

                //right-top corner
                else if (edgeArray[i, j] / 192 == 1 &&
                    (edgeArray[i, j] % 192 == 4 || edgeArray[i, j] % 192 == 5 ||
                    edgeArray[i, j] % 192 == 6 || edgeArray[i, j] % 192 == 7 ||
                    edgeArray[i, j] % 192 == 12 || edgeArray[i, j] % 192 == 13 ||
                    edgeArray[i, j] % 192 == 14 || edgeArray[i, j] % 192 == 15
                    ))
                {
                   
                    Instantiate(tile3, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 3;
                    landArray[i, j] = 0;
                }

                //left-bot corner
                else if (edgeArray[i, j] / 48 == 1 &&
                    (edgeArray[i, j] % 48 == 1 || edgeArray[i, j] % 48 == 3 ||
                    edgeArray[i, j] % 48 == 5 || edgeArray[i, j] % 48 == 7 ||
                    edgeArray[i, j] % 48 == 9 || edgeArray[i, j] % 48 == 11 ||
                    edgeArray[i, j] % 48 == 13 || edgeArray[i, j] % 48 == 15
                    ))
                {
                   
                    Instantiate(tile7, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 7;
                    landArray[i, j] = 0;
                }

                //right-bot corner
                else if (edgeArray[i, j] / 144 == 1 &&
                    (edgeArray[i, j] % 144 == 8 || edgeArray[i, j] % 144 == 9 ||
                    edgeArray[i, j] % 144 == 10 || edgeArray[i, j] % 144 == 11 ||
                    edgeArray[i, j] % 144 == 12 || edgeArray[i, j] % 144 == 13 ||
                    edgeArray[i, j] % 144 == 14 || edgeArray[i, j] % 144 == 15
                    ))
                {
                    
                    Instantiate(tile9, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 9;
                    landArray[i, j] = 0;
                }

                //top
                else if (edgeArray[i, j] / 224 == 1 && edgeArray[i, j] % 224>= 0 && edgeArray[i, j] % 224 <= 15)
                {

                    //open the gate!

                    if (Random.value < 0.1 && j != levelHeight - 1)
                    {
                        Instantiate(tile17, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 17;
                        landArray[i, j] = 0;
                    }
                    else
                    {
                        Instantiate(tile2, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 2;
                        landArray[i, j] = 0;
                    }

                }

                //down
                else if (edgeArray[i, j] / 176 == 1 && edgeArray[i, j] % 176 >= 0 && edgeArray[i, j] % 176 <= 15)
                {

                    //open the gate!
                    if (Random.value < 0.1 && j != 0)
                    {
                        Instantiate(tile16, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 16;
                        landArray[i, j] = 0;
                    }

                    else
                    {
                        Instantiate(tile8, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 8;
                        landArray[i, j] = 0;
                    }
                }

                //left
                else if (edgeArray[i, j] / 112 == 1 && edgeArray[i, j] % 112 >= 0 && edgeArray[i, j] % 112 <= 15)
                {

                    //open the gate!
                    if (Random.value < 0.1 && i != 0)
                    {
                        Instantiate(tile14, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 14;
                        landArray[i, j] = 0;
                    }
                    else
                    {
                        Instantiate(tile4, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 4;
                        landArray[i, j] = 0;
                    }

                }

                //right
                else if (edgeArray[i, j] / 208 == 1 && edgeArray[i, j] % 208 >= 0 && edgeArray[i, j] % 208 <= 15)
                {

                    //open the gate!
                    if (Random.value < 0.1 && i != levelWidth - 1)
                    {
                        Instantiate(tile15, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 15;
                        landArray[i, j] = 0;
                    }

                    else
                    {
                        Instantiate(tile6, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                        cellState[i, j] = 6;
                        landArray[i, j] = 0;
                    }
                        
                }

                //link left and top
                else if (edgeArray[i, j] / 240 == 1 && edgeArray[i, j] % 240 >= 0 && edgeArray[i, j] % 240 <= 7)
                {
                  
                    Instantiate(tile13, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 13;
                    landArray[i, j] = 0;
                }

                //link right and top
                else if (edgeArray[i, j] / 240 == 1 && 
                    (edgeArray[i, j] % 240 == 0 || edgeArray[i, j] % 240 == 2 ||
                    edgeArray[i, j] % 240 == 4 || edgeArray[i, j] % 240 == 6 ||
                    edgeArray[i, j] % 240 == 8 || edgeArray[i, j] % 240 == 10 ||
                    edgeArray[i, j] % 240 == 12 || edgeArray[i, j] % 240 == 14
                    ))
                {
                  
                    Instantiate(tile12, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 12;
                    landArray[i, j] = 0;
                }

                //link right and bot
                else if (edgeArray[i, j] / 240 == 1 &&
                    (edgeArray[i, j] % 240 == 0 || edgeArray[i, j] % 240 == 1 ||
                    edgeArray[i, j] % 240 == 4 || edgeArray[i, j] % 240 == 5 ||
                    edgeArray[i, j] % 240 == 8 || edgeArray[i, j] % 240 == 9 ||
                    edgeArray[i, j] % 240 == 12 || edgeArray[i, j] % 240 == 13
                    ))
                {
                   
                    Instantiate(tile10, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 10;
                    landArray[i, j] = 0;
                }

                //link left and bot
                else if (edgeArray[i, j] / 240 == 1 &&
                    (edgeArray[i, j] % 240 == 0 || edgeArray[i, j] % 240 == 1 ||
                    edgeArray[i, j] % 240 == 2 || edgeArray[i, j] % 240 == 3 ||
                    edgeArray[i, j] % 240 == 8 || edgeArray[i, j] % 240 == 9 ||
                    edgeArray[i, j] % 240 == 10 || edgeArray[i, j] % 240 == 11
                    ))
                {
               
                    Instantiate(tile11, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, -0.1f), transform.rotation);
                    cellState[i, j] = 11;
                    landArray[i, j] = 0;
                }
            }
        }
    }


}
