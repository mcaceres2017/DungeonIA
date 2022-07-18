using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{   public GameObject explosionPrefab;
    public float maxheal=40;
    public float heal=40;
    public float moveSpeed = 2f;
    public float attackCD= 1f;
    public float attackdamage=10f;
    public float hitdamage=10f;
    public float exp=1;
    public Rigidbody2D rb;
    public AIDetector Script;
    public Transform Target;
    public Animator animator;
    public AudioSource hit;
    public EnemyHealBar healbar;
    private float Timer;
    
    void Start()
    {
        gameObject.name="Slime";
        healbar.InitHealBar(maxheal);
    }

    void Update()
    {
        
        healbar.SetCurrentHeal(heal);
        Target=Script.Target;
    }
    void FixedUpdate()
    {
        if(Script.TargetVisible)
        {   
            if(transform.position.x > Target.position.x)
            {
                transform.localScale = new Vector3(-1.0f,1.0f,1.0f);

            }
            else if(transform.position.x < Target.position.x)
            {
                transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                
            }


            transform.position = Vector2.MoveTowards(transform.position,Target.position,moveSpeed* Time.deltaTime);
            animator.SetFloat("Speed",moveSpeed);
        
        }

        else{
            //Debug.Log("target no esta, slime quieto.");
            animator.SetFloat("Speed",0);
        
        }

    }
    public void takeHit(float damage){
        
        heal -=  damage;
        hit.Play();
        if( heal <= 0 ){
            //animator.SetBool("death",true);
            animator.SetTrigger("death");
            GameObject player=GameObject.FindWithTag("Player");
            player.gameObject.GetComponent<PlayerMovement>().GetExp(exp);
            player.gameObject.GetComponent<PlayerMovement>().GetMoney((int)exp);
            Instantiate(explosionPrefab,gameObject.transform.position,gameObject.transform.rotation);
            Destroy(gameObject,0.15f);


        }

    }
    public void attack(){
        //animator.SetBool("attack",true);
        //other.gameObject.GetComponent<PlayerMovement>().takeHit(attackdamage);
    
    }
    private void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.tag == "bullet" )
        {   
            takeHit(other.gameObject.GetComponent<bullet>().GetDamage());
                 
        }
        
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {   
        if( other.gameObject.tag == "Player" )
        {   
            if(attackCD<=Timer){
                //animator.SetBool("attack",true);
                animator.SetTrigger("attack");
                other.gameObject.GetComponent<PlayerMovement>().takeHit(attackdamage);
                Timer = 0f;
                
            }  
            else{
                //animator.SetBool("attack",false);
                Timer += Time.deltaTime;
            }
        }
        
    }
}
