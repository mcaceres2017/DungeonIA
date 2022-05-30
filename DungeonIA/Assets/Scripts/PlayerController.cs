using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public float moveSpeed = 4f;
    public Rigidbody2D rb;
    public GameObject Player;
    public Weapon weapon;
    Vector2 moveDirection;
    Vector2 mouseDirection;
    void Start()
    {
        
    }   
    void Update()
    {
        
        

        if (Input.GetMouseButtonDown(1))
        {
            weapon.Fire();
        }

        
        mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
    void FixedUpdate()
    {

       
        Vector2 aimDirection = mouseDirection - rb.position;
        gameObject.transform.position=Player.transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y,aimDirection.x)* Mathf.Rad2Deg -90f;
        rb.rotation =aimAngle;
    }
}
