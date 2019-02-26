using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class ProcedualGeneration2_1 : MonoBehaviour {

    private GameManager gameManager;
    public int levelWidth;
    public int levelHeight;
    private float tileSize;
    private int[,] landArray;
    private int[,] newLandArray;
    private int[,] edgeArray;
    private CellState[,] cellState;
    private bool isSimultaneous;
    private float landRatio;
    private float treeRatio;
    private int iteration;
    private bool isEdgeReady;
    public GameObject theCanvas;
    //public GameObject textManager;

    private CameraControl cameraControl;

    // Use this for initialization
    void Start () {
        isEdgeReady = false;    
        NextScene.nowName = "2_1";
        cameraControl = GameObject.Find("CameraFollowing").GetComponent<CameraControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        levelWidth=(int)Random.Range(40, 60);
        if (levelWidth % 2 == 1) {
            levelWidth++;
        }
        levelHeight = (int)Random.Range(40, 60);
        if (levelHeight % 2 == 1)
        {
            levelHeight++;
        }
        Debug.Log(levelWidth + "bibi" + levelHeight);
        tileSize = 24;

        cameraControl.border2 = new Vector2((float)(levelWidth+1) * tileSize, (float)(levelHeight+1) * tileSize);
        //cameraControl.border2 = new Vector2(levelWidth + 1, levelHeight + 1);
        
        landArray = new int[levelWidth, levelHeight];
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                landArray[i, j] = 0;//for generate
            }
        }
        newLandArray = new int[levelWidth, levelHeight];
        edgeArray = new int[levelWidth, levelHeight];
        cellState = new CellState[levelWidth, levelHeight];
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                cellState[i, j] = new CellState();//for generate
                cellState[i, j].position = new Vector2(-100, -100);
            }
        }
        isSimultaneous = false;
        landRatio = 0.5f;
        treeRatio = 0.5f;
        iteration = 8;
  
        //initial terrain
        Draw();
        AddWalls();
        //generate up to 2 level platforms
        //for (int i = 0; i < 3; i++)
        //{
        //}
        Generate(0);
            
        ChangeEdge();
        for(int i=0;i<10;i++)
        {
            DrawEdge();
        }
        
        
        
        
        GenerateTrees();
        DrawTrees();
        
        GenerateCactus();
        DrawCactus();

        GenerateGrass();
        DrawGrass();

        GenerateRock();
        DrawRock();
        
        GenerateBone();
        DrawBone();

        GenerateLoot();
        DrawLoot();

        DrawPlayer();

        GenerateEnemy();
        DrawEnemy();

        DrawPortal();
     
        //DrawBoss();

        FinishGeneration();

        DrawNPC();
    }

    void CA(float ratio, int iteration,int threshold,int neighborSize, bool isSimultaneous, int targetNum) {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 0)
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
                    if (w == 0 || h == 0 || w == levelWidth - 1 || h == levelHeight - 1)
                    {
                        landArray[w, h] = 0;

                    }
                    else
                    {
                        if (!isSimultaneous)
                        {
                            landArray[w, h] = DetermineCell(w, h, targetNum, threshold, neighborSize);
                        }
                        else
                        {
                            newLandArray[w, h] = DetermineCell(w, h, targetNum, threshold, neighborSize);
                        }
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
        if (Random.value < 0.15f)
        {
            enemyNum = 100;//jumper
        }
        else if (Random.value < 0.3f) {
            enemyNum = 101;//slider
        }
        else if (Random.value < 0.5f)
        {
            enemyNum = 102;//spearThrower
        }
        else if (Random.value < 0.7f)
        {
            enemyNum = 103;//stomper
        }
        else if (Random.value < 1.0f)
        {
            enemyNum = 104;//suicider
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
                    if (cellState[i, j].state == 0)
                    {
                        if (isReady) {
                            cellState[i, j].state = DetermineEnemyType();
                            isReady = false;
                        }
                        else if(Random.value<ratio)
                            cellState[i, j].state = DetermineEnemyType();
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

    void Draw() {

        GameObject tile0 = gameManager.GetTile2("Tile_0");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (i % 2==0 && j % 2==0)
                {
                    Instantiate(tile0, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                    cellState[i, j].state = 0;
                    if (i + 1 < levelWidth && j + 1 < levelHeight)
                    {
                        cellState[i + 1, j].state = 0;
                        cellState[i, j + 1].state = 0;
                        cellState[i + 1, j + 1].state = 0;
                    }
                }

            }
        }
  
    }
    void AddWalls() {
        GameObject tile100 = gameManager.GetTile2("Tile_100");
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (i == 0 || j == 0)
                {
                    Instantiate(tile100, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                   
                }
                if(i == levelWidth - 1 )
                {
                    Instantiate(tile100, new Vector3((i+1) * tileSize, j * tileSize, 0), transform.rotation);

                }
                if (j == levelHeight - 1)
                {
                    Instantiate(tile100, new Vector3(i * tileSize, (j+1) * tileSize, 0), transform.rotation);

                }
                if (j == levelHeight - 1 && i == levelWidth - 1)
                {
                    Instantiate(tile100, new Vector3((i+1) * tileSize, (j + 1) * tileSize, 0), transform.rotation);
                }
            }
        }
    }
    void ChangeEdge() {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {
                    if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                   && landArray[i, j + 1] == 1 && landArray[i, j] == 1
                   && landArray[i, j - 1] == 0 && landArray[i - 1, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                    {
                        edgeArray[i, j] = 1;//下底边
                    }
                    else if (i < levelWidth - 1 && i > 0
                    && j < levelHeight && j > 0
                    && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 1
                    && landArray[i - 1, j - 1] == 0 && landArray[i, j] == 1)
                    {
                        edgeArray[i, j] = 3;//左下角
                    }
                    else if (i < levelWidth - 1 && i > 0
                    && j < levelHeight && j > 0
                    && landArray[i, j - 1] == 0 && landArray[i - 1, j - 1] == 1
                    && landArray[i + 1, j - 1] == 0 && landArray[i, j] == 1)
                    {
                        edgeArray[i, j] = 2;//右下角
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                   && landArray[i, j - 1] == 1 && landArray[i, j] == 1
                   && landArray[i, j + 1] == 0 && landArray[i - 1, j + 1] == 0 && landArray[i + 1, j + 1] == 0)
                    {
                        edgeArray[i, j] = 7;//上底边
                    }
                    else if (i < levelWidth - 1 && i > 0
                    && j < levelHeight - 1 && j > -1
                    && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 1
                    && landArray[i - 1, j + 1] == 0 && landArray[i, j] == 1)
                    {
                        edgeArray[i, j] = 8;//左上角
                    }
                    else if (i < levelWidth - 1 && i > 0
                    && j < levelHeight - 1 && j > -1
                    && landArray[i, j + 1] == 0 && landArray[i - 1, j + 1] == 1
                    && landArray[i + 1, j + 1] == 0 && landArray[i, j] == 1)
                    {
                        edgeArray[i, j] = 6;//右上角
                    }
                    else if (i < levelWidth - 1 && i > 0
                    && j < levelHeight - 1 && j > 0
                    && (landArray[i, j + 1] == 1 || landArray[i, j - 1] == 1 )&& landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 1 && landArray[i, j] == 1)
                    {
                        edgeArray[i, j] = 4;//左边
                    }
                    else if (i < levelWidth - 1 && i > 0
                    && j < levelHeight - 1 && j > 0
                    && (landArray[i, j + 1] == 1 || landArray[i, j - 1] == 1) && landArray[i + 1, j] == 0
                    && landArray[i - 1, j] == 1 && landArray[i, j] == 1)
                    {
                        edgeArray[i, j] = 5;//右边
                    }
                }
                else
                {
                    edgeArray[i, j] = 0;
                }
            }//end for col
        }//end for row
    }//end Changeedge

    void DrawEdge()
    {
        isEdgeReady = true;
        GameObject tile1 = gameManager.GetTile2("Tile_1");
        GameObject tile2 = gameManager.GetTile2("Tile_2");
        GameObject tile3 = gameManager.GetTile2("Tile_3");
        GameObject tile4 = gameManager.GetTile2("Tile_4");
        GameObject tile5 = gameManager.GetTile2("Tile_5");
        GameObject tile6 = gameManager.GetTile2("Tile_6");
        GameObject tile7 = gameManager.GetTile2("Tile_7");
        GameObject tile8 = gameManager.GetTile2("Tile_8");
        GameObject tile9 = gameManager.GetTile2("Tile_9");
        GameObject tile10 = gameManager.GetTile2("Tile_10");
        GameObject tile11 = gameManager.GetTile2("Tile_11");
        GameObject tile12 = gameManager.GetTile2("Tile_12");
        GameObject tile13 = gameManager.GetTile2("Tile_13");
        GameObject tile14 = gameManager.GetTile2("Tile_14");
        GameObject tile15 = gameManager.GetTile2("Tile_15");
        GameObject tile16 = gameManager.GetTile2("Tile_16");
        GameObject tile17 = gameManager.GetTile2("Tile_17");
        GameObject tile18 = gameManager.GetTile2("Tile_18");
        GameObject tile19 = gameManager.GetTile2("Tile_19");
        GameObject tile99 = gameManager.GetTile2("99");
        GameObject tile100 = gameManager.GetTile2("100");
        GameObject tile101 = gameManager.GetTile2("101");
        GameObject tile102 = gameManager.GetTile2("102");
        GameObject tile103 = gameManager.GetTile2("103");
        GameObject tile104 = gameManager.GetTile2("104");
        GameObject tile105 = gameManager.GetTile2("105");
        GameObject tile106 = gameManager.GetTile2("106");
        /*
        GameObject tile9 = gameManager.GetTile("Tile_9");
        GameObject tile10 = gameManager.GetTile("Tile_10");
        GameObject tile11 = gameManager.GetTile("Tile_11");
        GameObject tile12 = gameManager.GetTile("Tile_12");
        GameObject tile13 = gameManager.GetTile("Tile_13");
        GameObject tile14 = gameManager.GetTile("Tile_14");
        GameObject tile15 = gameManager.GetTile("Tile_15");
        GameObject tile16 = gameManager.GetTile("Tile_16");
        GameObject tile17 = gameManager.GetTile("Tile_17");
        */
        for (int j = 0; j < levelHeight; j++)
        {
            
            for (int i = 0; i < levelWidth; i++)

            {
                if (landArray[i, j] == 1) {
                    //Instantiate(tile10, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                }

                //down
                if (edgeArray[i,j]==1 && cellState[i, j].state == 0)
                {
                   

                    
                    Vector2 position = new Vector2(-100, -100);
                    //如果右边还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j] == 1 && cellState[i, j].state != 1)
                    {
                        //open the gate!
                        if (Random.value < 0.1 && j != 0)
                        {
                            Instantiate(tile19, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 19;
                            cellState[i, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);
                            cellState[i + 1, j].state = 19;
                            cellState[i + 1, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);

                        }
                        else
                        {
                            Instantiate(tile1, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 1;
                            cellState[i, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);
                            cellState[i + 1, j].state = 1;
                            cellState[i + 1, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);
                        }
                    }
                    else if (cellState[i, j].state != 1) {
                        Instantiate(tile9, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 9;
                        cellState[i, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);

                    }


                }

                //left-bot corner
                else if (edgeArray[i, j] == 3 && cellState[i, j].state == 0)
                {
                    

                    Vector2 position = new Vector2(-100, -100);
                    //如果左上角还是自己，则使用两个格子的tile
                    if (edgeArray[i - 1, j + 1] == 3 && cellState[i, j].state != 3) {
                        if (edgeArray[i + 1, j - 1] == 1)//右下角是底边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 48;
                        }
                        else if (edgeArray[i + 1, j - 1] == 3)//右下角是自己类型
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 48;
                            position.y = position.y + 24;
                        }
                        else if (edgeArray[i + 1, j - 1] == 4)//右下角是左边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 48;
                            position.y = position.y - 40;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile3, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 3;
                            cellState[i, j].position = position;
                            cellState[i - 1, j + 1].state = 3;
                            cellState[i - 1, j + 1].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }
                    else if (cellState[i, j].state != 3)
                    {
                        //如果右下角已经有合成的斜边了
                        if (cellState[i + 1, j - 1].state == 3)
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 24;

                        }
                        //如果右下角是左边
                        else if (edgeArray[i + 1, j - 1] == 4)
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 40;

                        }

                        //右下角是底边
                        else if (edgeArray[i + 1, j - 1] == 1)
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;

                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile11, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 11;
                            cellState[i, j].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }


                }

                //right-bot corner
                else if (edgeArray[i, j] == 2 && cellState[i, j].state == 0)
                {
                    //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                    Vector2 position = new Vector2(-100, -100);
                    //如果右上角还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j + 1] == 2 && cellState[i, j].state != 2)
                    {
                        if (cellState[i - 1, j - 1].state == 1)//左下角是双底边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                        }
                        else if (cellState[i - 1, j - 1].state == 9)//左下角是单底边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                        }
                        else if (edgeArray[i - 1, j - 1] == 2)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 24;
                        }
                        else if (edgeArray[i - 1, j - 1] == 5)//左下角是右边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 40;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile2, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 2;
                            cellState[i, j].position = position;
                            cellState[i + 1, j + 1].state = 2;
                            cellState[i + 1, j + 1].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }
                    
                    else if (cellState[i, j].state != 2) {
                        //如果左下角已经有合成的斜边了
                        if (cellState[i - 1, j - 1].state == 2)
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 24;
                        }
                        //如果左下角是右边
                        else if (edgeArray[i - 1, j - 1] == 5)
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 40;
                        }

                        //左下角是双底边
                        else if (cellState[i - 1, j - 1].state == 1)
                        {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 48;
                        }

                        //左下角是单底边
                        else if (cellState[i - 1, j - 1].state == 9)
                        {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 24;
                        }

                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile12, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 12;
                            cellState[i, j].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }


                    }
        
                }
                //left
                else if (edgeArray[i, j] == 4 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);
                    if (cellState[i, j - 1].state == 11)//下边是单左下边
                    {
                        position = cellState[i, j - 1].position;
                        
                        position.y = position.y + 72;
                        
                    }
                    else if (cellState[i, j - 1].state == 3)//下边是双左下边
                    {
                        position = cellState[i, j - 1].position;  
                        position.y = position.y + 86;
                       
                    }
                    
                    else if (edgeArray[i, j-1] == 4)//下边是自己类型的
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 24;
                       
                    }
                    else if (cellState[i-1, j - 1].state == 8)//左下边是双边左上角
                    {
                        
                        position = cellState[i-1, j - 1].position;
                        position.x = position.x + 48;
                        position.y = position.y + 65;
                        
                        
                    }
                    else if (cellState[i - 1, j - 1].state == 15)//左下边是单边左上角
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x + 24;
                        position.y = position.y + 41;
                        
                    }

                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile14, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 14;
                        cellState[i, j].position = position;
                    }
                    else {
                        isEdgeReady = false;
                        //Instantiate(tile105, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                        

                }

                //right
                else if (edgeArray[i, j] == 5 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);
                    if (cellState[i, j - 1].state == 12)//下边是单右下边
                    {
                        position = cellState[i, j - 1].position;
                  
                        position.y = position.y + 72;
                        
                    }
                    else if (cellState[i, j - 1].state == 2)//下边是双右下边
                    {
                        position = cellState[i, j - 1].position;
                        position.x = position.x + 24;
                        position.y = position.y + 83;
                        
                    }
                    else if (edgeArray[i, j-1] == 5)//下边是自己类型的
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 24;
                        
                    }
                    else if (cellState[i + 1, j - 1].state == 6)//右下边是双边右上角
                    {

                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 65;
                        
                    }
                    else if (cellState[i + 1, j - 1].state == 17)//右下边是单边右上角
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 41;
                        
                    }
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 13;
                        cellState[i, j].position = position;
                    }
                    else {
                        isEdgeReady = false;
                       // Instantiate(tile106, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }


                }

                //left-top corner
                else if (edgeArray[i, j] == 8 && cellState[i, j].state == 0)
                {
     
                    Vector2 position = new Vector2(-100, -100);
                    //如果右上角还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j + 1] == 8 && cellState[i, j].state != 8)
                    {
                        if (cellState[i, j - 1].state == 3)//下角是双边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 67;
                        }
                        else if (cellState[i, j - 1].state == 11)//下角是单边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 53;
                        }
                        else if (edgeArray[i, j-1] == 1)//下角是单双下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 48;
                        }

                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (cellState[i, j - 1].state == 14)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }
                        else if (cellState[i - 1, j - 1].state == 16)//左下角是单上边
                        {
                            position = cellState[i-1, j - 1].position;
                            position.x = position.x + 24;
                        }
                        else if (cellState[i - 1, j - 1].state == 7)//左下角是双上边
                        {
                            position = cellState[i-1, j - 1].position;
                            position.x = position.x + 48;
                        }
                        else if (cellState[i - 1, j].state == 16)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (cellState[i - 1, j].state == 17)//左边是单右上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (cellState[i - 1, j].state == 6)//左边是双右上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 48;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile8, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 8;
                            cellState[i, j].position = position;
                            cellState[i + 1, j + 1].state = 8;
                            cellState[i + 1, j + 1].position = position;
                        }
                        else {
                            isEdgeReady = false;
                           // Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                            
                    }

                    //其他情况贴单个斜边
                    else if (cellState[i, j].state != 8)
                    {
                        if (cellState[i, j - 1].state == 3)//下角是双边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 67;
                        }
                        else if (cellState[i, j - 1].state == 11)//下角是单边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 53;
                        }
                        else if (edgeArray[i, j - 1] == 1)//下角是单双下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (cellState[i, j - 1].state == 14)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }
                        else if (cellState[i - 1, j - 1].state == 16)//左下角是单上边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                        }
                        else if (cellState[i - 1, j - 1].state == 7)//左下角是双上边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                        }
                        else if (cellState[i - 1, j].state == 16)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (cellState[i - 1, j].state == 17)//左边是单右上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (cellState[i - 1, j].state == 6)//左边是双右上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 48;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile15, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 15;
                            cellState[i, j].position = position;

                        }
                        else {
                            isEdgeReady = false;
                          //  Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                            
                    }
                   
                        

                }

                //top
                else if (edgeArray[i, j] == 7 && cellState[i, j].state == 0)
                {
                    
                    Vector2 position = new Vector2(-100, -100);

                    if (cellState[i - 1, j - 1].state == 8)//左下角是双边左上边
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x + 48;
                        position.y = position.y + 55;
                    }
                    else if (cellState[i - 1, j - 1].state == 15)//左下角是单边左上边
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x + 24;
                        position.y = position.y + 31;
                    }
                    else if (cellState[i - 1, j].state == 8)//左边是双边左上边
                    {
                        position = cellState[i - 1, j].position;
                        position.x = position.x + 48;
                        position.y = position.y + 55;
                    }
                    else if (cellState[i - 1, j].state == 15)//左边是单边左上边
                    {
                        position = cellState[i - 1, j].position;
                        position.x = position.x + 24;
                        position.y = position.y + 31;
                    }
                    else if (cellState[i - 1, j].state == 6)//左边是双边右上边
                    {
                        position = cellState[i - 1, j].position;
                        position.x = position.x + 48;
                      
                    }
                    else if (cellState[i - 1, j].state == 17)//左边是单边右上边
                    {
                        position = cellState[i - 1, j].position;
                        position.x = position.x + 24;
                        
                    }
                    /*
                    else if (cellState[i - 1, j - 1].state == 6)//左下角是双边右上边
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 55;
                    }
                    else if (cellState[i - 1, j - 1].state == 17)//左下角是单边右下边
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 31;
                    }
                    */
                    else if (cellState[i - 1, j].state == 16)//左边是自己
                    {
                        position = cellState[i - 1, j].position;
                        position.x = position.x + 24;

                    }
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile16, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 16;
                        cellState[i, j].position = position;
                    }
                    else {
                        isEdgeReady = false;
                       // Instantiate(tile102, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
     

                }
                

                //link left and top
                else if (edgeArray[i, j] / 240 == 1 && edgeArray[i, j] % 240 >= 0 && edgeArray[i, j] % 240 <= 7)
                {

                    //Instantiate(tile13, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    //cellState[i, j] = 13;
                    //landArray[i, j] = 0;
                }

                //link right and top
                else if (edgeArray[i, j] / 240 == 1 &&
                    (edgeArray[i, j] % 240 == 0 || edgeArray[i, j] % 240 == 2 ||
                    edgeArray[i, j] % 240 == 4 || edgeArray[i, j] % 240 == 6 ||
                    edgeArray[i, j] % 240 == 8 || edgeArray[i, j] % 240 == 10 ||
                    edgeArray[i, j] % 240 == 12 || edgeArray[i, j] % 240 == 14
                    ))
                {

                    //Instantiate(tile12, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    //cellState[i, j] = 12;
                    //landArray[i, j] = 0;
                }

                //link right and bot
                else if (edgeArray[i, j] / 240 == 1 &&
                    (edgeArray[i, j] % 240 == 0 || edgeArray[i, j] % 240 == 1 ||
                    edgeArray[i, j] % 240 == 4 || edgeArray[i, j] % 240 == 5 ||
                    edgeArray[i, j] % 240 == 8 || edgeArray[i, j] % 240 == 9 ||
                    edgeArray[i, j] % 240 == 12 || edgeArray[i, j] % 240 == 13
                    ))
                {

                    //Instantiate(tile10, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    //cellState[i, j] = 10;
                    //landArray[i, j] = 0;
                }

                //link left and bot
                else if (edgeArray[i, j] / 240 == 1 &&
                    (edgeArray[i, j] % 240 == 0 || edgeArray[i, j] % 240 == 1 ||
                    edgeArray[i, j] % 240 == 2 || edgeArray[i, j] % 240 == 3 ||
                    edgeArray[i, j] % 240 == 8 || edgeArray[i, j] % 240 == 9 ||
                    edgeArray[i, j] % 240 == 10 || edgeArray[i, j] % 240 == 11
                    ))
                {

                    //Instantiate(tile11, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    //cellState[i, j] = 11;
                    //landArray[i, j] = 0;
                }
            }
        }


        for (int j = levelHeight - 1; j > -1; j--) {
            for (int i = 0; i < levelWidth; i++)
            {
                //right-top corner
                if (edgeArray[i, j] == 6 && cellState[i, j].state == 0)
                {

                    Vector2 position = new Vector2(-100, -100);
                    //如果右下角还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j - 1] == 6 && cellState[i, j].state != 6)
                    {
                        /*
                        if (cellState[i, j - 1].state == 2)//下边是双右下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 67;
                        }
                        else if (cellState[i, j - 1].state == 12)//下边是单边右下边
                        {
                            position = cellState[i, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 53;
                        }
                        */
                        if (cellState[i - 1, j].state == 16)//左边是上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (edgeArray[i - 1, j + 1] == 6)//左上角是自己类型
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 48;
                            position.y = position.y - 48;
                        }
                        else if (cellState[i - 1, j + 1].state == 16)//左上角是单上边
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 53;
                        }
                        
                        else if (cellState[i-1, j + 1].state == 13)//左上角是右边
                        {
                            position = cellState[i-1, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 76;
                        }
                        
                        
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile6, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 6;
                            cellState[i, j].position = position;
                            cellState[i + 1, j - 1].state = 6;
                            cellState[i + 1, j - 1].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                    }

                    //其他情况贴单个斜边
                    else if (cellState[i, j].state != 6)
                    {
                        /*
                        if (cellState[i, j - 1].state == 2)//下边是双右下边
                        {
                            position = cellState[i, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 67;
                        }
                        else if (cellState[i, j - 1].state == 12)//下边是单边右下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 53;
                        }

                       
                        */
                        if (cellState[i - 1, j].state == 16)//左边是上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (edgeArray[i - 1, j + 1] == 6)//左上角是自己类型
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 48;
                            position.y = position.y - 24;
                        }
                        else if (cellState[i - 1, j + 1].state == 16)//左上角是单上边
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 30;
                        }
                        else if (cellState[i - 1, j + 1].state == 13)//左上角是右边
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 52;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile17, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 17;
                            cellState[i, j].position = position;

                        }
                        else
                        {
                            isEdgeReady = false;
                          //  Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                    }
                }

                //right
                else if (edgeArray[i, j] == 5 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);

                    if (edgeArray[i, j + 1] == 5)//上边是自己类型的
                    {
                        position = cellState[i, j + 1].position;
                        position.y = position.y - 24;

                    }
                    else if (cellState[i, j + 1].state == 6)//上边是双边右上角
                    {

                        position = cellState[i, j + 1].position;
                        position.x = position.x + 24;

                    }
                    else if (cellState[i, j + 1].state == 17)//上边是单边右上角
                    {
                        position = cellState[i, j + 1].position;
                    }
                    else if (cellState[i+1, j + 1].state == 2)//右上边是双边右下角
                    {

                        position = cellState[i + 1, j + 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 41;
                    }
                    else if (cellState[i+1, j + 1].state == 12)//右上边是单边右下角
                    {
                        position = cellState[i + 1, j + 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 41;
                    }
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 13;
                        cellState[i, j].position = position;
                    }
                    else
                    {
                        isEdgeReady = false;
                       // Instantiate(tile106, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }


                }

                //right-bot corner
                else if (edgeArray[i, j] == 2 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);
                    //如果左下角还是自己，则使用两个格子的tile
                    if (edgeArray[i - 1, j - 1] == 2 && cellState[i, j].state != 2)
                    {
                        if (cellState[i + 1, j].state == 1)//右边是双底边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                            position.y = position.y - 21;
                        }
                        else if (cellState[i + 1, j].state == 9)//右边是单底边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                            position.y = position.y - 17;
                        }
                        else if (cellState[i + 1, j].state == 3)//右边是双左下边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                            
                        }
                        else if (cellState[i + 1, j].state == 11)//右边是单左下边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                            position.y = position.y - 17;
                        }
                        else if (edgeArray[i + 1, j + 1] == 2)//右上角是自己类型
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 48;
                            position.y = position.y - 21;
                        }
                        else if (edgeArray[i + 1, j + 1] == 5)//右上角是右边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 83;
                        }
                        else if (edgeArray[i, j + 1] == 5)//上边是右边
                        {
                            position = cellState[i, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 83;
                        }
                        else if (cellState[i, j + 1].state == 17)//上边是单右上角
                        {
                            position = cellState[i, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 65;
                        }
                        else if (cellState[i, j + 1].state == 6)//上边是双右上角
                        {
                            position = cellState[i, j + 1].position;
                            
                            position.y = position.y - 65;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile2, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 2;
                            cellState[i, j].position = position;
                            cellState[i - 1, j - 1].state = 2;
                            cellState[i - 1, j - 1].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                           // Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }

                    else if (cellState[i, j].state != 2)
                    {
                        if (cellState[i + 1, j].state == 1)//右边是双底边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                            position.y = position.y - 6;
                        }
                        else if (cellState[i + 1, j].state == 9)//右边是单底边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                          
                        }
                        else if (cellState[i + 1, j].state == 3)//右边是双左下边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                            position.y = position.y + 14;

                        }
                        else if (cellState[i + 1, j].state == 11)//右边是单左下边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                           
                        }
                        else if (edgeArray[i + 1, j + 1] == 2)//右上角是自己类型
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 6;
                        }
                        else if (edgeArray[i + 1, j + 1] == 5)//右上角是右边
                        {
                            position = cellState[i + 1, j + 1].position;
                           
                            position.y = position.y - 71;
                        }
                        else if (edgeArray[i, j + 1] == 5)//上边是右边
                        {
                            position = cellState[i, j + 1].position;
                            position.y = position.y - 71;
                        }
                        else if (cellState[i, j+1].state == 17)//上边是单右上角
                        {
                            position = cellState[i, j + 1].position;
                           
                            position.y = position.y - 53;
                        }
                        else if (cellState[i, j+1].state == 6)//上边是双右上角
                        {
                            position = cellState[i, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 53;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile12, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 12;
                            cellState[i, j].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                           // Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }


                    }

                }
            }
        }
    }

    void GenerateTrees() {
        //initialize
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (cellState[i, j].state == 0)
                {
                    landArray[i, j] = 0;//draw trees on blank tiles
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
        GameObject tree1 = gameManager.GetTile2("Tile_20");
        GameObject tree2 = gameManager.GetTile2("Tile_21");
        

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i, j] == 1)
                {
                    Debug.Log("shu");
                    float tempValue = Random.value;
                    if (tempValue < 0.5)
                    {
                        Instantiate(tree1, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 20;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tree2, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 21;
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
                if (cellState[i, j].state == 0)
                {
                    landArray[i, j] = 0;
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
        GameObject tile22 = gameManager.GetTile2("Tile_22");
        GameObject tile23 = gameManager.GetTile2("Tile_23");
        GameObject tile24 = gameManager.GetTile2("Tile_24");
        GameObject tile25 = gameManager.GetTile2("Tile_25");
        GameObject tile26 = gameManager.GetTile2("Tile_26");
        GameObject tile27 = gameManager.GetTile2("Tile_27");
        GameObject tile28 = gameManager.GetTile2("Tile_28");
        GameObject tile29 = gameManager.GetTile2("Tile_29");


        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i, j] == 1)
                {
                
                    float tempValue = Random.value;
                    if (tempValue < 0.125)
                    {
                        Instantiate(tile22, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 22;//number in tileset folder
                    }
                    else if (tempValue < 0.25)
                    {
                        Instantiate(tile23, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 23;//number in tileset folder
                    }
                    else if (tempValue < 0.375)
                    {
                        Instantiate(tile24, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 24;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tile25, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 25;//number in tileset folder
                    }
                    else if (tempValue < 0.625)
                    {
                        Instantiate(tile26, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 26;//number in tileset folder
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(tile27, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 27;//number in tileset folder
                    }
                    else if (tempValue < 0.875)
                    {
                        Instantiate(tile28, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 28;//number in tileset folder
                    }
                    else if (tempValue < 0.1)
                    {
                        Instantiate(tile29, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 29;//number in tileset folder
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
                if (cellState[i, j].state == 0)
                {
                    landArray[i, j] = 0;
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
        GameObject tile30 = gameManager.GetTile2("Tile_30");
        GameObject tile31 = gameManager.GetTile2("Tile_31");
        GameObject tile32 = gameManager.GetTile2("Tile_32");
        GameObject tile33 = gameManager.GetTile2("Tile_33");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.25)
                    {
                        Instantiate(tile30, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 30;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tile31, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 31;//number in tileset folder
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(tile32, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 32;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile33, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 33;//number in tileset folder
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
                if (cellState[i, j].state == 0)
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
        GameObject tile34 = gameManager.GetTile2("Tile_34");
        GameObject tile35 = gameManager.GetTile2("Tile_35");
        GameObject tile36 = gameManager.GetTile2("Tile_36");
        GameObject tile37 = gameManager.GetTile2("Tile_37");
        GameObject tile38 = gameManager.GetTile2("Tile_38");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.20)
                    {
                        Instantiate(tile34, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 34;//number in tileset folder
                    }
                    else if (tempValue < 0.4)
                    {
                        Instantiate(tile35, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 35;//number in tileset folder
                    }
                    else if (tempValue < 0.6)
                    {
                        Instantiate(tile36, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 36;//number in tileset folder
                    }
                    else if (tempValue < 0.8)
                    {
                        Instantiate(tile37, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 37;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile38, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 38;//number in tileset folder
                    }
                }
            }
        }
    }

    void GenerateBone()
    {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j].state == 0)
                {
                    landArray[i, j] = 0;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        //CA(0.5f, 1, 8, 1, true, 0);
        CA(0.5f, 1, 8, 1, true, 0);
    }

    void DrawBone()
    {
        GameObject tile39 = gameManager.GetTile2("Tile_39");
        GameObject tile40 = gameManager.GetTile2("Tile_40");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.5)
                    {
                        Instantiate(tile39, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 39;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile40, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        cellState[i, j].state = 40;//number in tileset folder
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
                if (cellState[i, j].state == 0)
                {
                    landArray[i, j] = 0;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        CA(0.55f, 1, 8, 1, false, 0);
    }

    void DrawLoot()
    {
        GameObject tile41 = gameManager.GetTile2("Tile_41");
       

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i, j] == 1)
                {
                    Instantiate(tile41, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                    cellState[i, j].state = 41;//number in tileset folder           
                }
            }
        }
    }

    void GenerateEnemy()
    {
        /*
        for (int i = 0; i < 8; i++) {
            int xPos = Random.Range(0, levelWidth);
            int yPos = Random.Range(0, levelHeight);
            int size = Random.Range(1, 4);
            float ratio = (float)Random.Range(4,10)/(size*2+1)/ (size * 2 + 1);
            Seed(xPos,yPos,size,ratio);
        }
        */
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j].state == 0)
                {
                    landArray[i, j] = 0;
                }
                else
                {
                    landArray[i, j] = -1;
                }

            }
        }
        for (int i = 5; i < levelWidth-5; i++)
        {
            for (int j = 5; j< levelHeight-5; j++)
            {
                if (landArray[i, j] == 0)
                {
                    float ratio = (float)Random.Range(1, 50);
                    bool isClose = false;
                    if (ratio < 15)
                    {
                        for (int m = -2; m < 3; m++) {
                            for (int n = -2; n < 3; n++) {
                                if (landArray[i + m, j + n] != 0)
                                    isClose = true;
                            }
                        }
                        for (int m = -5; m < 6; m++)
                        {
                            for (int n = -5; n < 6; n++)
                            {
                                if (cellState[i + m, j + n].state==200)
                                    isClose = true;
                            }
                        }
                        if (!isClose) {
                            landArray[i, j] = -1;
                            cellState[i, j].state = DetermineEnemyType();
                        }

                    }
                              
                }
            }
        }
    }

    void DrawEnemy()
    {
        GameObject enemy1 = gameManager.GetEnemy("Jumper");
        GameObject enemy2 = gameManager.GetEnemy("Slider");
        GameObject enemy3 = gameManager.GetEnemy("SpearThrower");
        GameObject enemy4 = gameManager.GetEnemy("Stomper");
        GameObject enemy5 = gameManager.GetEnemy("SuicideBomber");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j].state == 100)
                {
                    Instantiate(enemy1, new Vector3(i*(float)tileSize, j*(float)tileSize, 0), transform.rotation);          
                }
                else if (cellState[i, j].state == 101)
                {
                    Instantiate(enemy2, new Vector3(i*(float)tileSize, j*(float)tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 102)
                {
                    Instantiate(enemy3, new Vector3(i*(float)tileSize, j*(float)tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 103)
                {
                    Instantiate(enemy4, new Vector3(i*(float)tileSize, j*(float)tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 104)
                {
                    Instantiate(enemy5, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                }
            }
        }
    }

    void DrawPortal()
    {
        GameObject portal1 = gameManager.GetPortal("Portal_2");
        bool isCreated = false;
        int xPos = Random.Range(0, levelWidth);
        int yPos = Random.Range(0, levelHeight);
        for (int i =4; i < levelWidth-4; i++)
        {
            for (int j = 4; j < levelHeight-4; j++)
            {
                if (cellState[i, j].state == 0)
                {
                    if (Random.Range(1, 1000) < 2) {
                        if (!isCreated) {
                            Instantiate(portal1, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
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
        for (int m = -5; m < 6; m++) {
            for (int n = -5; n < 6; n++)
            {
                if (cellState[levelWidth/2+m, levelHeight/2+n].state == 0)
                {
                    if (Random.Range(1, 100) < 5)
                    {
                        if (!isCreated)
                        {
                            player1 = Instantiate(player1, new Vector3((levelWidth / 2 + m) * tileSize, (levelHeight / 2 + n) * tileSize, 0), transform.rotation);
                            cellState[levelWidth / 2 + m, levelHeight / 2 + n].state = 200;
                            player1.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
                            player1.GetComponent<Movement_New>().WalkSpeed = 150;
                            player1.GetComponent<Shoot_New>().BulletSizeUp = 15;
                            player1.GetComponentInChildren<PickUp_New>().WeaponSizeUp = 15;
                            player1.GetComponentInChildren<PickUp_New>().ItemSizeUp = 15;
                            isCreated = true;
                        }
                    }

                }
            }
        }
        for (int i = levelWidth-1; i > -1; i--)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                
            }
        }

    }

    void DrawBoss()
    {
        float xPos = Random.Range(2.5f, cameraControl.border2.x-2.5f);
        float yPos = Random.Range(2.5f, cameraControl.border2.y-2.5f);
            
        
        GameObject boss = gameManager.GetBoss("FairBoss");
       
        Instantiate(boss, new Vector3(xPos, yPos, 0), boss.transform.rotation);
    }

    void DrawNPC()
    {
        GameObject textManager = GameObject.Find("TextManager");
        //textManager2.GetComponent<NPCManager>().FindAllPossibleId();
        //textManager2.GetComponent<NPCManager>().GenerateNPC();
        textManager.GetComponent<NPCManager>().Inite();
        GameObject NPCObject = textManager.GetComponent<NPCManager>().GenerateNPC();
       // Instantiate(NPCObject);
        bool isCreated = false;

        for (int i = levelWidth - 1; i > -1; i--)
        {
            for (int j = levelHeight - 1; j > -1; j--)
            {
                if (cellState[i, j].state == 0)
                {
                    if (Random.Range(1, 1000) < 2)
                    {
                        if (!isCreated)
                        {
                           
                           Instantiate(NPCObject, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                           // player1.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
                           // player1.GetComponent<Movement_New>().WalkSpeed = 5;
                           isCreated = true;
                        }
                    }

                }
            }
        }
    }

    void FinishGeneration() {
        theCanvas.SetActive(true);
    }

}
