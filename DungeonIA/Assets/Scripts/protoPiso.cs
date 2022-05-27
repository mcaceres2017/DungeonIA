using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class protoPiso : MonoBehaviour
{
    public Tile groundTile;

    public Vector3Int position;

    public Tilemap suelo;
    
    [ContextMenu("Paintar")]
    void paintSuelo()
    {
    	suelo.SetTile(position, groundTile);
    }



}
