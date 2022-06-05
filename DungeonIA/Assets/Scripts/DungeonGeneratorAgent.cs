using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneratorAgent : MonoBehaviour
{
    private Vector2Int startPosition = Vector2Int.zero;
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
    	floorPositions = new HashSet<Vector2Int>();
    	wallPositions = new HashSet<Vector2Int>();
    	roomsList = new List<RectInt>();
    	ClearTileMaps();
    	//seed
   		//in the future, it may be possible give a seed specified by the user.
    	Random.InitState((int)System.DateTime.Now.Ticks);
    	
    	StartWalk(floorPositions, roomsList);
    	AddWalls(floorPositions, wallPositions);
    	
    	PlaceTiles(floorPositions, floorTilemap, floorTile);
    	PlaceTiles(wallPositions, wallTilemap, wallTile);
    }

    //It does what the name suggest.
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

    	CreateRoom(floorPositions, roomsList, agentP);
    	// Put the agent in the queue.
    	Queue<LookAheadAgent> agentsQueue = new Queue<LookAheadAgent>();
    	agentsQueue.Enqueue(agentP);


    	while(agentsQueue.Count > 0)
        {
        	var agent = agentsQueue.Dequeue();
        	Debug.Log("tomado el agente:" + agent.name + " de la queue");
        	CreateCorridor(floorPositions, agent);
        	CreateRoom(floorPositions, roomsList, agent);

        	//probability of creating another agent that will be a copy of the 
        	//selected one.
        	if(UnityEngine.Random.Range(1, 100) < cloneProbability && agentsQueue.Count < maxAgents && agent.hp >= minCorridorLength)
        	{
        		agentsQueue.Enqueue(new LookAheadAgent(agent.hp, agent.lastPosition, agent.previousDirection, "" + nombreClon));
        		Debug.Log("Clon creado");
        		nombreClon++;
        	}

        	// if the agent has hp to make another corridor, we add it again to the queue.
        	if(agent.hp >= minCorridorLength) 
        	{
        		agentsQueue.Enqueue(agent);
        		Debug.Log("agente " + agent.name + " volvio a la queue");
        	}else 
        	{
        		Debug.Log("el agente: " + agent.name + " murio");
        	}
        }
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

        Debug.Log("El agente: "+ agent.name + "escogio la direccion: " + direction.x + "," + direction.y);
      	

        List<Vector2Int> corridor = new List<Vector2Int>();
        corridor.Add(currentPosition);


        // walk step by step making the corridor and adding it to the list.
        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        // updating the agent.
        agent.hp = agent.hp - corridorLength;
        agent.lastPosition = currentPosition;
        agent.previousDirection = (direction * -1);


        //then store the corridor positions in the hashset.
        floorPositions.UnionWith(corridor);
    }


    private void CreateRoom(HashSet<Vector2Int> floorPositions, List<RectInt> roomsList, LookAheadAgent agent)
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
    	for(int x = currentPosition.x - width; x <= currentPosition.x + width; x++)
    	{
    		for(int y = currentPosition.y - height; y <= currentPosition.y + height; y++)
    		{
    			roomPositions.Add(new Vector2Int(x,y));
    		}
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
        GenerateDungeon ();
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