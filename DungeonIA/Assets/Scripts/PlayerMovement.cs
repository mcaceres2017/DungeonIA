using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{   
    public GameObject mapGenerator;
    public int maxheal=100;
    public int heal=100;
    public float moveSpeed = 5f;
    public float basedamage=5f;
    public float damage=10f;
    public int level =1;
    public float exp=0f;
    public float expToNextLevel = 1200f;
    public float attackCD=1.5f;
    public Text scoreText;
    public Text levelText;


    //Variables para el movimiento.
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 moveDirection;

    public AudioSource steps;
    public AudioSource hitSound;
    public AudioSource deathSound;
    public AudioSource healSound;
    public AudioSource goldSound;
    public GameObject explosionPrefab;
    public UIBar healBar;
    public UIBar expBar;
    private bool Hactive;
    private bool Vactive;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {   
        gameObject.name="Player";
        healBar.InitBar(maxheal);
        expBar.SetMax(expToNextLevel);
        expBar.SetCurrentValue(exp);
        levelText.GetComponent<levelScript>().levelValue=level;
        for(int i=1;i<level;i++){
            attackCD=attackCD*0.95f;
        }
        gameObject.GetComponentInChildren<PlayerController>().WaitTime=attackCD;
    }

    // Update is called once per frame
    void Update()
    {
    	updatePosition();
        


        /**
         * Segmento sonido al caminar.
         * --------------------
    	 * Cada vez que el jugador se este moviendo
    	 * se reproduce el sonido de caminata, si el jugador
    	 * se detiene por completo, el sonido se detiene.
    	 */


        if(Input.GetButtonDown("Horizontal"))
        {
            if(Vactive==false)
            {
                Hactive=true;
                steps.Play(); 
            }
        }
        if(Input.GetButtonDown("Vertical"))
        {
            if(Hactive==false)
            {
                Vactive=true;
                steps.Play();
            }
        }

        if(Input.GetButtonUp("Horizontal"))
        {
            Hactive=false;
            if(Vactive==false)
            {
                steps.Pause();

            }
        }
        if(Input.GetButtonUp("Vertical"))
        {
            Vactive=false;
            if(Hactive==false)
            {
                steps.Pause();
            }
        }


        //###################################
        //???.
        //###################################
        damage= Mathf.Log(level)*basedamage+5;   
    }


    /**
     * Segmento movimientos.
     * --------------------
     * Este segmento "captura" la direccion en la que 
     * el jugador debe moverse (al apretarse una tecla)
     * y setea la animacion correcta para cada orientacion
     */

    private void updatePosition()
    {
    	float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX<0)
        {
            transform.localScale = new Vector3(-1.0f,1.0f,1.0f);

        }
        else if(moveX>=0)
        {
            transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }

        //El normalized es para eliminar el efecto de "derecha + arriba = doble de velocidad"
        moveDirection  = new Vector2(moveX,moveY).normalized;
        animator.SetFloat("Speed",moveDirection.sqrMagnitude);
    }





    public void GetMoney(int money)
    {
        scoreText.GetComponent<ScoreScript>().ScoreValue+=money;
        goldSound.Play();
    }

    public void GetExp(float _exp)
    {
        
        exp+= _exp;
        if(exp >= expToNextLevel)
        {
            exp=0;
            level++;
            levelText.GetComponent<levelScript>().levelValue++;
            attackCD=attackCD*0.95f;
            gameObject.GetComponentInChildren<PlayerController>().WaitTime=attackCD;
            expToNextLevel = expToNextLevel * 1.2f;
        }

        expBar.SetCurrentValue(exp);
        expBar.SetMax(expToNextLevel);
    }

    void FixedUpdate()
    {
    	//Esto permite que el jugador se mueva.
        rb.velocity = new Vector2(moveDirection.x * moveSpeed ,moveDirection.y * moveSpeed );
    }

    public void takeHit(float dam)
    {
        heal -=  (int)dam;
        healBar.SetCurrentValue(heal);

        if( heal == 0 )
        {
            deathSound.Play();
           
            //Destroy(gameObject,0.5f);
        }
        else
        {
            hitSound.Play();

        }

    }
    
    public void maxhealPlayer()
    {
        heal=maxheal;
        healBar.SetCurrentValue(heal);
        healSound.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {       
        GameObject collisionGameObject = collision.gameObject;
    
    }
}
