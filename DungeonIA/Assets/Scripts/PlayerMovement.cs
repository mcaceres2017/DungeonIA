using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   public int heal=100;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
  
    Vector2 moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {   
        
        gameObject.name="Player";
    }

    // Update is called once per frame
    void Update()
    {



        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");


        if(moveX<0)
        {
            transform.localScale = new Vector3(-1.0f,1.0f,1.0f);

        }else if(moveX>=0)
        {
            transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }

        moveDirection  = new Vector2(moveX,moveY).normalized;
        animator.SetFloat("Speed",moveDirection.magnitude) ;

        
        
    }

    void FixedUpdate(){

        rb.velocity = new Vector2(moveDirection.x * moveSpeed ,moveDirection.y * moveSpeed );

    }
    public void takeHit(float damage){
        
        heal -=  (int)damage;

        if( heal == 0 ){
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   GameObject collisionGameObject = collision.gameObject;
        if( collisionGameObject.tag == "Slime" )
        {
            takeHit(10f);     
        }
        
    }
}
