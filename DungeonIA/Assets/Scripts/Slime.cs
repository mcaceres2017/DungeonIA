using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{   
    
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public AIDetector Script;
    public Transform Target;
    public Animator animator;
    void Start()
    {
        

    }

    void Update()
    {
        Target=Script.Target;
        

        

    }
    void FixedUpdate()
    {
        
        if(Script.TargetVisible)
        {
            transform.position = Vector2.MoveTowards(transform.position,Target.position,moveSpeed* Time.deltaTime);
            animator.SetFloat("Speed",moveSpeed);
        }
        else{
            animator.SetFloat("Speed",0);
        }

    }
}
