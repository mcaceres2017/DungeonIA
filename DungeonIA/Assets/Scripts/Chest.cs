using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{   public float heal=10f;
    public GameObject explosionPrefab;
    public Animator animator;
    private bool alive=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeHit(float damage){
        
        heal -=  damage;
        
        if( heal <= 0 &&alive){
            alive=false;
            animator.SetBool("open",true);
            GameObject player=GameObject.FindWithTag("Player");
            player.gameObject.GetComponent<PlayerMovement>().GetMoney(10);
            Instantiate(explosionPrefab,gameObject.transform.position,gameObject.transform.rotation);
            
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.tag == "bullet" )
        {   
            takeHit(other.gameObject.GetComponent<bullet>().GetDamage());
                 
        }
        
    }
}
