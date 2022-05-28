using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public float moveSpeed = 4f;
    public Rigidbody2D rb;
    public Weapon weapon;
    Vector2 moveDirection;
    Vector2 mouseDirection;
    void Start()
    {
        
    }   
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        

        if (Input.GetMouseButtonDown(1))
        {
            weapon.Fire();
        }

        moveDirection  = new Vector2(moveX,moveY).normalized;
        mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
    void FixedUpdate()
    {

        rb.velocity = new Vector2(moveDirection.x * moveSpeed ,moveDirection.y * moveSpeed );
        Vector2 aimDirection = mouseDirection - rb.position;
        
        float aimAngle = Mathf.Atan2(aimDirection.y,aimDirection.x)* Mathf.Rad2Deg -90f;
        rb.rotation =aimAngle;
    }
}
