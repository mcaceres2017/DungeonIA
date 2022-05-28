using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
}
