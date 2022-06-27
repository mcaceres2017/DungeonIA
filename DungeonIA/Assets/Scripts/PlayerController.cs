using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public float moveSpeed = 4f;
    public Vector3 offset;
    //public float damage=10f;
    public Rigidbody2D rb;
    public GameObject Player;
    public Weapon weapon;
    public float WaitTime;
    public GameObject Weapon;
    Vector2 moveDirection;
    Vector2 mouseDirection;
    private float Timer;
    private Animator weaponAnimator;

    void Start()
    {
        weaponAnimator=weapon.GetComponent<Animator>();
    }   
    void Update()
    {
        
        
        
        if (Input.GetMouseButtonDown(1) && Timer<= 0)
        {
            weapon.Fire(Player.GetComponent<PlayerMovement>().damage);
            Timer=WaitTime;

        }
        if(Timer>= 0){
            weaponAnimator.SetBool("cooldown",true);
        }else{
            weaponAnimator.SetBool("cooldown",false);
        }
        
        Timer -= Time.deltaTime;
        
        mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
    void FixedUpdate()
    {

       
        Vector2 aimDirection = mouseDirection - rb.position;
        gameObject.transform.position=Player.transform.position + offset;
        float aimAngle = Mathf.Atan2(aimDirection.y,aimDirection.x)* Mathf.Rad2Deg -90f;
        rb.rotation =aimAngle;
    }
}
