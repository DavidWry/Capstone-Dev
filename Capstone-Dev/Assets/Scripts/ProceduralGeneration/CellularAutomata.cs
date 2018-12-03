using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class CellularAutomata : MonoBehaviour {

    private GameManager gameManager;
    private int levelWidth;
    private int levelHeight;
    private float tileSize;
    private int[,] landArray;
    private int[,] newLandArray;
    private int[,] edgeArray;
    private int[,] cellState;
    private bool isSimultaneous;
    private float landRatio;
    private float treeRatio;
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
        treeRatio = 0.5f;
        iteration = 8;

        //generate up to 2 level platforms
        for (int i = 0; i < 3; i++)
        {
            Generate(i);
            Draw(i);
            ChangeEdge();
            DrawEdge();
        }

        GenerateTrees();
        DrawTrees();

        GenerateCactus();
        DrawCactus();

        GenerateGrass();
        DrawGrass();

        GenerateShrub();
        DrawShrub();

        GenerateRock();
        DrawRock();

        GenerateLoot();
        DrawLoot();

        GenerateEnemy();
        DrawEnemy();

        DrawPortal();

       // DrawPlayer();
    }

    void CA(float ratio, int iteration,int threshold,int neighborSize, bool isSimultaneous, int targetNum) {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {
                    if (Random.value < ratio)
                        landArray[i, j] = 1;
                    else
                        landArray[i, j] = 0;
                }
            }
        }

        //change value in landArray
        for (int i = 0; i < iteration; i++)
        {
            for (int w = 0; w < levelWidth; w++)
            {
                for (int h = 0; h < levelHeight; h++)
                {

                    if (!isSimultaneous)
                    {
                        landArray[w, h] = DetermineCell(w, h, targetNum,threshold,neighborSize);
                    }
                    else
                    {
                        newLandArray[w, h] = DetermineCell(w, h, targetNum, threshold, neighborSize);
                    }
                }
            }
            if (isSimultaneous)
            {
                for (int w = 0; w < levelWidth; w++)
                {
                    for (int h = 0; h < levelHeight; h++)
                    {
                        landArray[w, h] = newLandArray[w, h];
                    }
                }
            }


        }

    }

    int DetermineEnemyType() {
        int enemyNum = 0;
        if (Random.value < 0.4f)
        {
            enemyNum = 100;
        }
        else if (Random.value < 0.6f) {
            enemyNum = 101;//This type will always drop ultimate resources
        }
        else if (Random.value < 1.0f)
        {
            enemyNum = 102;
        }
        return enemyNum;
    }

    void Seed(int xPos,int yPos, int size, float ratio) {
        bool isReady = false;
        
        for (int i = xPos - size; i <= xPos + size; i++) {
            for (int j = yPos - size; j <= yPos + size; j++) {
                if (i < 0 || i > levelWidth - 1 || j < 0 || j > levelHeight - 1)
                    continue;
                if (Random.value < ratio) {
                    if (cellState[i, j] == 5)
                    {
                        if (isReady) {
                            cellState[i, j] = DetermineEnemyType();
                            isReady = false;
                        }
                        else if(Random.value<ratio)
                            cellState[i, j] = DetermineEnemyType();
                    }
                    else
                    {
                        isReady = true;
                    }
                }
                    
            }
        }



    }
    void Generate(int currentLevel) {
        CA(0.5f, 8, 4, 1,false,1);
    }

    int DetermineCell(int row, int col, int targetCellNum, int threshold, int neighborSize) {
        int found = 0;
        int cellNum = 0;
/*
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
        */
        //calculate main area
        int minX = row-neighborSize;
        int maxX = row+neighborSize;
        int minY = col-neighborSize;
        int maxY = col+neighborSize;
        if (minX < 0)
            minX = 0;
        if (maxX > levelWidth - 1)
            maxX = levelWidth - 1;
        if (minY < 0)
            minY = 0;
        if (maxY > levelHeight - 1)
            maxY = levelHeight - 1;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (landArray[x, y] == targetCellNum)
                found++;
            }
        }
            //Debug.Log("found"+found);
        if (found > threshold)
            cellNum=1;
            //Debug.Log(cellNum);
        
        return cellNum;
        
    }

    void Draw(int currentLevel) {
        if (currentLevel == 0)//no need to redraw tile_5 when draw second level
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
                             8 * landArray[row - 1, col + 1] + 16 * landArray[row, col + 1] + 1 * landArray[row + 1, col + 1];
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

                else {
                    edgeArray[row, col] = 0;
                }
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
                    
                    Instantiate(tile1, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                   
                    Instantiate(tile3, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                   
                    Instantiate(tile7, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                    
                    Instantiate(tile9, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    cellState[i, j] = 9;
                    landArray[i, j] = 0;
                }

                //top
                else if (edgeArray[i, j] / 224 == 1 && edgeArray[i, j] % 224>= 0 && edgeArray[i, j] % 224 <= 15)
                {

                    //open the gate!

                    if (Random.value < 0.1 && j != levelHeight - 1)
                    {
                        Instantiate(tile17, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 17;
                        landArray[i, j] = 0;
                    }
                    else
                    {
                        Instantiate(tile2, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                        Instantiate(tile16, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 16;
                        landArray[i, j] = 0;
                    }

                    else
                    {
                        Instantiate(tile8, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                        Instantiate(tile14, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 14;
                        landArray[i, j] = 0;
                    }
                    else
                    {
                        Instantiate(tile4, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                        Instantiate(tile15, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 15;
                        landArray[i, j] = 0;
                    }

                    else
                    {
                        Instantiate(tile6, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 6;
                        landArray[i, j] = 0;
                    }
                        
                }

                //link left and top
                else if (edgeArray[i, j] / 240 == 1 && edgeArray[i, j] % 240 >= 0 && edgeArray[i, j] % 240 <= 7)
                {
                  
                    Instantiate(tile13, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                  
                    Instantiate(tile12, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
                   
                    Instantiate(tile10, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
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
               
                    Instantiate(tile11, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    cellState[i, j] = 11;
                    landArray[i, j] = 0;
                }
            }
        }
    }

    void GenerateTrees() {
        //initialize
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (cellState[i, j] == 5)
                {
                    landArray[i, j] = 1;//draw trees on blank tiles
                }
                else {
                    landArray[i, j] = -1;//do not generate trees on edge
                }
                //Debug.Log(landArray[i, j]);
            }
        }

        CA(0.5f, 1, 7, 1,true,1);
      

    }

    void DrawTrees()
    {
        GameObject tree1 = gameManager.GetTile("Tree_1");
        GameObject tree2 = gameManager.GetTile("Tree_2");
        GameObject tree3 = gameManager.GetTile("Tree_3");
        GameObject tree4 = gameManager.GetTile("Tree_4");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {
                    Debug.Log("shu");
                    float tempValue = Random.value;
                    if (tempValue < 0.25)
                    {
                        Instantiate(tree1, new Vector3((i+Random.Range(-0.5f,0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 18;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tree2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 19;
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(tree3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 20;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tree4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 21;
                    }
                }
            }
        }
    }

    void GenerateCactus()
    {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 5|| (cellState[i, j]>=18 && cellState[i, j] <= 21))
                {
                    landArray[i, j] = 1;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        //CA(0.5f, 1, 8, 1, true, 0);
        CA(0.5f, 1, 8, 1, false, 0);
    }

    void DrawCactus()
    {
        GameObject cactus1 = gameManager.GetTile("Cactus_1");
        GameObject cactus2 = gameManager.GetTile("Cactus_2");
        GameObject cactus3 = gameManager.GetTile("Cactus_3");
        GameObject cactus4 = gameManager.GetTile("Cactus_4");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {
                
                    float tempValue = Random.value;
                    if (tempValue < 0.25)
                    {
                        Instantiate(cactus1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 22;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(cactus2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 23;
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(cactus3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 24;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(cactus4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 25;
                    }
                }
            }
        }
    }

    void GenerateGrass()
    {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 5 || (cellState[i, j] >= 18 && cellState[i, j] <= 25))
                {
                    landArray[i, j] = 1;
                }
                else
                {
                    landArray[i, j] = -1;
                }
               
            }
        }

        //CA(0.5f, 1, 8, 1, true, 0);
        CA(0.5f, 1, 8, 1, false, 0);
    }

    void DrawGrass()
    {
        GameObject grass1 = gameManager.GetTile("Grass_1");
        GameObject grass2 = gameManager.GetTile("Grass_2");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.5)
                    {
                        Instantiate(grass1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 26;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(grass2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 27;
                    }
                }
            }
        }
    }

    void GenerateShrub()
    {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 5 || (cellState[i, j] >= 18 && cellState[i, j] <= 27))
                {
                    landArray[i, j] = 1;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        //CA(0.5f, 1, 8, 1, true, 0);
        CA(0.5f, 1, 8, 1, false, 0);
    }

    void DrawShrub()
    {
        GameObject shrub1 = gameManager.GetTile("Shrub_1");
        GameObject shrub2 = gameManager.GetTile("Shrub_2");
        GameObject shrub3 = gameManager.GetTile("Shrub_3");
        GameObject shrub4 = gameManager.GetTile("Shrub_4");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.25)
                    {
                        Instantiate(shrub1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 28;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(shrub2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 29;
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(shrub3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 30;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(shrub4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 31;
                    }
                }
            }
        }
    }

    void GenerateRock()
    {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 5 || (cellState[i, j] >= 18 && cellState[i, j] <= 31))
                {
                    landArray[i, j] = 1;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        // CA(0.5f, 1, 8, 1, true, 0);
        CA(0.5f, 1, 8, 1, false, 0);
    }

    void DrawRock()
    {
        GameObject rock1 = gameManager.GetTile("Rock_1");
        GameObject rock2 = gameManager.GetTile("Rock_2");
        GameObject rock3 = gameManager.GetTile("Rock_3");
        GameObject rock4 = gameManager.GetTile("Rock_4");
        GameObject prop2 = gameManager.GetTile("Prop_2");// add the plate here

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.20)
                    {
                        Instantiate(rock1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 34;//number in tileset folder
                    }
                    else if (tempValue < 0.4)
                    {
                        Instantiate(rock2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 35;
                    }
                    else if (tempValue < 0.6)
                    {
                        Instantiate(rock3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 36;
                    }
                    else if (tempValue < 0.8)
                    {
                        Instantiate(rock4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 37;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(prop2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j] = 33;
                    }
                }
            }
        }
    }

    void GenerateLoot()
    {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 5 || (cellState[i, j] >= 18 && cellState[i, j] <= 37))
                {
                    landArray[i, j] = 1;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        CA(0.65f, 1, 8, 1, false, 0);
    }

    void DrawLoot()
    {
        GameObject prop1 = gameManager.GetTile("Prop_1");
       

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {
                    Instantiate(prop1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                    cellState[i, j] = 32;//number in tileset folder            
                }
            }
        }
    }

    void GenerateEnemy()
    {
        for (int i = 0; i < 8; i++) {
            int xPos = Random.Range(0, levelWidth);
            int yPos = Random.Range(0, levelHeight);
            int size = Random.Range(1, 4);
            float ratio = (float)Random.Range(4,10)/(size*2+1)/ (size * 2 + 1);
            Seed(xPos,yPos,size,ratio);
        }
   
    }

    void DrawEnemy()
    {
        GameObject enemy1 = gameManager.GetEnemy("Minion_Type_1");
        GameObject enemy2 = gameManager.GetEnemy("Minion_Type_2");
        GameObject enemy3 = gameManager.GetEnemy("Minion_Type_3");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 100)
                {
                    Instantiate(enemy1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);          
                }
                else if (cellState[i, j] == 101)
                {
                    Instantiate(enemy2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                }
                else if (cellState[i, j] == 102)
                {
                    Instantiate(enemy3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                }
            }
        }
    }

    void DrawPortal()
    {
        GameObject portal1 = gameManager.GetPortal("Portal_1");
        bool isCreated = false;
        int xPos = Random.Range(0, levelWidth);
        int yPos = Random.Range(0, levelHeight);
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j] == 5)
                {
                    if (Random.Range(1, 1000) < 10) {
                        if (!isCreated) {
                            Instantiate(portal1, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                            isCreated = true;
                        }
                    }
                           
                }
            }
        }

    }

    void DrawPlayer()
    {
        GameObject player1 = gameManager.Player;
        bool isCreated = false;
        int xPos = Random.Range(0, levelWidth);
        int yPos = Random.Range(0, levelHeight);
        for (int i = levelWidth-1; i > -1; i--)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (cellState[i, j] == 5)
                {
                    if (Random.Range(1, 1000) < 10)
                    {
                        if (!isCreated)
                        {
                            Instantiate(player1, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                            isCreated = true;
                        }
                    }

                }
            }
        }

    }

}
