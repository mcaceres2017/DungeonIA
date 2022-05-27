using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class pieza : MonoBehaviour
{   public Tile wallTile;
    public Tile groundTile;
    public Vector3Int InitPos;
    
    //public Vector3Int position;

    public Tilemap suelo;
    private void paintSuelo(Vector3Int pos,Tile tile)
    {
    	suelo.SetTile(pos,tile);
    }
    // Start is called before the first frame update
    void Start()
    {   
        newRoom();
        

    }
    void newRoom()
    {
        int dado;
        int size=1;
        dado=Random.Range(0, 2);
        if(dado==0) size=3;
        else if(dado==1)size=5;
        else if(dado==2)size=7;    
        //3x3
        //Walls
        //0 abajo,1 arriba,2 izquierda, 3 derecha
        dado=Random.Range(0, 3);
        //abajo
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x+i,InitPos.y,InitPos.z),wallTile);
        }
        //arriba
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x+i,InitPos.y+size+1,InitPos.z),wallTile);
        }
        //izquierda
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x,InitPos.y+i,InitPos.z),wallTile);
            
        }
        //derecha
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x+size+1,InitPos.y+i,InitPos.z),wallTile);   
        }        


        //floor    
        for (int i = 1; i < size+1; i++){
            for (int j = 1; j < size+1; j++){
                paintSuelo(new Vector3Int(InitPos.x+i,InitPos.y+j,InitPos.z),groundTile);    
            }
        }


    }
    void corridor()
    {
        int dado;
        int size=1;
        dado=Random.Range(0, 2);
        if(dado==0) size=3;
        else if(dado==1)size=5;
        else if(dado==2)size=7;    
        //3x3
        //Walls
        //0 abajo,1 arriba,2 izquierda, 3 derecha
        dado=Random.Range(0, 3);
        //abajo
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x+i,InitPos.y,InitPos.z),wallTile);
        }
        //arriba
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x+i,InitPos.y+size+1,InitPos.z),wallTile);
        }
        //izquierda
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x,InitPos.y+i,InitPos.z),wallTile);
            
        }
        //derecha
        for (int i = 0; i < size+2; i++){
            paintSuelo(new Vector3Int(InitPos.x+size+1,InitPos.y+i,InitPos.z),wallTile);   
        }        


        //floor    
        for (int i = 1; i < size+1; i++){
            for (int j = 1; j < size+1; j++){
                paintSuelo(new Vector3Int(InitPos.x+i,InitPos.y+j,InitPos.z),groundTile);    
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
