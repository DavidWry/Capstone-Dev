using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;


public class ProcedualGeneration2_2 : MonoBehaviour {

    private GameManager gameManager;
    public int levelWidth=160;
    public int levelHeight=160;

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
    private int MIN_TILES=750;
 
    private int tilesPlaced;
	private int tilesToProcess;
	private int adjacentCells;
	private int startX;
	private int startY;
	private float turnRatio=0.25f;
    private int[] direction;

    // Use this for initialization
    void Start () {
        isEdgeReady = false;    
        NextScene.nowName = "2_1";
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
        //AddWalls();          
        ChangeEdge();
        FindTheDown();
        for(int i=0;i<180;i++)
        {
           DrawEdge();
        }
        Connect();
        ChangeColor();
        
        /*
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
        */
    }

    void Terrain()
    {
        GenerateCave();
    }
    void Smooth()
    {
        for (int i = 1; i < levelWidth-1; i++) {
            for (int j = 1; j < levelHeight-1; j++){
                
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
                /*
                if (landArray[i - 1, j + 1] == 1 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    landArray[i, j] = 1;//111001011
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 1 && landArray[i + 1, j + 1] == 1 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 0 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    landArray[i, j] = 1;//011001011
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 1 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 1 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 1)
                    landArray[i, j] = 0;//001011001
                if (landArray[i - 1, j + 1] == 1 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 0 &&
                   landArray[i - 1, j] == 1 && landArray[i, j] == 1 && landArray[i + 1, j] == 0 &&
                   landArray[i - 1, j - 1] == 1 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                    landArray[i, j] = 0;//101110100
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 0 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 1 && landArray[i + 1, j] == 0 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    landArray[i, j] = 0;//000010011
                if (landArray[i - 1, j + 1] == 1 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 0 &&
                   landArray[i - 1, j] == 1 && landArray[i, j] == 1 && landArray[i + 1, j] == 0 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 0 && landArray[i + 1, j - 1] == 0)
                    landArray[i, j] = 0;//100110000
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 0 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 1 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    landArray[i, j] = 0;//000011011
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 1 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 1 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    {
                        landArray[i-1, j] = 0;//001011011
                        landArray[i - 1, j-1] = 0;
                    }
                if (landArray[i - 1, j + 1] == 0 && landArray[i, j + 1] == 0 && landArray[i + 1, j + 1] == 0 &&
                   landArray[i - 1, j] == 0 && landArray[i, j] == 1 && landArray[i + 1, j] == 1 &&
                   landArray[i - 1, j - 1] == 0 && landArray[i, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    landArray[i, j] = 0;//000011011
                    */
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
        GameObject tile10 = gameManager.GetTile2("Tile_10");
       
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
                /*
                if (landArray[i, j] == 0)
                {
                    Instantiate(tile10, new Vector3(i * tileSize, j * tileSize, 0), transform.rotation);
                }
                */
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
                if (landArray[i, j] == 0)
                {
                    if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                   && landArray[i, j + 1] == 0 && landArray[i, j - 1] == 1 && 
                   landArray[i - 1, j - 1] == 1 && landArray[i + 1, j - 1] == 1)
                    {
                        edgeArray[i, j] = 1;//下底边
                        //Debug.Log("辖地变");
                    }

                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j - 1] == 1 && landArray[i, j + 1] == 0 
                    && landArray[i + 1, j + 1] == 0 && landArray[i - 1, j - 1] == 1)
                    {
                        edgeArray[i, j] = 3;//左下角
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j - 1] == 1 && landArray[i, j + 1] == 0
                    && 
                    (
                    (landArray[i + 1, j - 1] == 1 && landArray[i - 1, j + 1] == 0) 
                    ||
                    (landArray[i - 1, j - 1] == 0 && landArray[i + 1, j - 1] == 0 && landArray[i + 1, j] == 0)
                    )
                    )
                    {
                        edgeArray[i, j] = 2;//右下角
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                   && landArray[i, j - 1] == 0 && landArray[i, j + 1] == 1 && 
                   landArray[i - 1, j + 1] == 1 && landArray[i + 1, j + 1] == 1)
                    {
                        edgeArray[i, j] = 7;//上底边
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j + 1] == 1 && landArray[i, j - 1] == 0 
                    && 
                    (
                    (landArray[i + 1, j - 1] == 0 && landArray[i - 1, j + 1] == 1)
                    ||
                    (landArray[i + 1, j + 1] == 0 && landArray[i - 1, j + 1] == 0 && landArray[i - 1, j] == 0)
                    )
                    )
                    {
                        edgeArray[i, j] = 8;//左上角
                    }

                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i, j + 1] == 1 && landArray[i, j - 1] == 0 
                    && landArray[i - 1, j - 1] == 0 && landArray[i + 1, j + 1] == 1)
                    {
                        edgeArray[i, j] = 6;//右上角
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i - 1, j] == 1 && landArray[i + 1, j] == 0)
                    {
                        edgeArray[i, j] = 4;//右边
                    }
                    else if (i < levelWidth - 1 && i > 0
                   && j < levelHeight - 1 && j > 0
                    && landArray[i + 1, j] == 1 && landArray[i - 1, j] == 0)
                    {
                        edgeArray[i, j] = 5;//左边
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
        Vector2 firstTopPosition = new Vector2(-100, -100);
        Vector2 lastPosition = new Vector2(-100, -100);
        for (int i = 0; i < levelWidth; i++) {
            for (int j = 0; j < levelHeight; j++) {
                if (cellState[i, j].state == 16 && cellState[i, j].isFirstTop) {
                    firstTopPosition = cellState[i, j].position;
                    if (cellState[i + 1, j - 1].state != 0) {
                        lastPosition = cellState[i + 1, j - 1].position;
                    }
                    else if (cellState[i + 1, j + 1].state != 0)
                    {
                        lastPosition = cellState[i + 1, j + 1].position;
                    }
                    else if (cellState[i, j - 1].state != 0)
                    {
                        lastPosition = cellState[i, j - 1].position;
                    }
                    else if (cellState[i + 1, j].state != 0)
                    {
                        lastPosition = cellState[i + 1, j].position;
                    }
                }
            }
        }

        float yDistance = lastPosition.y - firstTopPosition.y;
        int rightTileNum = 0;
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
        if (rightTileNum > 0)
        {
            for (int i = 0; i < rightTileNum; i++)
            {
                Vector2 position = firstTopPosition;
                position.x = position.x + 14;
                position.y = position.y + 24 * (i+1);
                Instantiate(tile13, new Vector3(position.x, position.y, 0), transform.rotation);
            }
        }
        else if (rightTileNum < 0)
        {
            for (int i = 0; i > rightTileNum; i--)
            {
                Vector2 position = firstTopPosition;
                position.x = position.x + 14;
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
                        /*
                        //open the gate!
                        if (Random.value < 0.1 && j != 0)
                        {
                            Instantiate(tile19, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                            cellState[i, j].state = 19;
                            cellState[i, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);
                            cellState[i + 1, j].state = 19;
                            cellState[i + 1, j].position = new Vector2(i * (float)tileSize, j * (float)tileSize);

                        }
                        */

                        //else
                        //{
                        //如果左边是自己且都还没决定，就跳过这一此判定
                        if (edgeArray[i - 1, j] == 1 && cellState[i-1, j].state == 0) {
                            continue;

                        }
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
                        else if (cellState[i - 1, j].state == 1)//左边是自己
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 48;
                           
                        }
                        else if (cellState[i - 1, j+1].state == 11)//左上角是单边左下角
                        {
                            position = cellState[i - 1, j+1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 24;
                        }

                        else if (cellState[i - 1, j+1].state == 3)//左上角是双边左下角
                        {
                            position = cellState[i - 1, j+1].position;
                            position.x = position.x + 48;
                            position.y = position.y - 24;
                        }
                        else if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
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
                        else if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                        {
                            position = cellState[i - 1, j + 1].position;
                            
                            position.y = position.y - 66;
                        }
                        
                        else if (cellState[i, j + 1].state == 14)//上角是右边
                        {
                            position = cellState[i, j + 1].position;
                           
                            position.y = position.y - 66;
                        }
                        else if (cellState[i, j + 1].state == 16)//上角是上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 48;
                        }
                        else if (cellState[i, j + 1].state == 8)//上角是双左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 47;
                        }
                        else if (cellState[i, j + 1].state == 15)//上角是单左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 47;
                        }
                       
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile1, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 1;
                            cellState[i, j].position = new Vector2(position.x, position.y);
                            cellState[i + 1, j].state = 1;
                            cellState[i + 1, j].position = new Vector2(position.x, position.y);
                        }
                        else
                        {
                            isEdgeReady = false;
                            // Instantiate(tile102, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                            
                        //}
                    }
                    else if (cellState[i, j].state != 1) {
                        
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
                        else if (cellState[i - 1, j].state == 1)//左边是自己
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 48;

                        }

                        else if (cellState[i - 1, j + 1].state == 11)//左上角是单边左下角
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
                        else if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
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
                        else if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                        {
                            position = cellState[i - 1, j + 1].position;

                            position.y = position.y - 66;
                        }

                        
                        
                        else if (cellState[i, j + 1].state == 14)//上角是右边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 66;
                        }
                        else if (cellState[i, j + 1].state == 8)//上角是双左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 47;
                        }
                        else if (cellState[i, j + 1].state == 15)//上角是单左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 47;
                        }
                        else if (cellState[i, j + 1].state == 16)//上角是上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 48;
                        }
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile9, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 9;
                            cellState[i, j].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }


                }

                //left-bot corner
                else if (edgeArray[i, j] == 3 && cellState[i, j].state == 0)
                {
    
                    Vector2 position = new Vector2(-100, -100);
                    //如果右下角还是自己，则使用两个格子的tile
                    if (edgeArray[i + 1, j - 1] == 3 && cellState[i, j].state != 3) {
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

                        else if (cellState[i - 1, j + 1].state == 3)//左上角是自己类型
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 48;
                            position.y = position.y - 48;
                        }
                        else if (cellState[i, j + 1].state == 16)//上角是上边
                        {
                            position = cellState[i, j + 1].position;
                            
                            position.y = position.y - 70;
                        }
                        else if (cellState[i, j + 1].state == 14)//上角是右边
                        {
                            position = cellState[i, j + 1].position;
                          
                            position.y = position.y - 84;
                        }
                        else if (cellState[i - 1, j].state == 12)//左边是单右下边
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
                        else if (cellState[i, j + 1].state == 8)//上角是双边左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 66;
                        }
                        else if (cellState[i, j + 1].state == 15)//上角是单边左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 66;
                        }
                        else if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
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
                        else if (cellState[i - 1, j + 1].state == 14)//左上角是右边
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
                        if (position.x > 0 && position.y > 0)
                        {
                            Instantiate(tile3, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 3;
                            cellState[i, j].position = position;
                            cellState[i + 1, j - 1].state = 3;
                            cellState[i + 1, j - 1].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }
                    else if (cellState[i, j].state != 3)
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
                        else if (cellState[i - 1, j + 1].state == 3)//左上角是自己类型
                        {
                            position = cellState[i - 1, j + 1].position;
                            position.x = position.x + 48;
                            position.y = position.y - 24;
                        }
                        else if (cellState[i, j + 1].state == 16)//上角是上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 56;
                        }
                        else if (cellState[i, j + 1].state == 14)//上角是右边
                        {
                            position = cellState[i, j + 1].position;
                            
                            position.y = position.y - 72;
                        }
                        else if (cellState[i - 1, j].state == 12)//左边是单右下边
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
                        else if (cellState[i, j + 1].state == 8)//上角是双边左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 52;
                        }
                        else if (cellState[i, j + 1].state == 15)//上角是单边左上边
                        {
                            position = cellState[i, j + 1].position;

                            position.y = position.y - 52;
                        }
                        else if (cellState[i - 1, j + 1].state == 12)//左上角是单边右下角
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
                        else if (cellState[i - 1, j + 1].state == 14)//左上角是右边
                        {
                            if (edgeArray[i-1, j] == 1)//如果左边是单下边，但是还没有画出来
                            {
                                continue;
                            }
                            else {
                                position = cellState[i - 1, j + 1].position;

                                position.y = position.y - 72;
                            }
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
                            //Instantiate(tile103, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
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
                            position.y = position.y + 24;
                        }
                        else if (cellState[i - 1, j - 1].state == 9)//左下角是单底边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y + 24;
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
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }
                    
                    else if (cellState[i, j].state != 2) {

                        
                        //如果左下角已经有合成的斜边了
                        if (cellState[i - 1, j - 1].state == 2)
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
                        //左下角是双底边
                        else if (cellState[i - 1, j - 1].state == 1)
                        {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 48;
                                position.y = position.y + 24;
                        }

                        //左下角是单底边
                        else if (cellState[i - 1, j - 1].state == 9)
                        {
                                position = cellState[i - 1, j - 1].position;
                                position.x = position.x + 24;
                                position.y = position.y + 24;
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
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile104, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }


                    }
        
                }
                //right
                else if (edgeArray[i, j] == 4 && cellState[i, j].state == 0)
                {
                    Vector2 position = new Vector2(-100, -100);
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
                    else if (cellState[i, j - 1].state == 11)//下边是单左下边
                    {
                        position = cellState[i, j - 1].position;
                        
                        position.y = position.y + 72;
                        
                    }
                    else if (cellState[i, j - 1].state == 3)//下边是双左下边
                    {
                        position = cellState[i, j - 1].position;  
                        position.y = position.y + 86;
                    }
                    else if (cellState[i, j + 1].state == 16)//上边是单上边
                    {
                        position = cellState[i, j + 1].position;
                        position.y = position.y - 9;
                    }
                    else if (cellState[i - 1, j + 1].state == 11)//左上边是单边左下角//放在了判断是否是自己之前
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
                    else if (cellState[i, j + 1].state == 14)//上边是自己类型的
                    {
                        position = cellState[i, j + 1].position;
                        position.y = position.y - 24;
                    }
                    else if (cellState[i-1, j + 1].state == 14)//左上边是自己类型的
                    {
                        position = cellState[i-1, j + 1].position;
                        position.y = position.y - 24;
                    }
                    else if (cellState[i, j + 1].state == 8)//上边是双边左上角
                    {
                        position = cellState[i, j + 1].position;
                        
                        position.y = position.y -5;
                    }
                    else if (cellState[i, j + 1].state == 15)//上边是单边左上角
                    {
                        position = cellState[i, j + 1].position;
                      
                        position.y = position.y - 5;
                    }
                    else if (cellState[i, j-1].state == 14)//下边是自己类型的
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
                    else if (cellState[i + 1, j + 1].state == 15)//右上边是单边左上角
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
                    else if (cellState[i + 1, j + 1].state == 16)//右上边是上边
                    {
                        position = cellState[i + 1, j + 1].position;

                        position.y = position.y - 9;
                    }
                    else if (cellState[i + 1, j + 1].state == 14)//右上边是自己
                    {
                        position = cellState[i + 1, j + 1].position;

                        position.y = position.y - 24;
                    }
                    if (position.x > 0 && position.y > 0)
                    {
                        Instantiate(tile14, new Vector3(position.x, position.y, 0), transform.rotation);
                        cellState[i, j].state = 14;
                        cellState[i, j].position = position;
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
                    else if (cellState[i, j - 1].state == 12)//下边是单右下边
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
                        position.y = position.y + 76;
                        
                    }
                    else if (cellState[i + 1, j - 1].state == 17)//右下边是单边右上角
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x - 24;
                        position.y = position.y + 52;
                        
                    }
                    else if (cellState[i + 1, j - 1].state == 8)//右下边是双边左上角
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
                    else if (cellState[i - 1, j - 1].state == 13)//左下边是左边
                    {
                        position = cellState[i - 1, j - 1].position;
                        position.y = position.y + 24;

                    }
                    else if (cellState[i - 1, j - 1].state == 12)//左下边是单右下边
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
                    else if (cellState[i - 1, j - 1].state == 9)//左下边是单下边
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
                    else if (cellState[i + 1, j - 1].state == 13)//右下边是左边
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.y = position.y + 24;

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
                    //如果左下角还是自己，则使用两个格子的tile
                    if (edgeArray[i - 1, j - 1] == 8 && cellState[i, j].state != 8)
                    {
                        if (cellState[i + 1, j + 1].state == 16)//右上角是上边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 48;
                            position.y = position.y - 55;
                        }
                        else if (cellState[i + 1, j + 1].state == 8)//右上角是双左上边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 48;
                            position.y = position.y - 48;
                        }
                        else if (cellState[i + 1, j + 1].state == 14)//右上角是右边
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
                        else if (cellState[i, j - 1].state == 14)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
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
                            cellState[i - 1, j - 1].state = 8;
                            cellState[i - 1, j - 1].position = position;
                        }
                        else {
                            isEdgeReady = false;
                            //Instantiate(tile100, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                            
                    }

                    //其他情况贴单个斜边
                    else if (cellState[i, j].state != 8)
                    {
                        if (cellState[i + 1, j + 1].state == 16)//右上角是上边
                        {
                            position = cellState[i+1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 30;
                        }
                        else if (cellState[i + 1, j + 1].state == 14)//右上角是右边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 52;
                        }
                        else if (cellState[i + 1, j + 1].state == 8)//右上角是双左上边
                        {
                            position = cellState[i + 1, j + 1].position;
                            position.x = position.x - 24;
                            position.y = position.y - 24;
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
                        /*
                        else if (cellState[i - 1, j].state == 16)//左边是单上边
                        {
                            position = cellState[i - 1, j].position;
                            position.x = position.x + 24;
                        }
                        */
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
 
                        if (cellState[i + 1, j].state == 16)//右边是单上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                        }
                        else if (cellState[i + 1, j].state == 15)//右边是单左上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                        }
                        else if (cellState[i + 1, j].state == 8)//右边是双左上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 48;
                        }
                        else if (cellState[i, j - 1].state == 13)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 5;
                        }
                        else if (cellState[i+1, j - 1].state == 13)//右下角是左边
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

                        else if (cellState[i, j - 1].state == 2)//下角是双边右下边
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
                        else if (cellState[i, j - 1].state == 1)//下角是双下边
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
                        else if (cellState[i + 1, j - 1].state == 6)//右下角是自己类型
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 48;
                            position.y = position.y + 48;
                        }
                        else if (cellState[i + 1, j - 1].state == 15)//右下边是单左上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 48;

                        }
                        else if (cellState[i + 1, j - 1].state == 8)//右下边是双左上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 48;

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
                            Instantiate(tile6, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 6;
                            cellState[i, j].position = position;
                            cellState[i - 1, j + 1].state = 6;
                            cellState[i - 1, j + 1].position = position;
                        }
                        else
                        {
                            isEdgeReady = false;
                            //Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }

                    }

                    //其他情况贴单个斜边
                    else if (cellState[i, j].state != 6)
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
                        else if (edgeArray[i + 1, j] == 7)//右边是单上边
                        {
                            position = cellState[i + 1, j].position;
                            position.x = position.x - 24;
                        }
                        else if (cellState[i + 1, j - 1].state == 15)//右下边是单左上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;

                        }
                        else if (cellState[i + 1, j - 1].state == 8)//右下边是双左上边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;

                        }
                        /*
                        else if (cellState[i - 1, j+1].state == 16)//左上边是单上边
                        {
                            position = cellState[i - 1, j+1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 30;
                        }
                        */
                        else if (cellState[i + 1, j - 1].state == 12)//右下边是单右下边
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x + 24;
                            position.y = position.y - 30;
                        }
                        else if (cellState[i + 1, j - 1].state == 6)//右下边是自己
                        {
                            position = cellState[i + 1, j - 1].position;
                            position.x = position.x - 24;
                            position.y = position.y + 48;
                        }
                        
                        else if (cellState[i + 1, j - 1].state == 13)//右下边是左边
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
                        else if (cellState[i, j - 1].state == 2)//下角是双边右下边
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
                        else if (cellState[i, j - 1].state == 9)//下角是单下边
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
                        else if (edgeArray[i - 1, j - 1] == 8)//左下角是自己类型
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 48;
                            position.y = position.y + 48;
                        }
                        else if (cellState[i, j - 1].state == 14)//下角是左边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 5;
                        }
                        else if (cellState[i, j - 1].state == 13)//下角是右边
                        {
                            position = cellState[i, j - 1].position;
                            position.y = position.y + 7;
                        }
                        else if (cellState[i - 1, j - 1].state == 16)//左下角是单上边
                        {
                            position = cellState[i - 1, j - 1].position;
                            position.x = position.x + 24;
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
                            Instantiate(tile17, new Vector3(position.x, position.y, 0), transform.rotation);
                            cellState[i, j].state = 17;
                            cellState[i, j].position = position;

                        }
                        else
                        {
                            isEdgeReady = false;
                           // Instantiate(tile101, new Vector3(i * (float)tileSize, j * (float)tileSize, 0), transform.rotation);
                        }
                    }
                }

                //top
                else if (edgeArray[i, j] == 7 && (cellState[i, j].state == 0|| cellState[i, j].state == -1))

                {
            
                    Vector2 position = new Vector2(-100, -100);
                    if (cellState[i,j].state == -1)//最下面的
                    {
                        position = new Vector2(i * tileSize, j * tileSize);

                    }
                    else if (edgeArray[i, j-1] == 5)//下边是左边
                    {
                        position = cellState[i, j-1].position;
                        position.y = position.y +7;
                    }
                    else if (edgeArray[i + 1, j] == 7)//右边是自己//先判定右边，再判定右下边
                    {
                        position = cellState[i + 1, j].position;
                        position.x = position.x - 24;
                    }
                    else if (edgeArray[i+1, j - 1] == 5)//右下边是左边
                    {
                        position = cellState[i+1, j - 1].position;
                        position.y = position.y + 7;
                    }
                   
                    else if (cellState[i + 1, j].state == 15)//右边是单左上边
                    {
                        position = cellState[i + 1, j].position;
                        position.x = position.x - 24;
                    }
                    else if (cellState[i + 1, j].state == 8)//右边是双左上边
                    {
                        position = cellState[i + 1, j].position;
                        position.x = position.x - 24;
                       
                    }
                    else if (cellState[i + 1, j-1].state == 15)//右下边是单左上边
                    {
                        position = cellState[i + 1, j-1].position;
                        position.x = position.x - 24;
                    }
                    else if (cellState[i + 1, j - 1].state == 8)//右下边是双左上边
                    {
                        position = cellState[i + 1, j - 1].position;
                        position.x = position.x - 24;
                        //position.y = position.y + 58;
                    }
                    else if (cellState[i + 1, j - 1].state == 17)//右下边是单右上边
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
                    else if (cellState[i, j - 1].state == 12)//下边是单右下边
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
                    else if (edgeArray[i, j - 1] == 1)//下边是下底边
                    {
                        if (cellState[i, j-1].state == 1) {//双下底边
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
        for (int i = 2; i < levelWidth-2; i++)
        {
            for (int j = 2; j< levelHeight-2; j++)
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
