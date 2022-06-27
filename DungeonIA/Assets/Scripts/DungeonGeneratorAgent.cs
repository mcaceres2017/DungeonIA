using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneratorAgent : MonoBehaviour
{	public GameObject slimePrefab;
	public GameObject superSlimePrefab;
	public GameObject potionPrefab;
	public GameObject chestPrefab;
	public GameObject torchPrefab;
	public GameObject box1Prefab;
	public GameObject box2Prefab;
	public GameObject box3Prefab;
	public GameObject box4Prefab;
	public GameObject XboxPrefab;
	public GameObject YboxPrefab;
	public GameObject player;
	public int enemyMax=3;
    private Vector2Int startPosition = Vector2Int.zero;
	//private Vector2Int startPosition = player.transform.position;
    [SerializeField]
    private int minRoomRadius = 3;
    [SerializeField]
    private int maxRoomRadius = 15;
    [SerializeField]
    private int maxRoomAttempts = 3;
    [SerializeField]
    private int minCorridorLength = 3;
    [SerializeField]
    private int maxCorridorLength = 7;
    [SerializeField]
    private int agentHP = 7;
    [SerializeField]
    private int maxAgents = 3;
    [SerializeField]
    private int offset = 1;
    [SerializeField]
    [Range(0,100)]
    private int cloneProbability = 30;
    [SerializeField]
    private bool shouldDebugDrawRooms = false;
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private Tilemap wallTilemap;
    [SerializeField]
    private Tile floorTile;
    [SerializeField]
    private Tile wallTile;
	private string[] Rooms=new string[4]{"Enemigos","Pociones","Jefe","Tesoro"};

    private LookAheadAgent agentP;

    private List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) //LEFT
    };
    

    HashSet<Vector2Int> floorPositions;
    HashSet<Vector2Int> wallPositions;
    // list of rooms, to check whether a new potencial room will intersect
    // with the ones already created.
    List<RectInt> roomsList;


    // Master Method, is in charge of calling the rest.
    public void GenerateDungeon()
    {
		
		player.transform.position=Vector3Int.zero;
    	floorPositions = new HashSet<Vector2Int>();
    	wallPositions = new HashSet<Vector2Int>();
    	roomsList = new List<RectInt>();
    	ClearTileMaps();
		ClearGameObjects();
		


    	//seed
   		//in the future, it may be possible give a seed specified by the user.
    	Random.InitState((int)System.DateTime.Now.Ticks);
    	
    	StartWalk(floorPositions, roomsList);
    	AddWalls(floorPositions, wallPositions);
    	
    	PlaceTiles(floorPositions, floorTilemap, floorTile);
    	PlaceTiles(wallPositions, wallTilemap, wallTile);
    }
	//clear all created gameobjects in the dungeon.
	private void ClearGameObjects(){
		GameObject[] slimes;
		GameObject[] potions;
		GameObject[] treasures;
 
		slimes = GameObject.FindGameObjectsWithTag("Slime");
		potions = GameObject.FindGameObjectsWithTag("healPotion");
		treasures = GameObject.FindGameObjectsWithTag("treasure");

		foreach(GameObject slime in slimes)
		{	
			DestroyImmediate(slime,false);
		}
		foreach(GameObject potion in potions)
		{	
			DestroyImmediate(potion,false);
		}
		foreach(GameObject treasure in treasures)
		{	
			DestroyImmediate(treasure,false);
		}
	}

    //clean all tiles of the map.
    private void ClearTileMaps()
    {
    	floorTilemap.ClearAllTiles();
    	wallTilemap.ClearAllTiles();
    }

    //Generates the corridors and rooms of the dungeon.
    private void StartWalk(HashSet<Vector2Int> floorPositions, List<RectInt> roomsList)
    {

    	int nombreClon = 1;
    	//Initialize the agent and put it in a queue.
    	Vector2Int initialPreviousDirection = GetRandomCardinalDirection();
    	agentP = new LookAheadAgent(agentHP, startPosition, initialPreviousDirection, "agente P");

    	CreateRoom(floorPositions, roomsList, agentP,0);
    	// Put the agent in the queue.
    	Queue<LookAheadAgent> agentsQueue = new Queue<LookAheadAgent>();
    	agentsQueue.Enqueue(agentP);


    	while(agentsQueue.Count > 0)
        {
        	var agent = agentsQueue.Dequeue();
        	//Debug.Log("tomado el agente:" + agent.name + " de la queue");

        	CreateCorridor(floorPositions, agent);
			
			/**
			* 0:Inicio
			* 1:Normal
			* 2:Enemigos
			* 3:Tesoro
			* 4:Jefe
			* 5:--
			* 6:--
			*/
        	CreateRoom(floorPositions, roomsList, agent,UnityEngine.Random.Range(1,5));

        	//probability of creating another agent that will be a copy of the 
        	//selected one.
        	if(UnityEngine.Random.Range(1, 100) < cloneProbability && agentsQueue.Count < maxAgents && agent.hp >= minCorridorLength)
        	{
        		agentsQueue.Enqueue(new LookAheadAgent(agent.hp, agent.lastPosition, agent.previousDirection, "" + nombreClon));
        		//Debug.Log("Clon creado");
        		nombreClon++;
        	}

        	// if the agent has hp to make another corridor, we add it again to the queue.
        	if(agent.hp >= minCorridorLength) 
        	{
        		agentsQueue.Enqueue(agent);
        		//Debug.Log("agente " + agent.name + " volvio a la queue");
        	}else 
        	{
        		//Debug.Log("el agente: " + agent.name + " murio");
        	}
        }
    }
	private void Tesoros(){

	}

    public Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }


    private void CreateCorridor(HashSet<Vector2Int> floorPositions, LookAheadAgent agent)
    {
    	// get the agent position, a random direction, and the length of the corridor.
        var currentPosition = agent.lastPosition;
        Vector2Int direction;
        int corridorLength;
       
        // the new direction can't be "moving back".
        while(true)
        {
        	direction = GetRandomCardinalDirection();
        	if(direction != agent.previousDirection) break;
        }

        //the corridorLength has to be within the agent's remaining hp.
        while(true)
        {
        	corridorLength = UnityEngine.Random.Range(minCorridorLength,maxCorridorLength);
        	if(corridorLength <= agent.hp) break;
        }

        //Debug.Log("El agente: "+ agent.name + "escogio la direccion: " + direction.x + "," + direction.y);
      	

        List<Vector2Int> corridor = new List<Vector2Int>();
        corridor.Add(currentPosition);


        // walk step by step making the corridor and adding it to the list.
        for (int i = 0; i < corridorLength; i++)
        {
			
			currentPosition += direction;
			
			
			if(true){//i!=1 && corridorLength-1!=i
				//Debug.Log("direction X:"+direction.x+" Y:"+direction.y);
				//up 0 1
				if(direction.x==0 && direction.y==1){
					//1 0
					//-1 0
					corridor.Add(currentPosition + new Vector2Int(1,0));
					corridor.Add(currentPosition + new Vector2Int(-1,0));
					if(corridorLength-1==i){
						corridor.Add(currentPosition + new Vector2Int(0,1));
						corridor.Add(currentPosition + new Vector2Int(-1,1));
						corridor.Add(currentPosition + new Vector2Int(1,1));	
					}	

				}
				//down 0 -1
				else if(direction.x==0 && direction.y==-1){
					//1 0
					//-1 0
					corridor.Add(currentPosition + new Vector2Int(1,0));
					corridor.Add(currentPosition + new Vector2Int(-1,0));
					if(corridorLength-1==i){
						corridor.Add(currentPosition + new Vector2Int(0,-1));
						corridor.Add(currentPosition + new Vector2Int(1,-1));
						corridor.Add(currentPosition + new Vector2Int(-1,-1));	
					}
				
				}
				//left -1 0
				else if(direction.x==-1 && direction.y==0){
					//0 1
					//0 -1
					corridor.Add(currentPosition + new Vector2Int(0,1));
					corridor.Add(currentPosition + new Vector2Int(0,-1));
					if(corridorLength-1==i){
						corridor.Add(currentPosition + new Vector2Int(1,1));
						corridor.Add(currentPosition + new Vector2Int(1,0));
						corridor.Add(currentPosition + new Vector2Int(1,-1));	
					}
					

				}
				//right 1 0
				else if(direction.x==1 && direction.y==0){
					//0 1
					//0 -1
					corridor.Add(currentPosition + new Vector2Int(0,1));
					corridor.Add(currentPosition + new Vector2Int(0,-1));
					
					if(corridorLength-1==i){
						corridor.Add(currentPosition + new Vector2Int(-1,1));
						corridor.Add(currentPosition + new Vector2Int(-1,0));
						corridor.Add(currentPosition + new Vector2Int(-1,-1));	
					}

				}
			}
			
            corridor.Add(currentPosition);

        }

        // updating the agent.
        agent.hp = agent.hp - corridorLength;
        agent.lastPosition = currentPosition;
        agent.previousDirection = (direction * -1);


        //then store the corridor positions in the hashset.
        floorPositions.UnionWith(corridor);
    }
	private void InstantiatePrefabRoomRandom(GameObject prefab,int min,int max,int p,int width,int height,int xi,int yi){
		int count=0;
		for(int x = xi - width +1 ; x <= xi + width -1 ; x++)
    	{
    		for(int y = yi - height +1 ; y <= yi + height -1 ; y++)
    		{
				if(UnityEngine.Random.Range(1, 100)< p && count<max && ((x!=xi && y!=yi) ||(x!=xi+1 && y!=yi) ||(x!=xi-1 && y!=yi) )  ){
					count++;
					if(p< 100)p+=10;
					Instantiate(prefab,new Vector3(x,y,0)+ new Vector3(0.5f,0.5f,0), Quaternion.identity);
        
				}
				
    			
    		}
    	}

	}
	private GameObject GetRandomBox(){
		int p=UnityEngine.Random.Range(1, 100);
		
		if(p<50) return box1Prefab;
		else return box3Prefab;//return box2Prefab;
		//if(p<75) return box3Prefab;
		//else return box4Prefab;
		
	}

	private void InstantiatePrefabRandom(GameObject prefab,int x,int y){
		
		Instantiate(prefab,new Vector3(x,y,0)  + new Vector3(0.5f,0.5f,0), Quaternion.identity);
        

	}	

    private void CreateRoom(HashSet<Vector2Int> floorPositions, List<RectInt> roomsList, LookAheadAgent agent,int type)
    {
    	var currentPosition = agent.lastPosition;
    	RectInt potentialRoom = new RectInt(0,0,0,0);

    	int attempts;
    	int width = 0;
    	int height = 0;
    	bool overlaps;

    	List<Vector2Int> roomPositions = new List<Vector2Int>();
 

    	// the potential room will have limited attemps,
    	// if all the attemps at creating the room overlaps
    	// other room, the method returns.
    	for(attempts = 0; attempts < maxRoomAttempts; attempts++)
    	{
    		overlaps = false;
    		width = UnityEngine.Random.Range(minRoomRadius,maxRoomRadius);
    		height = UnityEngine.Random.Range(minRoomRadius,maxRoomRadius);

    		potentialRoom = new RectInt(currentPosition.x - width - offset, currentPosition.y - height - offset, (width*2) + 1 + (offset*2), (height*2) + 1 + (offset*2));


    		foreach(var room in roomsList)
    		{
    			if(room.Overlaps(potentialRoom))
    			{
    				overlaps = true;
    				break;
    			} 
    		}

    		if(overlaps == false) break;
    	}

    	if (attempts == maxRoomAttempts) 
    	{
    		return;
    	}

    	// only if the potential room doesn't overlaps other rooms and the 
    	// limit of attempts was not excedeed, we add it to the hashset and the list.

    	//Because the width and height of the room are radius, we start from 
    	//the center minus the radius to the center plus the radius.
    	
		for(int x = currentPosition.x - width ; x <= currentPosition.x + width ; x++)
    	{
    		for(int y = currentPosition.y - height ; y <= currentPosition.y + height ; y++)
    		{
				
    			roomPositions.Add(new Vector2Int(x,y));
    		}
    	}
		//normal
		if(type==1)
		{
			//slimes
			InstantiatePrefabRoomRandom(slimePrefab,0,enemyMax,10,width,height,currentPosition.x,currentPosition.y);
			
			//heal potions
			InstantiatePrefabRoomRandom( potionPrefab,0,3,10,width,height,currentPosition.x,currentPosition.y);
		
			
		}
		//enemigos
		else if(type==2)
		{
			//slimes
			InstantiatePrefabRoomRandom(superSlimePrefab,2,enemyMax,10,width,height,currentPosition.x,currentPosition.y);
			InstantiatePrefabRoomRandom(slimePrefab,0,enemyMax,10,width,height,currentPosition.x,currentPosition.y);
			//heal potions
			InstantiatePrefabRoomRandom( potionPrefab,0,3,10,width,height,currentPosition.x,currentPosition.y);
		

			//boxes
			//InstantiatePrefabRoomRandom(box1Prefab,1,3,10,width,height,currentPosition.x,currentPosition.y);
			//InstantiatePrefabRoomRandom(box2Prefab,1,3,10,width,height,currentPosition.x,currentPosition.y);
			

			InstantiatePrefabRandom(torchPrefab,currentPosition.x-width,currentPosition.y-height);
			InstantiatePrefabRandom(torchPrefab,currentPosition.x-width,currentPosition.y+height);
			InstantiatePrefabRandom(torchPrefab,currentPosition.x+width,currentPosition.y+height);
			InstantiatePrefabRandom(torchPrefab,currentPosition.x+width,currentPosition.y-height);
		}	
		//tesoros
		else if(type==3)
		{
			//chest
			InstantiatePrefabRandom(torchPrefab,currentPosition.x+1,currentPosition.y);
			InstantiatePrefabRandom(chestPrefab,currentPosition.x,currentPosition.y);
			InstantiatePrefabRandom(torchPrefab,currentPosition.x-1,currentPosition.y);
			//slimes
			InstantiatePrefabRoomRandom(superSlimePrefab,1,3,10,width,height,currentPosition.x,currentPosition.y);
			
			//heal potions
			InstantiatePrefabRoomRandom( potionPrefab,0,3,10,width-1,height-1,currentPosition.x,currentPosition.y);
		
			//boxes
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-width,currentPosition.y-height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-width+1,currentPosition.y-height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-width,currentPosition.y-height+1);

			

			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-width,currentPosition.y+height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-width+1,currentPosition.y+height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-width,currentPosition.y+height-1);

			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+width,currentPosition.y+height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+width-1,currentPosition.y+height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+width,currentPosition.y+height-1);

			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+width,currentPosition.y-height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+width-1,currentPosition.y-height);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+width,currentPosition.y-height+1);

		
		}

		//puzzle
		else if(type==4)
		{
			//chest
			InstantiatePrefabRandom(XboxPrefab,currentPosition.x,currentPosition.y+1);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x,currentPosition.y+2);
			InstantiatePrefabRandom(YboxPrefab,currentPosition.x+1,currentPosition.y);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x+2,currentPosition.y);
			InstantiatePrefabRandom(chestPrefab,currentPosition.x,currentPosition.y);

			InstantiatePrefabRandom(YboxPrefab,currentPosition.x-1,currentPosition.y);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x-2,currentPosition.y);
			InstantiatePrefabRandom(XboxPrefab,currentPosition.x,currentPosition.y-1);
			InstantiatePrefabRandom(GetRandomBox(),currentPosition.x,currentPosition.y-2);
		
		}

		
    	floorPositions.UnionWith(roomPositions);
    	roomsList.Add(potentialRoom);
    }


    private void AddWalls(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> wallPositions)
    {
    	Vector2Int up;
    	Vector2Int down;
    	Vector2Int left;
    	Vector2Int right;
    	Vector2Int leftUp;
    	Vector2Int rightUp;
    	Vector2Int leftDown;
    	Vector2Int rightDown;
    	List<Vector2Int> neighbours;

    	foreach (var position in floorPositions)
    	{	

    		up = new Vector2Int(position.x, position.y + 1);
    		down = new Vector2Int(position.x, position.y - 1);
    		left = new Vector2Int(position.x - 1, position.y);
    		right = new Vector2Int(position.x + 1, position.y);
    		leftUp = new Vector2Int(position.x - 1, position.y + 1);
    		rightUp = new Vector2Int(position.x + 1, position.y + 1);
    		leftDown = new Vector2Int(position.x - 1, position.y -1);
    		rightDown = new Vector2Int(position.x + 1, position.y - 1);

    		neighbours = new List<Vector2Int>{
    			up,
    			down,
    			left,
    			right,
    			leftUp,
    			rightUp,
    			leftDown,
    			rightDown
    		};
    		
    		foreach(var neighbour in neighbours)
    		{
    			if(!floorPositions.Contains(neighbour))
    			{
    				wallPositions.Add(neighbour);
    			}
    		}
    	}
    }


    private void PlaceTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, Tile tile)
    {
    	foreach (var position in positions)
        {
            var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        	tilemap.SetTile(tilePosition, tile);
        }
    }


    void OnDrawGizmos()
    {
        AttemptDebugDrawBsp ();
    }

	private void OnDrawGizmosSelected () 
	{
    	AttemptDebugDrawBsp ();
	}

	void AttemptDebugDrawBsp () 
	{
    	if (shouldDebugDrawRooms) 
    	{
        	DebugDrawRooms (roomsList);
    	}
	}

	public void DebugDrawRooms (List<RectInt> roomsList) 
	{
    	// Container
    	Gizmos.color = Color.green;

    	foreach(var room in roomsList)
    	{
    		// bottom
    		Gizmos.DrawLine (new Vector3 (room.x, room.y, 0), new Vector3Int (room.xMax, room.y, 0));
    		// right
    		Gizmos.DrawLine (new Vector3 (room.xMax, room.y, 0), new Vector3Int (room.xMax, room.yMax, 0));
    		// top
    		Gizmos.DrawLine (new Vector3 (room.x, room.yMax, 0), new Vector3Int (room.xMax, room.yMax, 0));
    		// left
    		Gizmos.DrawLine (new Vector3 (room.x, room.y, 0), new Vector3Int (room.x, room.yMax, 0));
    	}
	}

	void Start()
    {
        //GenerateDungeon ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



// This will be our "miner", the one that place corridors at
// at random orientations and also place the rooms of our dungeon.
public class LookAheadAgent
{
	public int hp;
	public string name;
	public Vector2Int previousDirection;
	public Vector2Int lastPosition;

	public LookAheadAgent(int _hp, Vector2Int _lastPosition, Vector2Int _previousDirection, string _name)
	{
		hp = _hp;
		lastPosition = _lastPosition;
		previousDirection = _previousDirection;
		name = _name;
	}
}