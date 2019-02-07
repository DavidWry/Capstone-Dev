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

    private CameraControl cameraControl;

    // Use this for initialization
    void Start () {
        NextScene.nowName = "2_1";
        cameraControl = GameObject.Find("CameraFollowing").GetComponent<CameraControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        levelWidth=(int)Random.Range(40, 60);
        levelHeight = (int)Random.Range(40, 60);
        tileSize = 24;

        cameraControl.border2 = new Vector2((float)(levelWidth+1) * tileSize / 100, (float)(levelHeight+1) * tileSize / 100);
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
            }
        }
        isSimultaneous = false;
        landRatio = 0.5f;
        treeRatio = 0.5f;
        iteration = 8;
  
        //initial terrain
        Draw();
        
        //generate up to 2 level platforms
        //for (int i = 0; i < 3; i++)
        //{
            Generate(0);
            
            ChangeEdge();
            DrawEdge();
        //}
        
        /*
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

        DrawPlayer();

        GenerateEnemy();
        DrawEnemy();

        DrawPortal();

        

        DrawBoss();
        */
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
        else if (Random.value < 0.8f)
        {
            enemyNum = 102;//This type will always drop ultimate resources
        }
        else if (Random.value < 1.0f)
        {
            enemyNum = 103;
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
                    if (cellState[i, j].state == 5)
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
                    Instantiate(tile10, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                }
                //down

                if (edgeArray[i,j]==1)
                {
                    //open the gate!
                    //if (Random.value < 0.1 && j != 0)
                    //{
                    //Instantiate(tile16, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                    //cellState[i, j] = 16;
                    //landArray[i, j] = 0;
                    //}

                    //Instantiate(tile99, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    //只画奇数，偶数不画
                    if (cellState[i, j].state != 1)
                    {
                        //到右边界了 || 右边是右下角方块 ||右边是左下方块
                        if ((i < levelWidth - 1) && (landArray[i + 1, j] == 0)||(edgeArray[i+1,j]==2) || (edgeArray[i + 1, j] == 3))
                        {
                            Instantiate(tile9, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 9;
                            cellState[i, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);

                        }
                        else
                        {
                            Instantiate(tile1, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 1;
                            cellState[i, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);
                            //如果右边也是下底边，则合并那个格子
                            if (i < levelWidth - 1 && landArray[i + 1, j] == 1)
                            {
                                cellState[i + 1, j].state = 1;
                                cellState[i + 1, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);

                            }
                        }
                        
                    }
                    
                }

                //left-bot corner
                else if (edgeArray[i, j] == 3)
                {
                    //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                    Vector2 position = new Vector2(0, 0);
                    //如果左上角还是自己，则使用两个格子的tile
                    if (edgeArray[i - 1, j + 1] == 3 && cellState[i, j].state !=3) {
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
                        Instantiate(tile3, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 3;
                        cellState[i, j].position = position;
                        cellState[i-1, j+1].state = 3;
                        cellState[i-1, j+1].position = position;
                    }
                    
                    //如果右下角已经有合成的斜边了
                    else if(cellState[i+1, j-1].state == 3 && cellState[i, j].state != 3)
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x- 24;
                        position.y = position.y + 24;
                        Instantiate(tile11, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 11;
                        cellState[i, j].position = position;
                    }
                    //如果右下角是左边
                    else if (edgeArray[i + 1, j - 1] == 4 && cellState[i, j].state != 3)
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x - 24;
                        position.y = position.y - 40;
                        Instantiate(tile11, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 11;
                        cellState[i, j].position = position;
                    }

                    //默认情况
                    else if (cellState[i, j].state != 3)
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x - 24;
                        Instantiate(tile11, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 11;
                        cellState[i, j].position = position;
                    }
                    //Vector2 position = new Vector2(i * (float)tileSize, j * (float)tileSize);
                    /*
                    if (position.x == 0 && position.y == 0&&cellState[i, j].state != 3) {
                        Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    */
                }

                //right-bot corner
                else if (edgeArray[i, j] == 2)
                {
                    //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                    Vector2 position = new Vector2(0, 0);
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
                        Instantiate(tile2, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 2;
                        cellState[i, j].position = position;
                        cellState[i + 1, j + 1].state = 2;
                        cellState[i + 1, j + 1].position = position;
                    }
                    //如果左下角已经有合成的斜边了
                    else if (cellState[i - 1, j - 1].state == 2 && cellState[i, j].state != 2)
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x + 48;
                        position.y = position.y + 24;
                        Instantiate(tile12, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 12;
                        cellState[i, j].position = position;
                    }
                    //如果左下角是右边
                    else if (edgeArray[i - 1, j - 1] == 5 && cellState[i, j].state != 2)
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.x = position.x + 24;
                        position.y = position.y - 40;
                        Instantiate(tile12, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 12;
                        cellState[i, j].position = position;
                    }

                    //左下角是下底边
                    else if (cellState[i, j].state != 2)
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
                        Instantiate(tile12, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 12;
                        cellState[i, j].position = position;
                    }
                }
                //left
                else if (edgeArray[i, j] == 4)
                {
                    Vector2 position = new Vector2(0, 0);
                    if (cellState[i, j - 1].state == 11)//下边是单边
                    {
                        position = cellState[i, j - 1].position;
                        
                        position.y = position.y + 72;
                        Instantiate(tile14, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 14;
                        cellState[i, j].position = position;
                    }
                    else if (cellState[i, j - 1].state == 3)//下边是双边
                    {
                        position = cellState[i, j - 1].position;
                       
                        position.y = position.y + 86;
                        Instantiate(tile14, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 14;
                        cellState[i, j].position = position;
                    }
                    
                    else if (edgeArray[i, j-1] == 4)//下边是自己类型的
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 24;
                        Instantiate(tile14, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 14;
                        cellState[i, j].position = position;
                    }
                    


                    if (cellState[i, j].state != 14)
                        Instantiate(tile105, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                }

                //right
                else if (edgeArray[i, j] == 5)
                {
                    Vector2 position = new Vector2(0, 0);
                    if (cellState[i, j - 1].state == 12)//下边是单边
                    {
                        position = cellState[i, j - 1].position;
                  
                        position.y = position.y + 72;
                        Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 13;
                        cellState[i, j].position = position;
                    }
                    else if (cellState[i, j - 1].state == 2)//下边是双边
                    {
                        position = cellState[i, j - 1].position;
                        position.x = position.x + 24;
                        position.y = position.y + 83;
                        Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 13;
                        cellState[i, j].position = position;
                    }
                    else if (edgeArray[i, j-1] == 5)//下边是自己类型的
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 24;
                        Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 13;
                        cellState[i, j].position = position;
                    }

                    if (cellState[i, j].state != 13)
                        Instantiate(tile106, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);


                }

                //left-top corner
                else if (edgeArray[i, j] == 8)
                {
                    Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                    Vector2 position = new Vector2(0, 0);
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
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i, j - 1] == 14)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }
                        Instantiate(tile8, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 8;
                        cellState[i, j].position = position;
                        cellState[i + 1, j + 1].state = 8;
                        cellState[i + 1, j + 1].position = position;
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
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i, j - 1] == 14)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }
                        Instantiate(tile15, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 15;
                        cellState[i, j].position = position;
                    }

                }




                //top
                else if (edgeArray[i, j] == 7)
                {
                    Instantiate(tile102, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                }

               

                //right-top corner
                else if (edgeArray[i, j] == 6)
                {

                    Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    cellState[i, j].state = 6;
                    
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
    }

    void GenerateTrees() {
        //initialize
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (cellState[i, j].state == 5)
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
                        cellState[i, j].state = 18;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tree2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 19;
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(tree3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 20;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tree4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
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
                if (cellState[i, j].state == 5|| (cellState[i, j].state >= 18 && cellState[i, j].state <= 21))
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
                        cellState[i, j].state = 22;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(cactus2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 23;
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(cactus3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 24;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(cactus4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 25;
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
                if (cellState[i, j].state == 5 || (cellState[i, j].state >= 18 && cellState[i, j].state <= 25))
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
                        cellState[i, j].state = 26;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(grass2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 27;
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
                if (cellState[i, j].state == 5 || (cellState[i, j].state >= 18 && cellState[i, j].state <= 27))
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
                        cellState[i, j].state = 28;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(shrub2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 29;
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(shrub3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 30;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(shrub4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 31;
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
                if (cellState[i, j].state == 5 || (cellState[i, j].state >= 18 && cellState[i, j].state <= 31))
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
                        cellState[i, j].state = 34;//number in tileset folder
                    }
                    else if (tempValue < 0.4)
                    {
                        Instantiate(rock2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 35;
                    }
                    else if (tempValue < 0.6)
                    {
                        Instantiate(rock3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 36;
                    }
                    else if (tempValue < 0.8)
                    {
                        Instantiate(rock4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 37;
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(prop2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                        cellState[i, j].state = 33;
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
                if (cellState[i, j].state == 5 || (cellState[i, j].state >= 18 && cellState[i, j].state <= 37))
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
                    cellState[i, j].state = 32;//number in tileset folder            
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
        GameObject enemy1 = gameManager.GetEnemy("Slider");
        GameObject enemy2 = gameManager.GetEnemy("SpearThrower");
        GameObject enemy3 = gameManager.GetEnemy("Stomper");
        GameObject enemy4 = gameManager.GetEnemy("SuicideBomber");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j].state == 100)
                {
                    Instantiate(enemy1, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);          
                }
                else if (cellState[i, j].state == 101)
                {
                    Instantiate(enemy2, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 102)
                {
                    Instantiate(enemy3, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 103)
                {
                    Instantiate(enemy4, new Vector3((i + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, (j + Random.Range(-0.5f, 0.5f)) * (float)tileSize / 100, 0), transform.rotation);
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
                if (cellState[i, j].state == 5)
                {
                    if (Random.Range(1, 1000) < 5) {
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
                if (cellState[i, j].state == 5)
                {
                    if (Random.Range(1, 1000) < 5)
                    {
                        if (!isCreated)
                        {
                            player1=Instantiate(player1, new Vector3(i * (float)tileSize / 100, j * (float)tileSize / 100, 0), transform.rotation);
                            //player1.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
                            player1.GetComponent<Movement_New>().WalkSpeed = 5;
                            isCreated = true;
                        }
                    }

                }
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

}
