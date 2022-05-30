using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{   
    public float heal=15;
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public AIDetector Script;
    public Transform Target;
    public Animator animator;
    void Start()
    {
        gameObject.name="Slime";

    }

    void Update()
    {
        Target=Script.Target;
        
    }
    void FixedUpdate()
    {
        
        if(Script.TargetVisible)
        {
            if(transform.position.x > Target.position.x)
            {
                transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
                Debug.Log("el target esta a la izquierda.");

            }
            else if(transform.position.x < Target.position.x)
            {
                transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                Debug.Log("el target esta a la derecha.");
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

        if( heal == 0 ){
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   GameObject collisionGameObject = collision.gameObject;
        if( collisionGameObject.tag == "bullet" )
        {
            takeHit(5f);     
        }
        
    }
}
