using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class ProcedualGeneration3_1 : MonoBehaviour {

    private GameManager gameManager;
    public int levelWidth=100;
    public int levelHeight=100;

    private float tileSize;
    private int[,] landArray;
    private int[,] newLandArray;

    private CellState[,] cellState;
    private bool isSimultaneous;
    private float landRatio;
    private float treeRatio;
    private int iteration;

    public GameObject theCanvas;
    //public GameObject textManager;
    private CameraControl cameraControl;
    private int MAX_TILES=1000;
    private float turnRatio = 0.05f;
    private DigState endPoint;

    private Vector2 entrancePosition;
    private Transform player;
    private GameObject portal;
    // Use this for initialization
    void Start () {  
        NextScene.nowName = "3_1";
        cameraControl = GameObject.Find("CameraFollowing").GetComponent<CameraControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

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
        
        //generate up to 2 level platforms
        Dig();
        DrawRoute();
        
        
        GenerateWater();
        SmoothWater();
        Draw();     
        DrawWater();

        GenerateGrass();
        DrawGrass();

        GenerateBuilding();
        DrawBuilding();

        GenerateHole();
        DrawHole();

        GenerateRock();
        DrawRock();

        GenerateTree();
        DrawTree();

        DrawPlayer();

        GenerateLoot();
        DrawLoot();

        GenerateEnemy();
        //DrawEnemy();

        DrawPortal();

        FinishGeneration();
        DrawNPC(); 
        /*
       GameObject tile36 = gameManager.GetTile2("Tile_67");
        for (int i = 1; i < levelWidth; i++)
        {
            for (int j = 1; j < levelHeight; j++)
            {
                //if (cellState[i, j].state >= 19 && cellState[i, j].state <= 35)
                //{
                //Instantiate(tile36, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation); ;
                // }
                if (landArray[i, j] == 1)
                {
                    Instantiate(tile36, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                }
            }
        }
        */

        /*

        GenerateBone();
        DrawBone();
  

        */

    }

    private void Update()
    {
        //Debug.Log(portalPosition);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 portalPosition = portal.transform.position;
            player.position = portalPosition;
        }
    }

    void SmoothWater()
    {
        for (int i = 1; i < levelWidth-1; i++) {
            for (int j = 1; j < levelHeight-1; j++){
                if (landArray[i, j] == 1) {
                    if (landArray[i, j + 1] != 1
                        && landArray[i, j - 1] == 1
                        && landArray[i - 1, j] != 1
                        && landArray[i + 1, j] != 1)
                    {
                        landArray[i, j] = 0;
                    }
                    else if (landArray[i, j + 1] == 1
                        && landArray[i, j - 1] != 1
                        && landArray[i - 1, j] != 1
                        && landArray[i + 1, j] != 1)
                    {
                        landArray[i, j] = 0;
                    }
                    else if (landArray[i, j + 1] != 1
                        && landArray[i, j - 1] != 1
                        && landArray[i - 1, j] == 1
                        && landArray[i + 1, j] != 1)
                    {
                        landArray[i, j] = 0;
                    }
                    else if (landArray[i, j + 1] != 1
                        && landArray[i, j - 1] != 1
                        && landArray[i - 1, j] != 1
                        && landArray[i + 1, j] == 1)
                    {
                        landArray[i, j] = 0;
                    }
                    else if (landArray[i, j + 1] != 1
                        && landArray[i, j - 1] != 1
                        && landArray[i - 1, j] != 1
                        && landArray[i + 1, j] != 1)
                    {
                        landArray[i, j] = 0;
                    }
                }
            }
        }
    }
   
    void CA(float ratio, int iteration,int threshold,int neighborSize, bool isSimultaneous, int targetNum) {
        
        //initialize
        for (int i = 5; i < levelWidth-5; i++)
        {
            for (int j = 5; j < levelHeight-5; j++)
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
            for (int w = 5; w < levelWidth-5; w++)
            {
                for (int h = 5; h < levelHeight-5; h++)
                {
                    if(landArray[w,h]==0|| landArray[w, h] == 1)
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
                    else if (landArray[w, h] == 3)
                    {
                        if (isSimultaneous)
                        {
                            newLandArray[w, h] = 3;
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
        /*
        GameObject tile1 = gameManager.GetTile2("Tile_1");
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 1)
                {
                    Instantiate(tile1, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    cellState[i, j].state = 1;
                }
            }

        }
        */
      
    }

    int DetermineEnemyType() {
        int enemyNum = 0;
        if (Random.value < 0.135f)
        {
            enemyNum = 100;//jumper
        }
        else if (Random.value < 0.27f) {
            enemyNum = 101;//slider
        }
        else if (Random.value < 0.45f)
        {
            enemyNum = 102;//spearThrower
        }
        else if (Random.value < 0.63f)
        {
            enemyNum = 103;//stomper
        }
        else if (Random.value < 0.9f)
        {
            enemyNum = 104;//suicider
        }
        else if (Random.value < 0.95f)
        {
            enemyNum = 105;//shotgunrange
        }
        else if (Random.value < 1.0f)
        {
            enemyNum = 106;//sniper
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
    void Dig() {
        Vector2 startPoint = new Vector2(Random.Range(5,levelWidth-6),Random.Range(5,levelHeight-6));
        DigState dig1 = new DigState();
        dig1.position = startPoint;
        float tempDirection = Random.value;
        if (tempDirection < 0.25) {
            dig1.direction = new Vector2(0, 1);
        }
        else if (tempDirection < 0.5)
        {
            dig1.direction = new Vector2(1, 0);
        }
        else if (tempDirection < 0.75)
        {
            dig1.direction = new Vector2(0, -1);
        }
        else if (tempDirection < 1)
        {
            dig1.direction = new Vector2(-1, 0);
        }
        endPoint = dig1;
        landArray[(int)startPoint.x, (int)startPoint.y] = 3;
        for (int i = 0; i < MAX_TILES; i++) {
            MoveOneTile();
        }
    }
    void MoveOneTile() {
        
        //朝已有方向移动一格
        Vector2 targetPos = endPoint.position + endPoint.direction;
        
        if (ValidateTile(targetPos))
        {
            float tempRange = Random.value;
            if (tempRange < endPoint.turnRatio)//转弯
            {
                float tempDirection = Random.value;
                if (tempDirection < 0.5)
                {
                    endPoint.direction = new Vector2((int)endPoint.direction.y, (int)endPoint.direction.x);
                }
                else
                {
                    endPoint.direction = new Vector2((int)endPoint.direction.y, (int)endPoint.direction.x) * (-1);
                }

                endPoint.turnRatio = 0.025f;
            }
            else//增加几率
            {
                endPoint.turnRatio += 0.025f;
            }
        }

    }
    bool ValidateTile(Vector2 targetPos)
    {
        bool hasMove = true;
        if (targetPos.x == 4 || targetPos.x == levelWidth - 5 || targetPos.y == 4 || targetPos.y == levelHeight - 5)//到边界了
        {
            Vector2 startPoint = new Vector2(Random.Range(5, levelWidth - 6), Random.Range(5, levelHeight - 6));
            DigState dig1 = new DigState();
            dig1.position = startPoint;
            float tempDirection = Random.value;
            if (tempDirection < 0.25)
            {
                dig1.direction = new Vector2(0, 1);
            }
            else if (tempDirection < 0.5)
            {
                dig1.direction = new Vector2(1, 0);
            }
            else if (tempDirection < 0.75)
            {
                dig1.direction = new Vector2(0, -1);
            }
            else if (tempDirection < 1)
            {
                dig1.direction = new Vector2(-1, 0);
            }
            endPoint = dig1;
            landArray[(int)startPoint.x, (int)startPoint.y] = 3;
            hasMove = false;
            //Debug.Log("哲理");
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
       
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
    
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
          
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
        
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
         
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
          
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
         
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
         
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
          
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
         
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
           
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
           
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
        
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 3
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 0)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
         
        }
        else if (landArray[(int)targetPos.x - 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x + 1, (int)targetPos.y] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y + 1] == 0
            && landArray[(int)targetPos.x, (int)targetPos.y - 1] == 3)
        {
            endPoint.position = targetPos;
            landArray[(int)targetPos.x, (int)targetPos.y] = 3;
           
        }
        else//重新开始
        {
            Vector2 startPoint = new Vector2(Random.Range(5, levelWidth - 6), Random.Range(5, levelHeight - 6));
            DigState dig1 = new DigState();
            dig1.position = startPoint;
            float tempDirection = Random.value;
            if (tempDirection < 0.25)
            {
                dig1.direction = new Vector2(0, 1);
            }
            else if (tempDirection < 0.5)
            {
                dig1.direction = new Vector2(1, 0);
            }
            else if (tempDirection < 0.75)
            {
                dig1.direction = new Vector2(0, -1);
            }
            else if (tempDirection < 1)
            {
                dig1.direction = new Vector2(-1, 0);
            }
            endPoint = dig1;
            landArray[(int)startPoint.x, (int)startPoint.y] = 3;
            hasMove = false;
           // Debug.Log("这里");
        }

        return hasMove;
    }
    void GenerateWater() {
        for (int w = 0; w < levelWidth; w++)
        {
            for (int h = 0; h < levelHeight; h++)
            {
                if (w < 5 || h < 5 || w > levelWidth - 6 || h > levelHeight - 6)
                {
                    landArray[w, h] = 1;
                    newLandArray[w, h] = 1;
                    Debug.Log("边界了");
                }
            }
        }
        CA(0.5f, 4, 13, 2,true,1);
    }

    int DetermineCell(int row, int col, int targetCellNum, int threshold, int neighborSize) {
        int found = 0;
        int cellNum = 0;

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
            cellNum = 1;
        else
            cellNum = 0;
           // Debug.Log(cellNum);
        
        return cellNum;
        
    }
    void DrawRoute()
    {
        GameObject tile19 = gameManager.GetTile2("Tile_19");
        GameObject tile20 = gameManager.GetTile2("Tile_20");
        GameObject tile21 = gameManager.GetTile2("Tile_21");
        GameObject tile22 = gameManager.GetTile2("Tile_22");
        GameObject tile23 = gameManager.GetTile2("Tile_23");
        GameObject tile24 = gameManager.GetTile2("Tile_24");
        GameObject tile25 = gameManager.GetTile2("Tile_25");
        GameObject tile26 = gameManager.GetTile2("Tile_26");
        GameObject tile27 = gameManager.GetTile2("Tile_27");
        GameObject tile28 = gameManager.GetTile2("Tile_28");
        GameObject tile29 = gameManager.GetTile2("Tile_29");
        GameObject tile30 = gameManager.GetTile2("Tile_30");
        GameObject tile31 = gameManager.GetTile2("Tile_31");
        GameObject tile32 = gameManager.GetTile2("Tile_32");
        GameObject tile33 = gameManager.GetTile2("Tile_33");
        GameObject tile34 = gameManager.GetTile2("Tile_34");
        GameObject tile35 = gameManager.GetTile2("Tile_35");
        //先初始化cellstate
        for (int i = 1; i < levelWidth - 1; i++)
        {
            for (int j = 1; j < levelHeight - 1; j++)
            {
                if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j - 1] == 3
                    && landArray[i, j + 1] == 0)
                {      
                    cellState[i, j].state = 19;
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {         
                    cellState[i, j].state = 20;
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {        
                    cellState[i, j].state = 21;
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {         
                    cellState[i, j].state = 22;
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {
                    if (Random.value < 0.5)
                    {                
                        cellState[i, j].state = 23;
                    }
                    else
                    {           
                        cellState[i, j].state = 24;
                    }

                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 0)
                {
                    if (Random.value < 0.5)
                    {                    
                        cellState[i, j].state = 25;
                    }
                    else
                    {             
                        cellState[i, j].state = 26;
                    }

                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {
                    cellState[i, j].state = 27;
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 3)
                {
                    cellState[i, j].state = 28;
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 3)
                {
                    cellState[i, j].state = 29;
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                { 
                    cellState[i, j].state = 30;
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {
                    cellState[i, j].state = 31;
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 0)
                {
                    cellState[i, j].state = 32;
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 0)
                {
                     cellState[i, j].state = 34;

                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {
                    cellState[i, j].state = 35;
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 3)
                {
                  
                   cellState[i, j].state = 33;
                   
                }


            }
        }

        for (int i = 1; i < levelWidth-1; i++)
        {
            for (int j = 1; j < levelHeight-1; j++)
            {
                if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j - 1] == 3
                    && landArray[i, j + 1] == 0)
                {
                    if (cellState[i, j - 1].state == 19
                 || cellState[i, j - 1].state == 25
                 || cellState[i, j - 1].state == 26)

                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile19, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 19;
                    }

                    
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i - 1, j].state == 20
                     || cellState[i - 1, j].state == 23
                     || cellState[i - 1, j].state == 24
                     || cellState[i - 1, j].state == 28)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile20, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 20;
                    }
                   
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i + 1, j].state == 21
                    || cellState[i + 1, j].state == 23
                    || cellState[i + 1, j].state == 24)
                    
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile21, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 21;
                    }
                    
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i, j+1].state == 22
                   || cellState[i, j+1].state == 25
                   || cellState[i, j+1].state == 26)

                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile22, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 22;
                    }

                    
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i, j + 1].state == 22
                        || cellState[i, j + 1].state == 25
                        || cellState[i, j + 1].state == 26
                        || cellState[i, j + 1].state == 30
                        || cellState[i, j + 1].state == 31)

                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        if (Random.value < 0.5)
                        {
                            Instantiate(tile23, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 23;
                        }
                        else
                        {
                            Instantiate(tile24, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 24;
                        }
                    }
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i+1, j].state == 21
                        || cellState[i+1, j].state == 23
                        || cellState[i + 1, j].state == 24
                        || cellState[i + 1, j].state == 28
                        || cellState[i + 1, j].state == 30
                        || cellState[i+1, j].state == 35)

                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        if (Random.value < 0.5)
                        {
                            Instantiate(tile25, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 25;
                        }
                        else
                        {
                            Instantiate(tile26, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 26;
                        }

                    }

                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i + 1, j].state == 21
                      || cellState[i + 1, j].state == 23
                      || cellState[i + 1, j].state == 24
                      )
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile27, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 27;
                    }
                    
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i + 1, j].state == 21
                      || cellState[i + 1, j].state == 23
                      || cellState[i + 1, j].state == 24
                      || cellState[i + 1, j].state == 28
                      || cellState[i + 1, j].state == 33)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile28, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 28;
                    }
                    
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i - 1, j].state == 20
                       || cellState[i - 1, j].state == 23
                       || cellState[i - 1, j].state == 24
                       || cellState[i - 1, j].state == 29
                       || cellState[i - 1, j].state == 31
                       || cellState[i - 1, j].state == 33)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile29, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 29;
                    }
                   
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i + 1, j].state == 21
                        || cellState[i + 1, j].state == 23
                        || cellState[i + 1, j].state == 24
                        || cellState[i + 1, j].state == 28
                        || cellState[i + 1, j].state == 30
                        || cellState[i + 1, j].state == 35)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile30, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 30;
                    }
                   
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i - 1, j].state == 20
                        || cellState[i - 1, j].state == 23
                        || cellState[i - 1, j].state == 24
                        || cellState[i - 1, j].state == 31
                        || cellState[i - 1, j].state == 35)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile31, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 31;
                    }
                    
                }
                else if (landArray[i - 1, j] == 3
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i - 1, j].state == 20
                        || cellState[i - 1, j].state == 23
                        || cellState[i - 1, j].state == 24
                        || cellState[i - 1, j].state == 29
                        || cellState[i - 1, j].state == 31
                        || cellState[i - 1, j].state == 32
                        || cellState[i - 1, j].state == 33
                        || cellState[i - 1, j].state == 35)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile32, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 32;
                    }
                    
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 3
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i+1, j].state == 21
                        || cellState[i + 1, j].state == 23
                        || cellState[i + 1, j].state == 24
                        || cellState[i + 1, j].state == 28
                        || cellState[i + 1, j].state == 30
                        || cellState[i + 1, j].state == 33
                        || cellState[i + 1, j].state == 34
                        || cellState[i + 1, j].state == 35)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile34, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 34;
                    }
                    
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 3
                    && landArray[i, j - 1] == 0)
                {
                    if (cellState[i, j + 1].state == 22
                       || cellState[i, j + 1].state == 25
                       || cellState[i, j + 1].state == 26
                       || cellState[i, j + 1].state == 30
                       || cellState[i, j + 1].state == 31
                       || cellState[i, j + 1].state == 32
                       || cellState[i, j + 1].state == 34
                       || cellState[i, j + 1].state == 35)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile35, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 35;
                    }
                   
                }
                else if (landArray[i - 1, j] == 0
                    && landArray[i + 1, j] == 0
                    && landArray[i, j + 1] == 0
                    && landArray[i, j - 1] == 3)
                {
                    if (cellState[i, j - 1].state == 19
                        || cellState[i, j - 1].state == 25 
                        || cellState[i, j - 1].state == 26 
                        || cellState[i, j - 1].state == 28
                        || cellState[i, j - 1].state == 29
                        || cellState[i, j - 1].state == 32
                        || cellState[i, j - 1].state == 33
                        || cellState[i, j - 1].state == 34)
                    {
                        landArray[i, j] = 0;
                        cellState[i, j].state = 0;
                    }
                    else
                    {
                        landArray[i, j] = 3;
                        Instantiate(tile33, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 33;
                    }
                }
               
               
            }
        }

    }
    void DrawWater()
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
        GameObject tile16 = gameManager.GetTile2("Tile_16");
        GameObject tile17 = gameManager.GetTile2("Tile_17");
        //先初始化cellstate
        
        for (int i = 1; i < levelWidth - 1; i++)
        {
            for (int j = 1; j < levelHeight - 1; j++)
            {
                if (landArray[i, j] == 1)
                {
                    if (landArray[i - 1, j] == 1
                        && landArray[i + 1, j] == 1
                        && landArray[i, j + 1] == 1
                        && landArray[i, j - 1] != 1
                        )
                    {
                        if (Random.value < 0.5)
                        {
                            cellState[i, j].state = 2;
                        }
                        else
                        {
                            cellState[i, j].state = 3;
                        }

                    }
                    else if (landArray[i - 1, j] != 1
                        && landArray[i + 1, j] == 1
                        && landArray[i, j + 1] == 1
                        && landArray[i, j - 1] == 1)
                    {
                        if (Random.value < 0.5)
                        {
                            cellState[i, j].state = 4;
                        }
                        else
                        {
                            cellState[i, j].state = 5;
                        }

                    }
                    else if (landArray[i - 1, j] != 1
                        && landArray[i + 1, j] == 1
                        && landArray[i, j + 1] == 1
                        && landArray[i, j - 1] != 1)
                    {
                        cellState[i, j].state = 6;
                    }

                    else if (landArray[i - 1, j] != 1
                        && landArray[i + 1, j] == 1
                        && landArray[i, j + 1] != 1
                        && landArray[i, j - 1] == 1)
                    {
                        cellState[i, j].state = 8;
                    }

                    else if (landArray[i - 1, j] == 1
                        && landArray[i + 1, j] != 1
                        && landArray[i, j + 1] == 1
                        && landArray[i, j - 1] == 1)
                    {
                        if (Random.value < 0.5)
                        {
                            cellState[i, j].state = 10;
                        }
                        else
                        {
                            cellState[i, j].state = 11;
                        }

                    }
                    else if (landArray[i - 1, j] == 1
                        && landArray[i + 1, j] != 1
                        && landArray[i, j + 1] == 1
                        && landArray[i, j - 1] != 1)
                    {
                        cellState[i, j].state = 12;
                    }

                    else if (landArray[i - 1, j] == 1
                        && landArray[i + 1, j] != 1
                        && landArray[i, j + 1] != 1
                        && landArray[i, j - 1] == 1)
                    {
                        cellState[i, j].state = 14;
                    }

                    else if (landArray[i - 1, j] == 1
                        && landArray[i + 1, j] == 1
                        && landArray[i, j + 1] != 1
                        && landArray[i, j - 1] == 1)
                    {
                        if (Random.value < 0.5)
                        {
                            cellState[i, j].state = 16;
                        }
                        else
                        {
                            cellState[i, j].state = 17;
                        }

                    }
                   
                }
            }
        }
        
        for (int i = 1; i < levelWidth - 1; i++)
        {
            for (int j = 1; j < levelHeight - 1; j++)
            {
                if (landArray[i, j] == 1)
                {
                    if (cellState[i, j].state == 2)
                    {
                        Instantiate(tile2, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 3)
                    {
                        Instantiate(tile3, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 4)
                    {
                        Instantiate(tile4, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 5)
                    {
                        Instantiate(tile5, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 6)
                    {
                        Instantiate(tile6, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (
                        (cellState[i - 1, j].state == 2
                        || cellState[i - 1, j].state == 3
                        || cellState[i - 1, j].state == 6
                        )
                        &&
                        (cellState[i, j - 1].state == 4
                        || cellState[i, j - 1].state == 5
                        || cellState[i, j - 1].state == 6
                        )
                        && landArray[i + 1, j] == 1
                        && landArray[i, j + 1] == 1
                        )
                    {
                        Instantiate(tile7, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 8)
                    {
                        Instantiate(tile8, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (
                        (cellState[i - 1, j].state == 8
                        || cellState[i - 1, j].state == 16
                        || cellState[i - 1, j].state == 17
                        )
                        &&
                        (cellState[i, j + 1].state == 4
                        || cellState[i, j + 1].state == 5
                        || cellState[i, j + 1].state == 8
                        )
                        && landArray[i + 1, j] == 1
                        && landArray[i, j - 1] == 1
                        )
                    {
                        Instantiate(tile9, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 10)
                    {
                        Instantiate(tile10, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 11)
                    {
                        Instantiate(tile11, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 12)
                    {
                        Instantiate(tile12, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (
                        (cellState[i + 1, j].state == 2
                        || cellState[i + 1, j].state == 3
                        || cellState[i + 1, j].state == 12
                        )
                        &&
                        (cellState[i, j - 1].state == 10
                        || cellState[i, j - 1].state == 11
                        || cellState[i, j - 1].state == 12
                        )
                        && landArray[i - 1, j] == 1
                        && landArray[i, j + 1] == 1
                        )
                    {
                        Instantiate(tile13, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 14)
                    {
                        Instantiate(tile14, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (
                        (cellState[i + 1, j].state == 14
                        || cellState[i + 1, j].state == 16
                        || cellState[i + 1, j].state == 17
                        )
                        &&
                        (cellState[i, j + 1].state == 10
                        || cellState[i, j + 1].state == 11
                        || cellState[i, j + 1].state == 14
                        )
                        && landArray[i - 1, j] == 1
                        && landArray[i, j - 1] == 1
                        )
                    {
                        Instantiate(tile15, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 16)
                    {
                        Instantiate(tile16, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (cellState[i, j].state == 17)
                    {
                        Instantiate(tile17, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    
                    else
                    {
                        Instantiate(tile1, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 1;
                    }
                    
                }
            }
        }

    }
    void Draw()
    {

        GameObject tile1 = gameManager.GetTile2("Tile_1");
        GameObject tile18 = gameManager.GetTile2("Tile_18");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 0)
                {
                    Instantiate(tile18, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    cellState[i, j].state = 18;
                }
                /*
                else if(landArray[i, j] == 1)
                {
                    Instantiate(tile1, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    cellState[i, j].state = 1;
                }
                */
            }
        }   
    }

  
    void GenerateGrass()
    {
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (landArray[i, j] == 1 || landArray[i, j] == 3)
                    landArray[i, j] = -1;
            }
        }
        CA(0.5f, 1, 4, 1, false, 1);
    }

    void DrawGrass()
    {
       
        GameObject tile36 = gameManager.GetTile2("Tile_36");
        GameObject tile37 = gameManager.GetTile2("Tile_37");
        GameObject tile38 = gameManager.GetTile2("Tile_38");
        GameObject tile39 = gameManager.GetTile2("Tile_39");
        GameObject tile40 = gameManager.GetTile2("Tile_40");
        GameObject tile41 = gameManager.GetTile2("Tile_41");
        GameObject tile42 = gameManager.GetTile2("Tile_42");
        GameObject tile43 = gameManager.GetTile2("Tile_43");
        GameObject tile44 = gameManager.GetTile2("Tile_44");
        GameObject tile45 = gameManager.GetTile2("Tile_45");

        for (int i = 5; i < levelWidth-5; i++)
        {
            for (int j = 5; j < levelHeight-5; j++)
            {
                if (landArray[i, j] == 1)
                {
                    //Debug.Log("a");
                    float tempValue = Random.value;
                    if (tempValue < 0.1)
                    {
                        Instantiate(tile36, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.2)
                    {
                        Instantiate(tile37, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.3)
                    {
                        Instantiate(tile38, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.4)
                    {
                        Instantiate(tile39, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tile40, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.6)
                    {
                        Instantiate(tile41, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.7)
                    {
                        Instantiate(tile42, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.8)
                    {
                        Instantiate(tile43, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 0.9)
                    {
                        Instantiate(tile44, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile45, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                }
            }
        }
    }
    void GenerateBuilding()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if ((landArray[i, j] == 1 || landArray[i, j] == 0) &&
                    (cellState[i, j].state == 18 || (cellState[i, j].state >= 36 && cellState[i, j].state <= 45)))
                    landArray[i, j] = 1;
                else
                    landArray[i, j] = -1;
            }
        }
    }

    void DrawBuilding()
    {
        GameObject tile90 = gameManager.GetTile2("Tile_90");
        GameObject tile91 = gameManager.GetTile2("Tile_91");
        GameObject tile92 = gameManager.GetTile2("Tile_92");
        GameObject tile93 = gameManager.GetTile2("Tile_93");


        for (int i = 5; i < levelWidth - 5; i++)
        {
            for (int j = 5; j < levelHeight - 5; j++)
            {
                bool isAble = false;
                if (landArray[i, j] == 1)
                {
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            if (cellState[i + m, j + n].state >= 19 && cellState[i + m, j + n].state <= 35)
                            {
                                isAble = true;
                            }
                        }
                    }
                    if (isAble)
                    {
                        float tempValue = Random.value;
                        if (tempValue < 0.05)//可以生成
                        {
                            float tempStyle = Random.value;
                            if (tempStyle < 0.05)
                            {
                                Instantiate(tile90, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 90;
                            }
                            else if (tempStyle < 0.1)
                            {
                                Instantiate(tile91, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 91;
                            }
                            else if (tempStyle < 0.15)
                            {
                                Instantiate(tile92, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 92;
                            }
                            else if (tempStyle < 0.2)
                            {
                                Instantiate(tile93, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 93;
                            }
  

                        }
                    }
                    //Debug.Log("a");


                }
            }
        }
    }
    void GenerateHole()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if ((landArray[i, j] == 1 || landArray[i, j] == 0) &&
                    (cellState[i, j].state == 18 || (cellState[i, j].state >= 36 && cellState[i, j].state <= 45)))
                    landArray[i, j] = 1;
                else
                    landArray[i, j] = -1;
            }
        }
    }

    void DrawHole()
    {
        GameObject tile46 = gameManager.GetTile2("Tile_46");
        GameObject tile47 = gameManager.GetTile2("Tile_47");
        GameObject tile48 = gameManager.GetTile2("Tile_48");
        GameObject tile49 = gameManager.GetTile2("Tile_49");
        GameObject tile50 = gameManager.GetTile2("Tile_50");
        GameObject tile51 = gameManager.GetTile2("Tile_51");
        GameObject tile52 = gameManager.GetTile2("Tile_52");
        GameObject tile53 = gameManager.GetTile2("Tile_53");
        GameObject tile54 = gameManager.GetTile2("Tile_54");
        GameObject tile55 = gameManager.GetTile2("Tile_55");
        GameObject tile56 = gameManager.GetTile2("Tile_56");
        GameObject tile57 = gameManager.GetTile2("Tile_57");
        GameObject tile58 = gameManager.GetTile2("Tile_58");
        GameObject tile59 = gameManager.GetTile2("Tile_59");
        GameObject tile60 = gameManager.GetTile2("Tile_60");
        GameObject tile61 = gameManager.GetTile2("Tile_61");
        GameObject tile62 = gameManager.GetTile2("Tile_62");
        GameObject tile63 = gameManager.GetTile2("Tile_63");
        GameObject tile64 = gameManager.GetTile2("Tile_64");
        GameObject tile65 = gameManager.GetTile2("Tile_65");
        GameObject tile66 = gameManager.GetTile2("Tile_66");

        for (int i = 5; i < levelWidth - 5; i++)
        {
            for (int j = 5; j < levelHeight - 5; j++)
            {
                bool isAble = false;
                if (landArray[i, j] == 1)
                {
                    for (int m = -1; m < 2; m++) {
                        for (int n = -1; n < 2; n++) {
                            if (cellState[i + m, j + n].state>=19 && cellState[i + m, j + n].state <= 35) {
                                isAble = true;
                            }
                        }
                    }
                    if (isAble)
                    {
                        float tempValue = Random.value;
                        if (tempValue < 0.05)//可以生成
                        {
                            float tempStyle = Random.value;
                            if (tempStyle < 0.05)
                            {
                                Instantiate(tile46, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 46;
                            }
                            else if (tempStyle < 0.1)
                            {
                                Instantiate(tile47, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 47;
                            }
                            else if (tempStyle < 0.15)
                            {
                                Instantiate(tile48, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 48;
                            }
                            else if (tempStyle < 0.2)
                            {
                                Instantiate(tile49, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 49;
                            }
                            else if (tempStyle < 0.25)
                            {
                                Instantiate(tile50, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 50;
                            }
                            else if (tempStyle < 0.3)
                            {
                                Instantiate(tile51, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 51;
                            }
                            else if (tempStyle < 0.35)
                            {
                                Instantiate(tile52, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 52;
                            }
                            else if (tempStyle < 0.4)
                            {
                                Instantiate(tile53, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 53;
                            }
                            else if (tempStyle < 0.45)
                            {
                                Instantiate(tile54, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 54;
                            }
                            else if (tempStyle < 0.5)
                            {
                                Instantiate(tile55, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 55;
                            }
                            else if (tempStyle < 0.55)
                            {
                                Instantiate(tile56, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 56;
                            }
                            else if (tempStyle < 0.6)
                            {
                                Instantiate(tile57, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 57;
                            }
                            else if (tempStyle < 0.65)
                            {
                                Instantiate(tile58, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 58;
                            }
                            else if (tempStyle < 0.7)
                            {
                                Instantiate(tile59, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 59;
                            }
                            else if (tempStyle < 0.75)
                            {
                                Instantiate(tile60, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 60;
                            }
                            else if (tempStyle < 0.8)
                            {
                                Instantiate(tile61, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 61;
                            }
                            else if (tempStyle < 0.85)
                            {
                                Instantiate(tile62, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 62;
                            }
                            else if (tempStyle < 0.9)
                            {
                                Instantiate(tile63, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 63;
                            }
                            else if (tempStyle < 0.95)
                            {
                                Instantiate(tile64, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 64;
                            }
                            else if (tempStyle < 1)
                            {
                                Instantiate(tile65, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 65;
                            }
                            else
                            {
                                Instantiate(tile66, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                                cellState[i, j].state = 66;
                            }

                        }
                    }
                    //Debug.Log("a");
                    
                    
                }
            }
        }
    }
    void GenerateRock()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if ((landArray[i, j] == 1 || landArray[i, j] == 0) &&
                    ((cellState[i, j].state < 46 && cellState[i, j].state > 35) || cellState[i, j].state == 18))
                {
                    landArray[i, j] = 0;
                    newLandArray[i, j] = 0;
                }

                else
                {
                    landArray[i, j] = -1;
                    newLandArray[i, j] = -1;
                }
                    
            }
        }
      
        CA(0.5f, 1, 8, 1, false, 0);     
    }

    void DrawRock()
    {
        GameObject tile67 = gameManager.GetTile2("Tile_67");
        GameObject tile68 = gameManager.GetTile2("Tile_68");
        GameObject tile69 = gameManager.GetTile2("Tile_69");
        GameObject tile70 = gameManager.GetTile2("Tile_70");
        GameObject tile71 = gameManager.GetTile2("Tile_71");
        GameObject tile72 = gameManager.GetTile2("Tile_72");
        GameObject tile73 = gameManager.GetTile2("Tile_73");
        GameObject tile74 = gameManager.GetTile2("Tile_74");
        GameObject tile75 = gameManager.GetTile2("Tile_75");
        GameObject tile76 = gameManager.GetTile2("Tile_76");

        for (int i = 5; i < levelWidth - 5; i++)
        {
            for (int j = 5; j < levelHeight - 5; j++)
            {
                if (landArray[i,j]==1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.1)
                    {
                        Instantiate(tile67, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 67;
                    }
                    else if (tempValue < 0.2)
                    {
                        Instantiate(tile68, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 68;//number in tileset folder
                    }
                    else if (tempValue < 0.3)
                    {
                        Instantiate(tile69, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 69;//number in tileset folder
                    }
                    else if (tempValue < 0.4)
                    {
                        Instantiate(tile70, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 70;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tile71, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 71;//number in tileset folder
                    }
                    else if (tempValue < 0.6)
                    {
                        Instantiate(tile72, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 72;
                    }
                    else if (tempValue < 0.7)
                    {
                        Instantiate(tile73, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 73;//number in tileset folder
                    }
                    else if (tempValue < 0.8)
                    {
                        Instantiate(tile74, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 74;//number in tileset folder
                    }
                    else if (tempValue < 0.9)
                    {
                        Instantiate(tile75, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 75;//number in tileset folder
                    }
                    else
                    {
                        Instantiate(tile76, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 76;//number in tileset folder
                    }
                }
            }
        }
    }
    void GenerateTree()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if ((landArray[i, j] == 1 || landArray[i, j] == 0) &&
                    ((cellState[i, j].state < 46 && cellState[i, j].state > 35) || cellState[i, j].state == 18))
                {
                    landArray[i, j] = 0;
                    newLandArray[i, j] = 0;
                }

                else
                {
                    landArray[i, j] = -1;
                    newLandArray[i, j] = -1;
                }

            }
        }

        CA(0.5f, 1, 8, 1, false, 0);
    }

    void DrawTree()
    {
        GameObject tile78 = gameManager.GetTile2("Tile_78");
        GameObject tile79 = gameManager.GetTile2("Tile_79");
        GameObject tile80 = gameManager.GetTile2("Tile_80");
        GameObject tile81 = gameManager.GetTile2("Tile_81");
        GameObject tile82 = gameManager.GetTile2("Tile_82");
        GameObject tile83 = gameManager.GetTile2("Tile_83");
        GameObject tile84 = gameManager.GetTile2("Tile_84");
        GameObject tile85 = gameManager.GetTile2("Tile_85");
        GameObject tile86 = gameManager.GetTile2("Tile_86");
        GameObject tile87 = gameManager.GetTile2("Tile_87");
        GameObject tile88 = gameManager.GetTile2("Tile_88");
        GameObject tile89 = gameManager.GetTile2("Tile_89");

        for (int i = 5; i < levelWidth - 5; i++)
        {
            for (int j = 5; j < levelHeight - 5; j++)
            {
                if (landArray[i, j] == 1)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.083)
                    {
                        Instantiate(tile78, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 78;
                    }
                    else if (tempValue < 0.167)
                    {
                        Instantiate(tile79, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 79;//number in tileset folder
                    }
                    else if (tempValue < 0.250)
                    {
                        Instantiate(tile80, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 80;//number in tileset folder
                    }
                    else if (tempValue < 0.333)
                    {
                        Instantiate(tile81, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 81;//number in tileset folder
                    }
                    else if (tempValue < 0.416)
                    {
                        Instantiate(tile82, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 82;//number in tileset folder
                    }
                    else if (tempValue < 0.499)
                    {
                        Instantiate(tile83, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 83;
                    }
                    else if (tempValue < 0.582)
                    {
                        Instantiate(tile84, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 84;//number in tileset folder
                    }
                    else if (tempValue < 0.665)
                    {
                        Instantiate(tile85, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 85;//number in tileset folder
                    }
                    else if (tempValue < 0.748)
                    {
                        Instantiate(tile86, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 86;//number in tileset folder
                    }
                    else if (tempValue < 0.831)
                    {
                        Instantiate(tile87, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 87;//number in tileset folder
                    }
                     else if (tempValue < 0.914)
                    {
                        Instantiate(tile88, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 88;//number in tileset folder
                    }
                    else
                    {
                        Instantiate(tile89, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        cellState[i, j].state = 89;//number in tileset folder
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
                if (cellState[i, j].state == 200 || cellState[i, j].state == 201)
                {
                    cellState[i, j].state = 0;
                }

            }
        }


        // CA(0.5f, 1, 8, 1, true, 0);
        CA(0.6f, 1, 8, 1, false, 200);
    }

    void DrawBone()
    {
        GameObject tile39 = gameManager.GetTile2("Tile_39");
        GameObject tile40 = gameManager.GetTile2("Tile_40");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (cellState[i, j].state == 201)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.5)
                    {
                        Instantiate(tile39, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        cellState[i, j].state = 39;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile40, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        cellState[i, j].state = 40;//number in tileset folder
                    }
                }
            }
        }
    }


    void GenerateLoot()
    {


        // CA(0.5f, 1, 8, 1, true, 0);
        CA(0.6f, 1, 8, 1, false, 0);
       
    }

    void DrawLoot()
    {
        GameObject tile77 = gameManager.GetTile2("Tile_77");
       

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (landArray[i,j] == 1)
                {
                    Instantiate(tile77, new Vector3(i*tileSize,j*tileSize, 0), transform.rotation);
                    cellState[i, j].state = 77;//number in tileset folder           
                }
            }
        }
    }

    void GenerateEnemy()
    {

        for (int i = 5; i < levelWidth-5; i++)
        {
            for (int j = 5; j< levelHeight-5; j++)
            {
                if (landArray[i,j]==0||landArray[i,j]==1)
                {
                    float ratio = (float)Random.Range(1, 50);
                    bool isClose = false;
                    if (ratio < 4)
                    {
                        
                        for (int m = -2; m < 3; m++) {
                            for (int n = -2; n < 3; n++) {
                                if (cellState[i+m, j+n].state <= 17|| 
                                    (cellState[i + m, j + n].state <= 77&& cellState[i + m, j + n].state >= 46))
                                    isClose = true;
                            }
                        }
                        
                        for (int m = -5; m < 6; m++)
                        {
                            for (int n = -5; n < 6; n++)
                            {
                                if (cellState[i + m, j + n].state==300)
                                    isClose = true;
                            }
                        }
                        if (!isClose) {
                           // landArray[i, j] = -1;
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
        GameObject enemy6 = gameManager.GetEnemy("ShotRange");
        GameObject enemy7 = gameManager.GetEnemy("Sniper");

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (cellState[i, j].state == 100)
                {
                    Instantiate(enemy1, new Vector3(i*tileSize, j*tileSize, 0), transform.rotation);          
                }
                else if (cellState[i, j].state == 101)
                {
                    Instantiate(enemy2, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 102)
                {
                    Instantiate(enemy3, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 103)
                {
                    Instantiate(enemy4, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 104)
                {
                    Instantiate(enemy5, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 105)
                {
                    Instantiate(enemy6, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 106)
                {
                    Instantiate(enemy7, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
            }
        }
    }

    void DrawPortal()
    {
        portal = gameManager.GetPortal("Portal_4");
        Vector2 maxPosition = new Vector2(0, 0);
        float maxDistance = 0f;
        bool isClose = false;
        for (int i =5; i < levelWidth-5; i++)
        {
            for (int j = 5; j < levelHeight-5; j++)
            {
                if (landArray[i, j] == 0 || landArray[i, j] == 1)
                {
                    isClose = false;
                    for (int m = -2; m < 3; m++)
                    {
                        for (int n = -2; n < 3; n++)
                        {
                            if (cellState[i + m, j + n].state <= 17 ||
                                    (cellState[i + m, j + n].state <= 77 && cellState[i + m, j + n].state >= 46))
                                isClose = true;
                        }
                    }
                    float tempDistance = Mathf.Abs(entrancePosition.x - i) + Mathf.Abs(entrancePosition.y - j);
                    if (tempDistance > maxDistance && !isClose) {
                        maxDistance = tempDistance;
                        maxPosition = new Vector2(i*tileSize,j*tileSize);
                    }
            
                }
            }
        }

        portal=Instantiate(portal, new Vector3(maxPosition.x, maxPosition.y, 0), transform.rotation);

    }

    void DrawPlayer()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if ((landArray[i, j] == 1 || landArray[i, j] == 0) &&
                    ((cellState[i, j].state <= 45 && cellState[i, j].state >= 18)))
                {
                    landArray[i, j] = 0;
                }

                else
                {
                    landArray[i, j] = -1;
                }

            }
        }

        GameObject player1 = gameManager.Player;
        Vector2 position = new Vector2(Random.Range(5,levelWidth-5),Random.Range(5,levelHeight-5));
        bool isEmptySpace = false;
        while (!isEmptySpace)
        {
            for (int m = -1; m < 2; m++)
            {
                for (int n = -1; n < 2; n++)
                {
                    if (landArray[(int)position.x + m, (int)position.y + n] == 0)
                    {
                        isEmptySpace = true;
                        position.x = position.x + m;
                        position.y = position.y + n;
                        break;
                    }

                }
            }
            if (!isEmptySpace) {
                position = new Vector2(Random.Range(5, levelWidth - 5), Random.Range(5, levelHeight - 5));
            }
        }

        entrancePosition = position;
        player1 = Instantiate(player1, new Vector3(position.x*tileSize, position.y*tileSize, 0), transform.rotation);
        cellState[(int)position.x, (int)position.y].state=300;
        player1.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
        player1.GetComponent<Movement_New>().WalkSpeed = 150;
        player1.GetComponent<Shoot_New>().BulletSizeUp = 15;
        player1.GetComponentInChildren<PickUp_New>().WeaponSizeUp = 15;
        player1.GetComponentInChildren<PickUp_New>().ItemSizeUp = 15;
        player = player1.transform;
        LoadPlayerData(player1);
        
    }

    void LoadPlayerData(GameObject player)
    {
        player.AddComponent<LoadForPlayer>();
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

        bool isClose = false;
        for (int i = 5; i < levelWidth-5; i++)
        {
            for (int j = 5; j < levelHeight-5; j++)
            {
                if (landArray[i, j] == 0 || landArray[i, j] == 1)
                {
                    isClose = false;
                    for (int m = -2; m < 3; m++)
                    {
                        for (int n = -2; n < 3; n++)
                        {
                            if (cellState[i + m, j + n].state <= 17 ||
                                    (cellState[i + m, j + n].state <= 77 && cellState[i + m, j + n].state >= 46))
                                isClose = true;
                        }
                    }
                    if (Random.Range(1, 100) < 2 && !isClose && !isCreated)
                    {
                        Instantiate(NPCObject, new Vector3(i*tileSize, j*tileSize, 0), transform.rotation);
                        // player1.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
                        // player1.GetComponent<Movement_New>().WalkSpeed = 5;
                        isCreated = true;
                    }
                }
            }
        }

    }

    void FinishGeneration() {
        theCanvas.SetActive(true);
    }

}
