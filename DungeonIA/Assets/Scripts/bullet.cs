using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{   
    public GameObject explosionPrefab;
    private float damage;

    void Start()
    {   
        
        gameObject.name="Bullet";
        Destroy(gameObject,1.25f);
        
    }
    public void SetDamage(float _damage)
    {
        damage=_damage;
    }
    public float GetDamage()
    {
        return damage;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {   
        GameObject collisionGameObject = collision.gameObject;
        if( collisionGameObject.name != "Player" ){
            Destroy(gameObject);
        }
        
    }
    void OnDestroy()
    {
        Instantiate(explosionPrefab,gameObject.transform.position,gameObject.transform.rotation);
    }
}
