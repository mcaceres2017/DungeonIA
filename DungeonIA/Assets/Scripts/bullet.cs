using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{   
    

    void Start()
    {   
        
        gameObject.name="Bullet";
        Destroy(gameObject,2f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   GameObject collisionGameObject = collision.gameObject;
        if( collisionGameObject.name != "Player" ){
            Destroy(gameObject);
        }
        
    }
}
