using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class ProcedualGeneration2_2 : MonoBehaviour {

    private GameManager gameManager;
    public int levelWidth=200;
    public int levelHeight=200;

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
    private int MIN_TILES=1000;
 
    private int tilesPlaced;
	private int tilesToProcess;
	private int adjacentCells;
	private int startX;
	private int startY;
	private float turnRatio=0.25f;
    private int[] direction;

    private Vector2 entrancePosition;
    private Transform player;
    private GameObject portal;
    public int lootCount = 0;
    public int enemyCount = 0;
    public float levelTime = 0;
    // Use this for initialization
    void Start () {
        isEdgeReady = false;    
        NextScene.nowName = "2_2";
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
        Terrain();
        for (int i = 0; i < 3; i++) {
            Smooth();
        }
       
        Draw();      
        ChangeEdge();
        //ReCaculateLandArray();
        
        FindTheDown();
        for(int i=0;i<360;i++)
        {
        //while (!isEdgeReady) {
            DrawEdge();
        }
           
        //}
        Connect();
        ChangeColor();
        ChangeLandArrayPosition();
        
        
        GenerateGrass();
        DrawGrass();

        GenerateRock();
        DrawRock();

        GenerateBone();
        DrawBone();

        GenerateLoot();
        DrawLoot();

        AddEntrance();
        DrawPlayer();

        GenerateEnemy();
        DrawEnemy();
        DrawPortal();
        FinishGeneration();

        DrawNPC();
        
      
    }

    private void Update()
    {
        levelTime += Time.deltaTime;
        //Debug.Log(portalPosition);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 portalPosition = portal.transform.position;
            player.position = portalPosition;
        }
    }
    void Terrain()
    {
        GenerateCave();
    }
    void Smooth()
    {
        for (int i = 1; i < levelWidth-1; i++) {
            for (int j = 1; j < levelHeight-1; j++){
                /*
                if (landArray[i, j] == 1)
                {
                    int totalNum = 0;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            if (landArray[i + m, j + n] == 0)
                                totalNum++;
                        }
                    }
                    if (totalNum > 4)
                    {
                        landArray[i, j] = 0;
                    }
                }
                */
                if (landArray[i, j] == 0) {
                    int totalNum = 0;
                    for (int m = -1; m < 2; m++) {
                        for (int n = -1; n < 2; n++) {
                            if (landArray[i + m, j + n] == 0)
                                totalNum++;
                        }
                    }
                    if (totalNum < 4) {
                        landArray[i, j] = 1;
                    }
                }
                
                
                if (landArray[i, j + 1] == 1 && landArray[i, j - 1] == 1 && landArray[i + 1, j] == 0 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i-1, j+1] == 0
                   && landArray[i+1, j+1] == 1 && landArray[i-1, j-1] == 0 && landArray[i+1, j-1] == 0)
                    landArray[i, j-1] = 0;//011000010 把缺少的一个横快补上
                if (landArray[i, j + 1] == 0 && landArray[i, j - 1] == 0 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j] == 1 && landArray[i, j] == 0)
                    landArray[i, j] = 1;//111001011
                if (landArray[i, j] == 0 && landArray[i-1, j] == 0 && landArray[i + 1, j] == 1 &&
                   landArray[i, j+1] == 1 && landArray[i, j-1] == 1 && landArray[i - 1, j+1] == 0 &&
                   landArray[i - 1, j - 1] == 0)
                    landArray[i, j] = 1;//如果是突出去的左边，就消除那个格子
                if (landArray[i, j] == 0 && landArray[i - 1, j] == 1 && landArray[i + 1, j] == 0 &&
                   landArray[i, j + 1] == 1 && landArray[i, j - 1] == 1 && landArray[i + 1, j + 1] == 0 &&
                   landArray[i + 1, j - 1] == 0)
                    landArray[i, j] = 1;//如果是突出去的右边，就消除那个格子
                if (landArray[i-1, j+1] == 1 && landArray[i, j+1] == 0 && landArray[i + 1, j+1] == 0 &&
                   landArray[i-1, j] == 1 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j-1] == 1 && landArray[i + 1, j-1] == 1)
                    landArray[i-1, j-1] = 1;//如果是突出去一块连接一个区域，去掉那个块
                if (landArray[i - 1, j + 1] == 1 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 1)
                    landArray[i + 1, j] = 1;//如果是突出去一块连接一个区域，去掉那个块
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 1 &&
                  landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                  landArray[i - 1, j - 1] == 1 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉那个区域
                    landArray[i-1, j] = 1;
                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 1 &&
                  landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                  landArray[i - 1, j - 1]>-1 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉一块
      
                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 0 &&
                  landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                  landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉一块

                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 1 &&
                 landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                 landArray[i - 1, j - 1] == 1 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉区域
                    landArray[i-1, j] = 1;
                    landArray[i, j+1] = 1;

                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 0 &&
                  landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                  landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉一块

                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 0 &&
                  landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                  landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 0 && landArray[i+1, j - 1] == 1)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉一块

                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                 landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                 landArray[i - 1, j - 1] == 1 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i-1, j] = 1;//如果是突出去一块连接一个区域，去掉桥接部分
                    landArray[i - 1, j+1] = 1;
                }

                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                 landArray[i - 1, j] == 1 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                 landArray[i - 1, j - 1] == 1 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉桥接部分
                    
                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                 landArray[i - 1, j] == 1 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                 landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                {
                    landArray[i-1, j+1] = 1;//如果是突出去一块连接一个区域，去掉桥接部分

                }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                 landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 0 &&
                 landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                {
                    landArray[i, j] = 1;//如果是突出去一块连接一个区域，去掉桥接部分

                }
            }
        }
    }
    void GenerateCave(){
        direction = new int[4]{ 0, 1, 2, 3 };
        tilesPlaced=1;
		startX=Mathf.RoundToInt(levelWidth/2);
		startY=Mathf.RoundToInt(levelHeight/2);
		while (tilesPlaced<MIN_TILES) {  
            tilesToProcess = 1;
            DrawCave();
            do
            {
                startX = Mathf.FloorToInt(Random.Range(2, levelWidth - 2 ));
                startY = Mathf.FloorToInt(Random.Range(2, levelHeight - 2));
            } while (!HasFreeAdjacents());  
		}
	}
    void DrawCave(){
        //drawTile(startX, startY);

        landArray[startX, startY] = 1;
        ArrayList xCoordsArray = new ArrayList
        {
            startX
        };
        ArrayList yCoordsArray = new ArrayList
        {
            startY
        };
        int[] xOffset =new int[4]{ -1, 0, 1, 0 };
        int[] yOffset = new int[4] { 0, -1, 0, 1 };
		while (tilesToProcess>0) {
		    int currentX = (int)xCoordsArray[xCoordsArray.Count-1];
            xCoordsArray.RemoveAt(xCoordsArray.Count - 1);
            int currentY = (int)yCoordsArray[yCoordsArray.Count - 1];
            yCoordsArray.RemoveAt(yCoordsArray.Count - 1);
            adjacentCells=GetAdjacentCells(tilesToProcess);
			if (adjacentCells>0) {
			    if (Random.value<turnRatio) {
				    direction=Shuffle(direction);
                }
				for (int j=0; j<4; j++) {
				    int adjacentX=currentX+xOffset[direction[j]];
					int adjacentY=currentY+yOffset[direction[j]];
					if (adjacentX>=2 && adjacentX<levelWidth-2 && 
                        adjacentY>=2 && adjacentY<levelHeight-2&&
                        landArray[adjacentX,adjacentY]==0)
                    {
					    xCoordsArray.Add(adjacentX);
						yCoordsArray.Add(adjacentY);
						tilesPlaced++;
                        //DrawTile(adjacentX, adjacentY);
                        
                        landArray[adjacentX, adjacentY] = 1;
                        //Debug.Log(landArray[adjacentX, adjacentY]);
                        tilesToProcess++;
						adjacentCells--;
						if (adjacentCells==0) {
						    break;
						}
					}
				}
			}
			tilesToProcess--;
		}
	}

    int GetAdjacentCells(int num) {
	    int wayOuts=Mathf.RoundToInt(Mathf.Floor(Random.Range(0,4))/2+0.1f);
		if (num==1 && wayOuts==0) {
		    wayOuts=1;
		}
        return wayOuts;
	}
    int[] Shuffle(int[] startArray){
        //int[] suffledArray = new int[startArray.Length];
        for (int i = 0; i < startArray.Length; i++) {
            int temp = startArray[i];
            int randomIndex = Random.Range(0, startArray.Length);
            startArray[i] = startArray[randomIndex];
            startArray[randomIndex] = temp;
        }
		return startArray;
	}
    bool HasFreeAdjacents(){
		if (landArray[startX,startY]==0) {
			return false;
		}
        if (startX < 2 || startX > levelWidth - 3 || startY < 2 || startY > levelHeight - 3) {
            return false;
        }
		if (startY+1<levelHeight && landArray[startX, startY+1] == 0) {
			return true;
		}
		if (startY - 1 >= 0 && landArray[startX, startY-1] == 0) {
			return true;
		}
		if (startX + 1 < levelWidth && landArray[startX + 1,startY]==0) {
			return true;
		}
		if (startX - 1 >= 0 && landArray[startX - 1,startY]==0) {
			return true;
		}
		return false;
	}



    void CA(float ratio, int iteration,int threshold,int neighborSize, bool isSimultaneous, int targetNum) {
        //initialize
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 2 && cellState[i,j].state==0)
                {

                    if (Random.value < ratio) {
                        //Debug.Log("200");
                        cellState[i, j].state = 201;
                    }
                        
                    else
                        cellState[i, j].state = 200;
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
                    if (cellState[w, h].state == 200 || cellState[w, h].state == 201)
                    {
                        if (w == 0 || h == 0 || w == levelWidth - 1 || h == levelHeight - 1)
                        {
                            cellState[w, h].state = 200;
                           // Debug.Log("边界了");
                        }
                        else
                        {
                            if (!isSimultaneous)
                            {
                                cellState[w, h].state = DetermineCell(w, h, targetNum, threshold, neighborSize);
                            }
                            else
                            {
                                newLandArray[w, h] = DetermineCell(w, h, targetNum, threshold, neighborSize);
                            }
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
                        cellState[w, h].state = newLandArray[w, h];
                    }
                }
            }


        }

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
    void Generate(int currentLevel) {
        CA(0.5f, 8, 4, 1,false,1);
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
                if (cellState[x, y].state == targetCellNum)
                found++;
            }
        }
        //Debug.Log("found"+found);
        if (found > threshold)
            cellNum = 201;
        else
            cellNum = 200;
           // Debug.Log(cellNum);
        
        return cellNum;
        
    }

    void Draw() {

        GameObject tile0 = gameManager.GetTile2("Tile_0");
        GameObject tile46 = gameManager.GetTile2("Tile_46");
       
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
                
                if (landArray[i, j] == 0)
                {
                   // Instantiate(tile46, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
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
        GameObject tile99 = gameManager.GetTile2("99");
        GameObject tile100 = gameManager.GetTile2("100");
        GameObject tile101 = gameManager.GetTile2("101");
        GameObject tile102 = gameManager.GetTile2("102");
        GameObject tile103 = gameManager.GetTile2("103");
        GameObject tile104 = gameManager.GetTile2("104");
        GameObject tile105 = gameManager.GetTile2("105");
        GameObject tile106 = gameManager.GetTile2("106");
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                if (landArray[i, j] == 0)
                {
                    if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                   && landArray[i, j + 1] == 0 && landArray[i, j - 1] == 1 && 
                   landArray[i - 1, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    {
                        edgeArray[i, j] = 1;//下底边
                                            //Debug.Log("辖地变");
                       // Instantiate(tile99, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }

                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j - 1] == 1 && landArray[i, j + 1] == 0 
                    &&
                    (
                    (landArray[i + 1, j + 1] == 0 && landArray[i - 1, j - 1] == 1)
                    ||
                    (landArray[i + 1, j + 1] == 1 && landArray[i - 1, j - 1] == 1 && landArray[i - 1, j+1] == 0 && landArray[i + 1, j - 1] == 0)              
                    )
                    )
                        
                    {
                        edgeArray[i, j] = 3;//左下角
                      ///  Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j - 1] == 1 && landArray[i, j + 1] == 0
                    && 
                    (
                    (landArray[i + 1, j - 1] == 1 && landArray[i - 1, j + 1] == 0) 
                    ||
                    (landArray[i - 1, j - 1] == 0 && landArray[i + 1, j - 1] == 0 && landArray[i + 1, j] == 0)
                    ||
                    (landArray[i - 1, j - 1] == 0 && landArray[i + 1, j + 1] == 0 && landArray[i - 1, j+1] == 1 && landArray[i + 1, j - 1] == 1)
                    )
                    )
                    {
                        edgeArray[i, j] = 2;//右下角
                        //Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                   && landArray[i, j - 1] == 0 && landArray[i, j + 1] == 1 && 
                   landArray[i - 1, j + 1] == 1 && landArray[i + 1, j + 1] == 1)
                    {
                        edgeArray[i, j] = 7;//上底边
                       // Instantiate(tile102, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j + 1] == 1 && landArray[i, j - 1] == 0 
                    && 
                    (
                    (landArray[i + 1, j - 1] == 0 && landArray[i - 1, j + 1] == 1)
                    ||
                    (landArray[i + 1, j + 1] == 0 && landArray[i - 1, j + 1] == 0 && landArray[i - 1, j] == 0)
                    ||
                    (landArray[i + 1, j + 1] == 0 && landArray[i - 1, j - 1] == 0 && landArray[i - 1, j+1] == 1 && landArray[i + 1, j - 1] == 1)
                    )
                    )
                    {
                        edgeArray[i, j] = 8;//左上角
                       // Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }

                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j + 1] == 1 && landArray[i, j - 1] == 0 
                    &&
                    (
                    (landArray[i - 1, j - 1] == 0 && landArray[i + 1, j + 1] == 1)
                    ||
                    (landArray[i + 1, j - 1] == 1 && landArray[i - 1, j + 1] == 0 && landArray[i - 1, j-1] == 1 && landArray[i + 1, j + 1] == 1)          
                    )
                    ) 
                    {
                        edgeArray[i, j] = 6;//右上角
                       // Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i - 1, j] == 1 && landArray[i + 1, j] == 0)
                    {
                        edgeArray[i, j] = 4;//右边
                        //Instantiate(tile106, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i + 1, j] == 1 && landArray[i - 1, j] == 0)
                    {
                        edgeArray[i, j] = 5;//左边
                       // Instantiate(tile105, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                }
                else
                {
                    edgeArray[i, j] = 0;
                }
            }//end for col
        }//end for row
    }//end Changeedge
    void FindTheDown()
    {
        bool isFind = false;
        bool isScan = false;
        for (int j = 0; j < levelHeight; j++)
        {
            for (int i = levelWidth-1; i >= 0; i--)
            {

                if (edgeArray[i, j] == 7 && !isFind)//上底边
                {
                    isFind = true;
                    isScan = true;
                    cellState[i, j].state = -1;//最下底边
                    cellState[i, j].isFirstTop = true;

                }
                else if (edgeArray[i, j] == 7 && isScan) {
                    cellState[i, j].state = -1;
                }
                else if (edgeArray[i, j] != 7 && isScan)
                {
                    isScan = false;
                }

            }
        }
    }

    public void Connect()
    {
        GameObject tile13 = gameManager.GetTile2("Tile_14");
        GameObject tile16 = gameManager.GetTile2("Tile_16");
        Vector2 firstTopPosition = new Vector2(-100, -100);
        Vector2 lastPosition = new Vector2(-100, -100);
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (cellState[i, j].state == 16 && cellState[i, j].isFirstTop) {
                    firstTopPosition = cellState[i, j].position;
                    if (cellState[i + 1, j - 1].state != 0)//右下角
                    {
                        lastPosition = cellState[i + 1, j - 1].position;
                    }
                    else if (cellState[i + 1, j + 1].state != 0)//右上角
                    {
                        lastPosition = cellState[i + 1, j + 1].position;
                    }
                    else if (cellState[i + 1, j].state != 0)//右边
                    {
                        lastPosition = cellState[i + 1, j].position;
                    }
                    else if (cellState[i, j - 1].state != 0)//下边
                    {
                        lastPosition = cellState[i, j - 1].position;
                    }
                    
                    else//检测同列的
                    {
                        int n = j + 1;
                        while (n < levelHeight) {
                            if (cellState[i, n].state != 0)
                            {
                                lastPosition = cellState[i, n].position;
                                break;
                            }
                            else {
                                n++;
                            }
                        }
                    }
                }
            }
        }
        float xDistance = lastPosition.x - firstTopPosition.x;
        float yDistance = lastPosition.y - firstTopPosition.y;
        int rightTileNum = 0;
        int topTileNum = 0;
        //看横向差距
        if (xDistance <= 24 && xDistance > 0)
        {
            topTileNum = 0;
        }
        else if (xDistance > 24)
        {
            xDistance = xDistance - 24;
            topTileNum = (int)xDistance / 24 + 1;
        }
 
        //看纵向差距
        if (yDistance <= 29 && yDistance > 0)
        {
            rightTileNum = 1;
        }
        else if (yDistance > 29)
        {
            yDistance = yDistance - 29;
            rightTileNum = (int)yDistance / 24 + 2;
        }
        else if (yDistance < 0 && yDistance >= -29)
        {
            rightTileNum = -1;
        }
        else if (yDistance < -29)
        {
            yDistance = yDistance + 29;
            rightTileNum = (int)yDistance / 24 - 2;
        }
        //画横向
        if (topTileNum > 0)
        {
            for (int i = 1; i <= topTileNum; i++)
            {
                Vector2 position = firstTopPosition;
                position.x = position.x + 24*i;
                
                Instantiate(tile16, new Vector3(position.x, position.y, 0), transform.rotation);
            }
        }
        //画纵向
        if (rightTileNum > 0)
        {
            for (int i = 0; i < rightTileNum; i++)
            {
                Vector2 position = firstTopPosition;
                position.x = position.x + 14+24*topTileNum;
                position.y = position.y + 24 * (i+1);
                Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
            }
        }
        else if (rightTileNum < 0)
        {
            for (int i = 0; i > rightTileNum; i--)
            {
                Vector2 position = firstTopPosition;
                position.x = position.x + 14 + 24 * topTileNum; ;
                position.y = position.y - 7 + 24 * (i);
                Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
            }
        }
    }

    public void ChangeColor() {
        GameObject tile10 = gameManager.GetTile2("Tile_10");
        GameObject tile42 = gameManager.GetTile2("Tile_42");
        GameObject tile43 = gameManager.GetTile2("Tile_43");
        bool hasWall = false;

        Vector2 startPosition=new Vector2(0,0);
        Vector2 endPosition = new Vector2(0, 0);
        //粗上色
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                
                if (j == 0)//初始化
                {
                    hasWall = false;
                    startPosition = new Vector2(i*tileSize,0);
                    endPosition = new Vector2(i * tileSize, levelHeight * tileSize);
                }
                if (cellState[i, j].state != 0 && landArray[i, j - 1] == 0) {
                    hasWall = true; 
                    endPosition = cellState[i, j].position;         
                }
                else if (cellState[i, j].state != 0 && landArray[i, j + 1] == 0)
                {

                    startPosition = cellState[i, j].position;
                    startPosition.y += 24f;
                 
               
                }
                else {
                    hasWall = false;
                }
                if (hasWall) {
                  int tileNum = (int)(endPosition.y - startPosition.y) / (int)tileSize;
                  for (int m = 0; m < tileNum; m++)
                  {
                    Instantiate(tile10, new Vector3(i * tileSize, m * tileSize+startPosition.y, 0), transform.rotation);
                  }
                    
                }
                if (j == levelHeight - 1) {
                    endPosition = new Vector2(i * tileSize, levelHeight * tileSize);
                    int tileNum = (int)(endPosition.y - startPosition.y) / (int)tileSize;
                    for (int m = 0; m < tileNum; m++)
                    {
                        Instantiate(tile10, new Vector3(i * tileSize, m * tileSize + startPosition.y, 0), transform.rotation);
                    }
                }

            }
        }
        //细上色
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {
                Vector2 position = cellState[i, j].position;
                if (cellState[i, j].state == 6)//双右上边
                {
                    for (int m = 0; m < 4; m++) {
                        for (int n=-2; n < 2; n++){
                            Instantiate(tile42, new Vector3(position.x+m*0.5f*tileSize, position.y + n * 0.5f * tileSize, 0), transform.rotation);
                        }
                    }
                    Instantiate(tile42, new Vector3(position.x, position.y + tileSize, 0), transform.rotation);
                    Instantiate(tile42, new Vector3(position.x+0.5f*tileSize, position.y + tileSize, 0), transform.rotation);
                    Instantiate(tile42, new Vector3(position.x+tileSize, position.y + tileSize, 0), transform.rotation);

                    Instantiate(tile42, new Vector3(position.x, position.y + 1.5f*tileSize, 0), transform.rotation);
                    Instantiate(tile42, new Vector3(position.x + 0.5f * tileSize, position.y + 1.5f*tileSize, 0), transform.rotation);

                    Instantiate(tile42, new Vector3(position.x, position.y + 2f * tileSize, 0), transform.rotation);

                    Instantiate(tile43, new Vector3(position.x+tileSize, position.y + 1.5f * tileSize, 0), transform.rotation);
                    Instantiate(tile43, new Vector3(position.x, position.y + 2.5f * tileSize, 0), transform.rotation);
                }

                else if (cellState[i, j].state == 17)//单右上边
                {
                    for (int m = 0; m < 2; m++)
                    {
                        for (int n = -2; n < 2; n++)
                        {
                            Instantiate(tile42, new Vector3(position.x + m * 0.5f * tileSize, position.y + n * 0.5f * tileSize, 0), transform.rotation);
                        }
                    }
                    Instantiate(tile42, new Vector3(position.x, position.y + tileSize, 0), transform.rotation);

                    Instantiate(tile43, new Vector3(position.x, position.y + 1.5f*tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 8)//双左上边
                {
                    for (int m = 0; m < 4; m++)
                    {
                        for (int n = -2; n < 2; n++)
                        {
                            Instantiate(tile42, new Vector3(position.x + m * 0.5f * tileSize, position.y + n * 0.5f * tileSize, 0), transform.rotation);
                        }
                    }
                    Instantiate(tile42, new Vector3(position.x + 1.5f * tileSize, position.y + tileSize, 0), transform.rotation);
                    Instantiate(tile42, new Vector3(position.x + 0.5f * tileSize, position.y + tileSize, 0), transform.rotation);
                    Instantiate(tile42, new Vector3(position.x + tileSize, position.y + tileSize, 0), transform.rotation);

                    Instantiate(tile42, new Vector3(position.x + 1.5f * tileSize, position.y + 1.5f * tileSize, 0), transform.rotation);
                    Instantiate(tile42, new Vector3(position.x + tileSize, position.y + 1.5f * tileSize, 0), transform.rotation);

                    Instantiate(tile42, new Vector3(position.x + 1.5f * tileSize, position.y + 2f * tileSize, 0), transform.rotation);

                    Instantiate(tile43, new Vector3(position.x + 0.75f*tileSize, position.y + 1.5f * tileSize, 0), transform.rotation);
                    Instantiate(tile43, new Vector3(position.x + 1.75f*tileSize, position.y + 2.5f * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 15)//单左上边
                {
                    for (int m = 0; m < 2; m++)
                    {
                        for (int n = -2; n < 2; n++)
                        {
                            Instantiate(tile42, new Vector3(position.x + m * 0.5f * tileSize, position.y + n * 0.5f * tileSize, 0), transform.rotation);
                        }
                    }
                    Instantiate(tile42, new Vector3(position.x + 0.5f * tileSize, position.y + tileSize, 0), transform.rotation);

                    Instantiate(tile43, new Vector3(position.x + 0.75f * tileSize, position.y + 1.5f * tileSize, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 16)//上边
                {
                    for (int m = 0; m < 2; m++)
                    {
                        for (int n = -2; n < 1; n++)
                        {
                            Instantiate(tile42, new Vector3(position.x + m * 0.5f * tileSize, position.y + n * 0.5f * tileSize, 0), transform.rotation);
                        }
                    }
                    for (int m = 0; m < 4; m++)
                    {
                        Instantiate(tile43, new Vector3(position.x + m * 0.25f * tileSize, position.y + 0.5f * tileSize, 0), transform.rotation);
                    }
                }
            }
        }
    }

    public void ReCaculateLandArray() {
        int areaNum=0;
        bool hasEdge = false;
        bool isScan = false;

        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (j == 0) {
                    hasEdge = false;
                    
                }
                if (edgeArray[i, j] != 0 && !isScan) {
                    areaNum += 1;
                    isScan = true;
                    hasEdge = true;
                    Debug.Log("0");
                    if (areaNum > 1) {
                        Debug.Log("超过了");
                    }
                }
                if (areaNum == 1 && isScan)
                {
                    if (edgeArray[i, j] !=0)
                        hasEdge = true;
                }
                if (j == levelHeight - 1) {
                    if (!hasEdge)
                        isScan = false;
                }
               
            }
        }
       
    }
    public void ChangeLandArrayPosition() {
        GameObject tile43 = gameManager.GetTile2("Tile_22");
        Vector2 startPosition = new Vector2(0, 0);
        Vector2 endPosition = new Vector2(0, 0);
        Vector2 start = new Vector2(0, 0);
        Vector2 end = new Vector2(0, 0);
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelHeight; j++)
            {

                if (j == 0)//初始化
                {

                    startPosition = new Vector2(i * tileSize, 0);
                    endPosition = new Vector2(i * tileSize, levelHeight * tileSize);
                    start = new Vector2(i,0);
                    end= new Vector2(i, levelHeight-1);
                }
                if (cellState[i, j].state != 0 && landArray[i, j + 1] == 1)
                {
            
                    startPosition = cellState[i, j].position;
                    startPosition.y += 24f;
                    start = new Vector2(i, j);
                }
                else if (cellState[i, j].state != 0 && landArray[i, j - 1] == 1)
                {
                    endPosition = cellState[i, j].position;
                    end = new Vector2(i, j);
                    float yDistance = endPosition.y - startPosition.y-tileSize;
                    int middleTiles = (int)end.y - (int)start.y-1;
                    float middleTileSize = yDistance / middleTiles;
                    for (int n = (int)start.y + 1; n < (int)end.y; n++) {
                        landArray[i, n] = 2;
                        cellState[i, n].position = new Vector2(i * tileSize,startPosition.y+24f+(n-start.y-1)*middleTileSize);
                       // Instantiate(tile43, new Vector3(cellState[i, n].position.x, cellState[i, n].position.y, 0), transform.rotation);
                    }
                }

            }
        }
    }
    void DrawEdge()
    {
        isEdgeReady = true;
        GameObject tile1 = gameManager.GetTile2("Tile_1");
        GameObject tile2 = gameManager.GetTile2("Tile_2");
        GameObject tile3 = gameManager.GetTile2("Tile_3");

        GameObject tile6 = gameManager.GetTile2("Tile_6");
       
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

        for (int j = 0; j <levelHeight; j++)
        { 
            for (int i = levelWidth-1; i >= 0; i--)
            {

 
                //down
                if (edgeArray[i,j]==1 && cellState[i, j].state == 0)
                {    
                    Vector2 position = new Vector2(-100, -100);
                    //如果右边还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j] == 1 && cellState[i, j].state != 1)
                    {

                        //如果左边是自己且都还没决定，就跳过这一此判定
                        if (edgeArray[i - 1, j] == 1 && cellState[i - 1, j].state == 0) {
                            continue;

                        }
                        if (edgeArray[i - 1, j] == 2)//左边是右下角
                        {
                            if (cellState[i - 1, j].state == 12)//左边是单边右下角
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                            }

                            else if (cellState[i - 1, j].state == 2)//左边是双边右下角
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                                position.y = position.y + 24;
                            }
                        }
                        else if (edgeArray[i - 1, j] == 1)//左边是自己
                        {
                            if (cellState[i - 1, j].state == 1)//左边是自己
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 3)//左上角是左下角
                        {
                            if (cellState[i - 1, j + 1].state == 11)//左上角是单边左下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 24;
                                position.y = position.y - 24;
                            }

                            else if (cellState[i - 1, j + 1].state == 3)//左上角是双边左下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;
                                position.y = position.y - 24;
                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 2)//左上角是单边右下角
                        {
                            if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 24;
                                position.y = position.y - 24;
                            }
                            else if (cellState[i - 1, j + 1].state == 2)//左上角是双边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;

                            }
                        }

                        else if (edgeArray[i - 1, j + 1] == 4)//左上角是右边
                        {
                            if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                            {

                                position = cellState[i - 1, j + 1].position;

                                position.y = position.y - 66;
                            }
                        }

                        else if (edgeArray[i, j + 1] == 4)//上角是右边
                        {
                            if (cellState[i, j + 1].state == 14)//上角是右边
                            {
                                position = cellState[i, j + 1].position;
                                position.y = position.y - 66;
                            }
                        }

                        else if (edgeArray[i, j + 1] == 7)//上角是上边
                        {
                            if (cellState[i, j + 1].state == 16)//上角是上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 48;
                            }
                        }

                        else if (edgeArray[i, j + 1] == 8)//上角是左上边
                        {
                            if (cellState[i, j + 1].state == 8)//上角是双左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 47;
                            }
                            else if (cellState[i, j + 1].state == 15)//上角是单左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 47;
                            }
                        }


                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile1, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 1;
                            cellState[i, j].position = new Vector2(position.x, position.y);
                            cellState[i + 1, j].state = 1;
                            cellState[i + 1, j].position = new Vector2(position.x, position.y);
                            // Instantiate(tile99, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                            // Instantiate(tile99, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                        //}
                    }
                    else if (cellState[i, j].state != 1) {

                        if (edgeArray[i - 1, j] == 2)//左边是右下角
                        {
                            if (cellState[i - 1, j].state == 12)//左边是单边右下角
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                            }
                            else if (cellState[i - 1, j].state == 2)//左边是双边右下角
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                                position.y = position.y + 24;
                            }
                        }
                        else if (edgeArray[i - 1, j] == 1)//左边是自己
                        {
                            if (cellState[i - 1, j].state == 1)//左边是自己
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;

                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 3)//左上角是左下角
                        {
                            if (cellState[i - 1, j + 1].state == 11)//左上角是单边左下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 24;
                                position.y = position.y - 24;
                            }

                            else if (cellState[i - 1, j + 1].state == 3)//左上角是双边左下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;
                                position.y = position.y - 24;
                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 2)//左上角是右下角
                        {
                            if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 24;
                                position.y = position.y - 24;
                            }

                            else if (cellState[i - 1, j + 1].state == 2)//左上角是双边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;
                            }
                        }

                        else if (edgeArray[i - 1, j + 1] == 4)//左上角是右边
                        {
                            if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                            {
                                position = cellState[i - 1, j + 1].position;

                                position.y = position.y - 66;
                            }
                        }

                        else if (edgeArray[i, j + 1] == 4)//上角是右边
                        {
                            if (cellState[i, j + 1].state == 14)//上角是右边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 66;
                            }
                        }
                        else if (edgeArray[i, j + 1] == 8)//上角是左上边
                        {
                            if (cellState[i, j + 1].state == 8)//上角是双左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 47;
                            }
                            else if (cellState[i, j + 1].state == 15)//上角是单左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 47;
                            }
                        }
                        else if (edgeArray[i, j + 1]== 7)//上角是上边
                        {
                            if (cellState[i, j + 1].state == 16)//上角是上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 48;
                            }
                        }

                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile9, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 9;
                            cellState[i, j].position = position;
                           // Instantiate(tile99, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                          // Instantiate(tile99, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }


                }

                //left-bot corner
                else if (edgeArray[i, j] == 3 && cellState[i, j].state == 0)
                {
    
                    Vector2 position = new Vector2(-100, -100);
                    //如果右下角还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j - 1] == 3 && cellState[i, j].state != 3) {
                        if (edgeArray[i - 1, j] == 1)//左边是下底边
                        {
                            if (cellState[i - 1, j].state == 9)//左边是单下底边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                                position.y = position.y - 24;
                            }
                            else if (cellState[i - 1, j].state == 1)//左边是双下底边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                                position.y = position.y - 24;
                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 3)//左上角是自己类型
                        {
                            if (cellState[i - 1, j + 1].state == 3)//左上角是自己类型
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;
                                position.y = position.y - 48;
                            }
                        }
                        else if (edgeArray[i, j + 1] == 7)//上角是上边
                        {
                            if (cellState[i, j + 1].state == 16)//上角是上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 70;
                            }
                        }
                        else if (edgeArray[i, j + 1] == 4)//上角是右边
                        {
                            if (cellState[i, j + 1].state == 14)//上角是右边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 84;
                            }
                        }
                        else if (edgeArray[i - 1, j] == 2)//左边是右下边
                        {
                            if (cellState[i - 1, j].state == 12)//左边是单右下边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                                position.y = position.y - 24;

                            }
                            else if (cellState[i - 1, j].state == 2)//左边是双右下边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;

                            }
                        }
                        else if (edgeArray[i, j + 1] == 8)//上角是左上边
                        {
                            if (cellState[i, j + 1].state == 8)//上角是双边左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 66;
                            }
                            else if (cellState[i, j + 1].state == 15)//上角是单边左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 66;
                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 2)//左上角是右下角
                        {
                            if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 24;
                                position.y = position.y - 14;

                            }
                            else if (cellState[i - 1, j + 1].state == 2)//左上角是双边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;

                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 4)//左上角是右边
                        {
                            if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                            {
                                if (edgeArray[i - 1, j] == 1)//如果左边是单下边，但是还没有被画出来
                                {
                                    continue;
                                }
                                else
                                {
                                    position = cellState[i - 1, j + 1].position;

                                    position.y = position.y - 83;
                                }
                            }
                        }
                
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile3, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 3;
                            cellState[i, j].position = position;
                            cellState[i + 1, j - 1].state = 3;
                            cellState[i + 1, j - 1].position = position;
                          // Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                           // Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }
                    else if (cellState[i, j].state != 3)
                    {
                        if (edgeArray[i - 1, j] == 1)//左边是下底边
                        {
                            if (cellState[i - 1, j].state == 9)//左边是单下底边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;

                            }
                            else if (cellState[i - 1, j].state == 1)//左边是双下底边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;

                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 3)//左上角是自己类型
                        {
                            if (cellState[i - 1, j + 1].state == 3)//左上角是自己类型
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;
                                position.y = position.y - 24;
                            }
                        }
                        else if (edgeArray[i, j + 1] == 7)//上角是上边
                        {
                            if (cellState[i, j + 1].state == 16)//上角是上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 56;
                            }
                        }
                        else if (edgeArray[i, j + 1] == 4)//上角是右边
                        {
                            if (cellState[i, j + 1].state == 14)//上角是右边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 72;
                            }
                        }
                        else if (edgeArray[i - 1, j] == 2)//左边是右下边
                        {
                            if (cellState[i - 1, j].state == 12)//左边是单右下边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;

                            }
                            else if (cellState[i - 1, j].state == 2)//左边是双右下边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                                position.y = position.y + 10;
                            }
                        }
                        else if (edgeArray[i, j + 1] == 8)//上角是左上边
                        {
                            if (cellState[i, j + 1].state == 8)//上角是双边左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 52;
                            }
                            else if (cellState[i, j + 1].state == 15)//上角是单边左上边
                            {
                                position = cellState[i, j + 1].position;

                                position.y = position.y - 52;
                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 2)//左上角是单边右下角
                        {
                            if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 24;
                                position.y = position.y - 12;

                            }
                            else if (cellState[i - 1, j + 1].state == 2)//左上角是双边右下角
                            {
                                position = cellState[i - 1, j + 1].position;
                                position.x = position.x + 48;

                            }
                        }
                        else if (edgeArray[i - 1, j + 1] == 4)//左上角是右边
                        {
                            if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                            {
                                if (edgeArray[i - 1, j] == 1)//如果左边是单下边，但是还没有画出来
                                {
                                    continue;
                                }
                                else
                                {
                                    position = cellState[i - 1, j + 1].position;

                                    position.y = position.y - 72;
                                }
                            }
                        }
                        
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile11, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 11;
                            cellState[i, j].position = position;
                          // Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                          //  Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
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
                        if (edgeArray[i - 1, j - 1] == 1)//左下角是底边
                        {
                            if (cellState[i - 1, j - 1].state == 1)//左下角是双底边
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 48;
                                position.y = position.y + 24;
                            }
                            else if (cellState[i - 1, j - 1].state == 9)//左下角是单底边
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 24;
                                position.y = position.y + 24;
                            }
                        }

                        else if (edgeArray[i - 1, j - 1] == 2)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i - 1, j - 1] == 5)//左下角是左边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 24;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile2, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 2;
                            cellState[i, j].position = position;
                            cellState[i + 1, j + 1].state = 2;
                            cellState[i + 1, j + 1].position = position;
                          // Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                           // Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }
                    
                    else if (cellState[i, j].state != 2) {


                        //如果左下角已经有合成的斜边了
                        if (edgeArray[i - 1, j - 1] == 2)
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        //如果左下角是右边
                        else if (edgeArray[i - 1, j - 1] == 4)
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 40;
                        }
                        //如果左下角是左边
                        else if (edgeArray[i - 1, j - 1] == 5)
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 24;
                        }
                        
                        else if (edgeArray[i - 1, j - 1]== 1)//左下角是底边
                        {
                            if (cellState[i - 1, j - 1].state == 1)//左下角是底边
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 48;
                                position.y = position.y + 24;
                            }
                            else if (cellState[i - 1, j - 1].state == 9) //左下角是单底边
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 24;
                                position.y = position.y + 24;
                            }
                        }
                        
                        else if (edgeArray[i + 1, j] == 1)//右边是单双底边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                            position.y = position.y - 10;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile12, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 12;
                            cellState[i, j].position = position;
                          // Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                           // Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }


                    }
        
                }
                //right
                else if (edgeArray[i, j] == 4 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);
                    if (edgeArray[i - 1, j + 1] == 2)//左上边是右下角
                    {
                        if (cellState[i - 1, j + 1].state == 12)//左上边是单边右下角
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 45;
                        }
                        else if (cellState[i - 1, j + 1].state == 2)//左上边是双边右下角
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 63;
                        }
                    }
                    else if (edgeArray[i, j + 1] == 4)//上边是自己类型的//放在了左下边之前
                    {
                        position = cellState[i, j + 1].position;
                        position.y = position.y - 24;
                    }
                   

                    else if (edgeArray[i, j + 1] == 7)//上边是单上边
                    {
                        position = cellState[i, j + 1].position;
                        position.y = position.y - 9;
                    }
                    else if (edgeArray[i - 1, j + 1] == 3)//左上边是左下角//放在了判断是否是自己之前
                    {
                        if (cellState[i - 1, j + 1].state == 11)//左上边是单边左下角
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 24;

                        }
                        else if (cellState[i - 1, j + 1].state == 3)//左上边是双边左下角
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 48;
                            position.y = position.y - 24;

                        }
                    }
                    else if (edgeArray[i, j + 1] == 8)//上边是左上角//放在了下边是左下边之前
                    {
                        if (cellState[i, j + 1].state == 8)//上边是双边左上角
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 5;
                        }
                        else if (cellState[i, j + 1].state == 15)//上边是单边左上角
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 5;
                        }
                    }

                    else if (edgeArray[i, j - 1] == 3)//下边是左下边//放在了左上边是左下角之后
                    {
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
                    }

                    else if (edgeArray[i - 1, j + 1] == 4)//左上边是自己类型的
                    {
                        position = cellState[i - 1, j + 1].position;
                        position.y = position.y - 24;
                    }
                   
                    else if (edgeArray[i, j - 1] == 4)//下边是自己类型的
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 24;

                    }
                    else if (edgeArray[i - 1, j - 1] == 8)//左下边是左上角
                    {
                        if (cellState[i - 1, j - 1].state == 8)//左下边是双边左上角
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 65;
                        }
                        else if (cellState[i - 1, j - 1].state == 15)//左下边是单边左上角
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 41;
                        }
                    }
                    else if (edgeArray[i + 1, j + 1]== 8)//右上边是左上角
                    {
                        if (cellState[i + 1, j + 1].state == 15)//右上边是单边左上角
                        {
                            position = cellState[i + 1, j + 1].position;

                            position.y = position.y - 5;
                        }
                        else if (cellState[i + 1, j + 1].state == 8)//右上边是双边左上角
                        {
                            if (edgeArray[i, j + 1] == 7)//如果上边是上边，但是还没有画出来
                            {
                                continue;
                            }
                            else
                            {
                                position = cellState[i + 1, j + 1].position;
                                position.y = position.y - 5;
                            }
                        }
                    }
                    
                    else if (edgeArray[i + 1, j + 1]== 7)//右上边是上边
                    {
                        position = cellState[i + 1, j + 1].position;

                        position.y = position.y - 9;
                    }
                    else if (edgeArray[i + 1, j + 1]== 4)//右上边是自己
                    {
                        position = cellState[i + 1, j + 1].position;

                        position.y = position.y - 24;
                    }
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile14, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 14;
                        cellState[i, j].position = position;
                       // Instantiate(tile105, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else {
                        isEdgeReady = false;
                      // Instantiate(tile105, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                        

                }

                //left
                else if (edgeArray[i, j] == 5 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);
                    if (edgeArray[i, j - 1] == 1)//下边是下边
                    {
                        if (cellState[i, j - 1].state == 9)//下边是单下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 68;
                        }
                        else if (cellState[i, j - 1].state == 1)//下边是双下边
                        {
                            position = cellState[i, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 69;

                        }

                    }
                    else if (edgeArray[i, j - 1] == 2)//下边是右下边
                    {
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
                    }

                    else if (edgeArray[i, j - 1] == 5)//下边是自己类型的
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 24;

                    }
                    else if (edgeArray[i + 1, j - 1] == 6)//右下边是右上角
                    {
                        if (cellState[i + 1, j - 1].state == 6)//右下边是双边右上角
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 76;
                        }
                        else if (cellState[i + 1, j - 1].state == 17)//右下边是单边右上角
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 52;
                        }
                    }
                    else if (edgeArray[i + 1, j - 1] == 8)//右下边是左上角
                    {
                        if (cellState[i + 1, j - 1].state == 8)//右下边是双边左上角
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 17;
                        }
                        else if (cellState[i + 1, j - 1].state == 15)//右下边是单边左上角
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 17;
                        }
                    }

                    else if (edgeArray[i - 1, j - 1] == 5)//左下边是左边
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.y = position.y + 24;

                    }
                    else if (edgeArray[i - 1, j - 1] == 2)//左下边是单右下边
                    {
                        if (cellState[i - 1, j - 1].state == 12)//左下边是单右下边
                        {
                            if (edgeArray[i, j - 1] == 1)//如果自己下边是下边，但是还没有被画出来
                            {
                                continue;
                            }
                            else
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 10;
                                position.y = position.y + 72;
                            }

                        }
                        else if (cellState[i - 1, j - 1].state == 2)//左下边是双右下边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 34;
                            position.y = position.y + 83;

                        }
                    }
                    else if (edgeArray[i - 1, j - 1]== 1)//左下边是下边
                    {
                        if (cellState[i - 1, j - 1].state == 9)//左下边是单下边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 10;
                            position.y = position.y + 68;

                        }
                        else if (cellState[i - 1, j - 1].state == 1)//左下边是双下边
                        {
                            if (edgeArray[i, j - 1] == 1)//如果自己下边是下边，但是还没有被画出来
                            {
                                continue;
                            }
                            else
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 34;
                                position.y = position.y + 24;
                            }
                        }
                    }
                    
                    else if (edgeArray[i + 1, j - 1]== 5)//右下边是左边
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.y = position.y + 24;

                    }
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 13;
                        cellState[i, j].position = position;
                      // Instantiate(tile106, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
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
                    //如果左下角还是自己，则使用两个格子的tile
                    if (edgeArray[i - 1, j - 1] == 8 && cellState[i, j].state != 8)
                    {
                        if (edgeArray[i + 1, j + 1] == 7)//右上角是上边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 48;
                            position.y = position.y - 55;
                        }
                        else if (edgeArray[i + 1, j + 1] == 8)//右上角是左上边
                        {
                            if (cellState[i + 1, j + 1].state == 8)//右上角是双左上边
                            {
                                position = cellState[i + 1, j + 1].position;
                                position.x = position.x - 48;
                                position.y = position.y - 48;
                            }
                        }

                        else if (edgeArray[i + 1, j + 1] == 4)//右上角是右边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 48;
                            position.y = position.y - 76;
                        }
                        /*
                        else if (cellState[i, j - 1].state == 3)//下角是双边左下边
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
                        */
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i, j - 1] == 5)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }

                        else if (edgeArray[i - 1, j] == 7)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (edgeArray[i - 1, j] == 6)//左边是右上边
                        {
                            if (cellState[i - 1, j].state == 17)//左边是单右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                            }
                            else if (cellState[i - 1, j].state == 6)//左边是双右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                            }
                        }
                        
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile8, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 8;
                            cellState[i, j].position = position;
                            cellState[i - 1, j - 1].state = 8;
                            cellState[i - 1, j - 1].position = position;
                        }
                        else {
                            isEdgeReady = false;
                         Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                            
                    }

                    //其他情况贴单个斜边
                    else if (cellState[i, j].state != 8)
                    {
                        if (edgeArray[i + 1, j + 1] == 7)//右上角是上边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 30;
                        }
                        else if (edgeArray[i + 1, j + 1] == 4)//右上角是右边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 52;
                        }
                        else if (edgeArray[i + 1, j + 1] == 8)//右上角是左上边
                        {
                            if (cellState[i + 1, j + 1].state == 8)//右上角是双左上边
                            {
                                position = cellState[i + 1, j + 1].position;
                                position.x = position.x - 24;
                                position.y = position.y - 24;
                            }
                        }

                        /*
                        else if (cellState[i, j - 1].state == 3)//下角是双边左下边
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
                        */
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i, j - 1] == 5)//下角是左边
                        {
                            if (edgeArray[i + 1, j + 1] == 7)//如果右上角是上边，但是还没有被画出来
                            {
                                continue;
                            }
                            else
                            {
                                position = cellState[i, j - 1].position;
                                position.y = position.y + 7;
                            }


                        }
                        else if (edgeArray[i - 1, j - 1] == 7)//左下角是单上边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;

                        }

                        /*
                        else if (cellState[i - 1, j].state == 16)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        */
                        else if (edgeArray[i - 1, j]== 6)//左边是右上边
                        {
                            if (cellState[i - 1, j].state == 17)//左边是单右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                            }
                            else if (cellState[i - 1, j].state == 6)//左边是双右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                            }
                        }
                        
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile15, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 15;
                            cellState[i, j].position = position;
                          // Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);

                        }
                        else {
                            isEdgeReady = false;
                       // Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }                            
                    }
                }

                //right-top corner
                else if (edgeArray[i, j] == 6 && cellState[i, j].state == 0)
                {           
                    Vector2 position = new Vector2(-100, -100);
                    //如果左上角还是自己，则使用两个格子的tile
                    if (edgeArray[i - 1, j + 1] == 6 && cellState[i, j].state != 6)
                    {

                        if (edgeArray[i + 1, j] == 7)//右边是单上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                        }
                        else if (edgeArray[i + 1, j] == 8)//右边是左上边
                        {
                            if (cellState[i + 1, j].state == 15)//右边是单左上边
                            {
                                position = cellState[i + 1, j].position;
                                position.x = position.x - 48;
                            }
                            else if (cellState[i + 1, j].state == 8)//右边是双左上边
                            {
                                position = cellState[i + 1, j].position;
                                position.x = position.x - 48;
                            }
                        }

                        else if (edgeArray[i, j - 1] == 5)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 5;
                        }
                        else if (edgeArray[i + 1, j - 1] == 5)//右下角是左边
                        {
                            if (cellState[i, j - 2].state == 13)//如果下两格也是左边。
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 24;
                                position.y = position.y + 5;
                            }
                            else
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 35;
                            }

                        }
                        else if (edgeArray[i, j - 1] == 2)//下角是右下边
                        {
                            if (cellState[i, j - 1].state == 2)//下角是双边右下边
                            {
                                position = cellState[i, j - 1].position;

                                position.y = position.y + 64;
                            }
                            else if (cellState[i, j - 1].state == 12)//下角是单边右下边
                            {
                                position = cellState[i, j - 1].position;
                                position.x = position.x - 24;
                                position.y = position.y + 52;
                            }
                        }
                        else if (edgeArray[i, j - 1] == 1)//下角是下边
                        {
                            if (cellState[i, j - 1].state == 1)//下角是双下边
                            {
                                position = cellState[i, j - 1].position;
                                position.y = position.y + 48;
                            }
                            else if (cellState[i, j - 1].state == 9)//下角是单下边
                            {
                                position = cellState[i, j - 1].position;
                                position.x = position.x - 24;
                                position.y = position.y + 48;
                            }
                        }

                        /*
                        else if (cellState[i, j - 1].state == 3)//下角是双边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 67;
                        }
                        else if (cellState[i, j - 1].state == 11)//下角是单边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 53;
                        }
                       
                        
                        */
                        else if (edgeArray[i + 1, j - 1] == 6)//右下角是自己类型
                        {
                            if (cellState[i + 1, j - 1].state == 6)//右下角是自己类型
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 48;
                                position.y = position.y + 48;
                            }
                        }
                        else if (edgeArray[i + 1, j - 1] == 8)//右下边是左上边
                        {
                            if (cellState[i + 1, j - 1].state == 15)//右下边是单左上边
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 48;
                            }
                            else if (cellState[i + 1, j - 1].state == 8)//右下边是双左上边
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 48;
                            }
                        }
                        else if (edgeArray[i - 1, j - 1] == 7)//左下角是单上边
                        {
                            if (cellState[i - 1, j - 1].state == 16)//左下角是单上边
                            {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 24;
                            }
                        }


                        else if (edgeArray[i - 1, j] == 7)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (edgeArray[i - 1, j] == 6)//左边是右上边
                        {
                            if (cellState[i - 1, j].state == 17)//左边是单右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                            }
                            else if (cellState[i - 1, j].state == 6)//左边是双右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                            }
                        }
                        
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile6, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 6;
                            cellState[i, j].position = position;
                            cellState[i - 1, j + 1].state = 6;
                            cellState[i - 1, j + 1].position = position;
                        //  Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                          // Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }

                    //其他情况贴单个斜边
                    else if (cellState[i, j].state != 6)
                    {
                        if (edgeArray[i + 1, j] == 8)//右边是左上边
                        {
                            if (cellState[i + 1, j].state == 15)//右边是单左上边
                            {
                                position = cellState[i + 1, j].position;
                                position.x = position.x - 24;

                            }
                            else if (cellState[i + 1, j].state == 8)//右边是双左上边
                            {
                                position = cellState[i + 1, j].position;
                                position.x = position.x - 24;
                            }
                        }

                        else if (edgeArray[i + 1, j] == 7)//右边是单上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                        }
                        else if (edgeArray[i + 1, j - 1] == 8)//右下边是左上边
                        {
                            if (cellState[i + 1, j - 1].state == 15)//右下边是单左上边
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 24;

                            }
                            else if (cellState[i + 1, j - 1].state == 8)//右下边是双左上边
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 24;

                            }
                        }

                        /*
                        else if (cellState[i - 1, j+1].state == 16)//左上边是单上边
                        {
                            position = cellState[i - 1, j+1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 30;
                        }
                        */
                        else if (edgeArray[i + 1, j - 1] == 2)//右下边是右下边
                        {
                            if (cellState[i + 1, j - 1].state == 12)//右下边是单右下边
                            {
                                if (edgeArray[i + 1, j] == 8)//如果右边是单左上边，但是还没画
                                {
                                    continue;
                                }
                                else
                                {
                                    position = cellState[i + 1, j - 1].position;
                                    position.x = position.x + 24;
                                    position.y = position.y - 30;
                                }
                            }
                        }
                        else if (edgeArray[i + 1, j - 1] == 6)//右下边是自己
                        {
                            if (cellState[i + 1, j - 1].state == 6)//右下边是自己
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 24;
                                position.y = position.y + 48;
                            }
                        }


                        else if (edgeArray[i + 1, j - 1] == 5)//右下边是左边
                        {
                            if (cellState[i, j - 2].state == 13)//如果下两格也是左边
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.y = position.y + 5;
                            }
                            else
                            {
                                position = cellState[i + 1, j - 1].position;
                                position.x = position.x - 9;
                            }
                        }
                        else if (edgeArray[i, j - 1] == 2)//下角是右下边
                        {
                            if (cellState[i, j - 1].state == 2)//下角是双边右下边
                            {
                                position = cellState[i, j - 1].position;
                                position.x = position.x + 24;
                                position.y = position.y + 64;
                            }
                            else if (cellState[i, j - 1].state == 12)//下角是单边右下边
                            {
                                position = cellState[i, j - 1].position;
                                position.y = position.y + 52;
                            }
                        }
                        else if (edgeArray[i, j - 1] == 1)//下角是下边
                        {
                            if (cellState[i, j - 1].state == 9)//下角是单下边
                            {
                                position = cellState[i, j - 1].position;
                                position.y = position.y + 48;
                            }
                            else if (cellState[i, j - 1].state == 1)//下角是双下边
                            {
                                position = cellState[i, j - 1].position;
                                position.x = position.x + 24;
                                position.y = position.y + 48;
                            }
                        }

                        /*
                        else if (cellState[i, j - 1].state == 3)//下角是双边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 67;
                        }
                        else if (cellState[i, j - 1].state == 11)//下角是单边左下边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 53;
                        }
                       
                        
                       
                        */
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是双左上边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (edgeArray[i, j - 1] == 5)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 5;
                        }
                        else if (edgeArray[i, j - 1] == 4)//下角是右边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }
                        else if (edgeArray[i - 1, j - 1] == 7)//左下角是单上边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                        }

                        else if (edgeArray[i - 1, j] == 7)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        else if (edgeArray[i - 1, j] == 6)//左边是右上边
                        {
                            if (cellState[i - 1, j].state == 17)//左边是单右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 24;
                            }
                            else if (cellState[i - 1, j].state == 6)//左边是双右上边
                            {
                                position = cellState[i - 1, j].position;
                                position.x = position.x + 48;
                            }
                        }
                        
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile17, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 17;
                            cellState[i, j].position = position;
                         // Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                        else
                        {
                            isEdgeReady = false;
                        //  Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                    }
                }

                //top
                else if (edgeArray[i, j] == 7 && (cellState[i, j].state == 0|| cellState[i, j].state == -1))

                {
            
                    Vector2 position = new Vector2(-100, -100);
                    if (cellState[i, j].state == -1)//最下面的
                    {
                        position = new Vector2(i * tileSize, j * tileSize);

                    }
                    else if (edgeArray[i, j - 1] == 5)//下边是左边
                    {
                        position = cellState[i, j - 1].position;
                        position.y = position.y + 7;
                    }
                    else if (edgeArray[i + 1, j] == 7)//右边是自己//先判定右边，再判定右下边
                    {
                        position = cellState[i + 1, j].position;
                        position.x = position.x - 24;
                    }
                    else if (edgeArray[i + 1, j - 1] == 5)//右下边是左边
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.y = position.y + 7;
                    }
                    else if (edgeArray[i + 1, j] == 8)//右边是左上边
                    {
                        if (cellState[i + 1, j].state == 15)//右边是单左上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                        }
                        else if (cellState[i + 1, j].state == 8)//右边是双左上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;

                        }
                    }
                    else if (edgeArray[i + 1, j - 1] == 8)//右下边是左上边
                    {
                        if (cellState[i + 1, j - 1].state == 15)//右下边是单左上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                        }
                        else if (cellState[i + 1, j - 1].state == 8)//右下边是双左上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            //position.y = position.y + 58;
                        }
                    }
                    else if (edgeArray[i + 1, j - 1] == 6)//右下边是右上边
                    {
                        if (cellState[i + 1, j - 1].state == 17)//右下边是单右上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 30;
                        }
                        else if (cellState[i + 1, j - 1].state == 6)//右下边是双右上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 51;
                        }
                    }
                    else if (edgeArray[i, j - 1] == 2)//下边是右下边
                    {
                        if (cellState[i, j - 1].state == 12)//下边是单右下边
                        {
                            position = cellState[i, j - 1].position;

                            position.y = position.y + 55;
                        }
                        else if (cellState[i, j - 1].state == 2)//下边是双右下边
                        {
                            position = cellState[i, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 66;
                        }
                    }
                    
                    else if (edgeArray[i, j - 1] == 1)//下边是下底边
                    {
                        if (edgeArray[i + 1, j] == 8)//如果右边是单上左边，但是还没有被画出来
                        {
                            continue;
                        }
                        else
                        {
                            if (cellState[i, j - 1].state == 1)
                            {//双下底边
                                position = cellState[i, j - 1].position;
                                position.x = position.x + 24;
                                position.y = position.y + 51;
                            }
                            else if (cellState[i, j - 1].state == 9)//单下底边
                            {
                                position = cellState[i, j - 1].position;
                                position.y = position.y + 51;
                            }

                        }


                    }
                    
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile16, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 16;
                        cellState[i, j].position = position;
                     // Instantiate(tile102, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                    }
                    else {
                        isEdgeReady = false;
                     // Instantiate(tile102, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
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
                if (cellState[i, j].state == 200|| cellState[i, j].state == 201)
                {
                    cellState[i, j].state = 0;
                }

            }
        }
        //CA(0.5f, 1, 8, 1, true, 0);
        CA(0.5f, 1, 4, 1, false, 201);
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
                if (cellState[i, j].state == 201)
                {
                    //Debug.Log("a");
                    float tempValue = Random.value;
                    if (tempValue < 0.25)
                    {
                        Instantiate(tile30, new Vector3(cellState[i,j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        //cellState[i, j].state = 30;//number in tileset folder
                    }
                    else if (tempValue < 0.5)
                    {
                        Instantiate(tile31, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        //cellState[i, j].state = 31;//number in tileset folder
                    }
                    else if (tempValue < 0.75)
                    {
                        Instantiate(tile32, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        //cellState[i, j].state = 32;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile33, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        //cellState[i, j].state = 33;//number in tileset folder
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
                if (cellState[i, j].state == 200 || cellState[i, j].state == 201)
                {
                    cellState[i, j].state = 0;
                }

            }
        }


        // CA(0.5f, 1, 8, 1, true, 0);
        CA(0.6f, 1, 8, 1, false, 200);
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
                if (cellState[i, j].state == 201)
                {

                    float tempValue = Random.value;
                    if (tempValue < 0.20)
                    {
                        Instantiate(tile34, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        cellState[i, j].state = 34;//number in tileset folder
                    }
                    else if (tempValue < 0.4)
                    {
                        Instantiate(tile35, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        cellState[i, j].state = 35;//number in tileset folder
                    }
                    else if (tempValue < 0.6)
                    {
                        Instantiate(tile36, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        cellState[i, j].state = 36;//number in tileset folder
                    }
                    else if (tempValue < 0.8)
                    {
                        Instantiate(tile37, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                        cellState[i, j].state = 37;//number in tileset folder
                    }
                    else if (tempValue < 1)
                    {
                        Instantiate(tile38, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
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

    void DrawLoot()
    {
        
           GameObject tile41 = gameManager.GetTile2("Tile_41");
       

        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = levelHeight-1; j > -1; j--)
            {
                if (cellState[i, j].state == 201)
                {
                    lootCount++;
                    Instantiate(tile41, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                    cellState[i, j].state = 41;//number in tileset folder           
                }
            }
        }
    }

    void AddEntrance() {
        GameObject tile44 = gameManager.GetTile2("Tile_44");
        GameObject tile45 = gameManager.GetTile2("Tile_45");
        bool hasDoor = false;
        bool isClose = false;
        for (int j = levelHeight - 1; j >= 0; j--)
        {
            for (int i = 0; i < levelWidth; i++)
            {
                if (cellState[i, j].state == 1 && !hasDoor)
                {
                    isClose = false;
                    for (int m = -3; m < 4; m++)
                    {
                        for (int n = -3; n < 0; n++)
                        {
                            if (cellState[i + m, j + n].state > 0 && cellState[i + m, j + n].state < 100)
                                isClose = true;
                        }
                    }
                    if (!isClose)
                    {
                        float tempValue = Random.value;
                        if (tempValue < 0.5)
                        {
                            Instantiate(tile44, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                            cellState[i, j].state = 44;
                        }
                        else if (tempValue < 1)
                        {
                            Instantiate(tile45, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                            cellState[i, j].state = 45;
                        }
                        entrancePosition = new Vector2(i, j);
                        hasDoor = true;
                    }
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
        /*
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
        */
        for (int i = 5; i < levelWidth-5; i++)
        {
            for (int j = 5; j< levelHeight-5; j++)
            {
                if (cellState[i, j].state == 200)
                {
                    float ratio = (float)Random.Range(1, 50);
                    bool isClose = false;
                    if (ratio < 5)
                    {
                        
                        for (int m = -2; m < 3; m++) {
                            for (int n = -2; n < 3; n++) {
                                if (cellState[i+m, j+n].state != 200)
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
                enemyCount++;
                if (cellState[i, j].state == 100)
                {
                    Instantiate(enemy1, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);          
                }
                else if (cellState[i, j].state == 101)
                {
                    Instantiate(enemy2, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 102)
                {
                    Instantiate(enemy3, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 103)
                {
                    Instantiate(enemy4, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 104)
                {
                    Instantiate(enemy5, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 105)
                {
                    Instantiate(enemy6, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                }
                else if (cellState[i, j].state == 106)
                {
                    Instantiate(enemy7, new Vector3(cellState[i, j].position.x, cellState[i, j].position.y, 0), transform.rotation);
                }
            }
        }
    }

    void DrawPortal()
    {
        portal = gameManager.GetPortal("Portal_3");
        Vector2 maxPosition = new Vector2(0, 0);
        float maxDistance = 0f;
        bool isClose = false;
        for (int i =2; i < levelWidth-2; i++)
        {
            for (int j = 2; j < levelHeight-2; j++)
            {
                if (cellState[i, j].state >=100 )
                {
                    isClose = false;
                    for (int m = -3; m < 4; m++)
                    {
                        for (int n = -3; n < 4; n++)
                        {
                            if (cellState[i + m, j + n].state > 0 && cellState[i + m, j + n].state < 100)
                                isClose = true;
                        }
                    }
                    float tempDistance = Mathf.Abs(entrancePosition.x - i) + Mathf.Abs(entrancePosition.y - j);
                    if (tempDistance > maxDistance && !isClose) {
                        maxDistance = tempDistance;
                        maxPosition = cellState[i, j].position;
                    }
            
                }
            }
        }

        portal=Instantiate(portal, new Vector3(maxPosition.x, maxPosition.y, 0), transform.rotation);

    }

    void DrawPlayer()
    {
        GameObject player1 = gameManager.Player;
        Vector2 position = cellState[(int)entrancePosition.x, (int)entrancePosition.y - 1].position;
        player1 = Instantiate(player1, new Vector3(position.x, position.y, 0), transform.rotation);
        cellState[(int)entrancePosition.x, (int)entrancePosition.y - 1].state=300;
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
        for (int i = 2; i < levelWidth-2; i++)
        {
            for (int j = 2; j < levelHeight-2; j++)
            {
                if (cellState[i, j].state >= 100)
                {
                    isClose = false;
                    for (int m = -2; m < 3; m++)
                    {
                        for (int n = -2; n < 3; n++)
                        {
                            if (cellState[i + m, j + n].state > 0 && cellState[i + m, j + n].state < 100)
                                isClose = true;
                        }
                    }
                    if (Random.Range(1, 100) < 2 && !isClose && !isCreated)
                    {
                        Instantiate(textManager.GetComponent<NPCManager>().NPCPortal, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                        textManager.GetComponent<NPCManager>().PortalPosition = new Vector3(i * tileSize, j * tileSize, 0);
                        Instantiate(NPCObject, new Vector3(310, -610, 0), transform.rotation);
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
