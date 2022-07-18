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

    //Todo este codigo es como un puente entre el player
    // y el arma.....
    void Start()
    {
        weaponAnimator=weapon.GetComponent<Animator>();
    }   
    void Update()
    {
        
        /**
         * Segmento de disparo.
         * -------------------
         * Permite al jugador disparar con M1 si es que
         * el disparo no esta en cooldown.
         */
        
        if (Input.GetMouseButtonDown(0) && Timer<= 0)
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

       
        // Esto va moviendo la flecha que apunta para disparar junto
        // al jugador
        gameObject.transform.position=Player.transform.position + offset;

        // y todo esto de aqui es para "rotar" la flecha en la direccion
        // del mouse.
        Vector2 aimDirection = mouseDirection - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y,aimDirection.x)* Mathf.Rad2Deg -90f;
        rb.rotation =aimAngle;
    }
}
