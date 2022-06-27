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
    public float attackCD=1.5f;
    public Text scoreText;
    public Text levelText;
    public Rigidbody2D rb;
    public Animator animator;
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
    Vector2 moveDirection;
    
    
    
    // Start is called before the first frame update
    void Start()
    {   
        gameObject.name="Player";
        healBar.InitBar(maxheal);
        expBar.InitBar(10f);
        expBar.SetCurrentValue(exp);
        levelText.GetComponent<LevelScript>().levelValue=level;
        for(int i=1;i<level;i++){
            attackCD=attackCD*0.95f;
        }
        gameObject.GetComponentInChildren<PlayerController>().WaitTime=attackCD;
    }

    // Update is called once per frame
    void Update()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Horizontal"))
        {
            Hactive=true;
            steps.Play();

        }
        if(Input.GetButtonDown("Vertical"))
        {
            Vactive=true;
            steps.Play();
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




        if(moveX<0)
        {
            transform.localScale = new Vector3(-1.0f,1.0f,1.0f);

        }
        else if(moveX>=0)
        {
            transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }

        moveDirection  = new Vector2(moveX,moveY).normalized;
        animator.SetFloat("Speed",moveDirection.magnitude) ;
        damage= Mathf.Log(level)*basedamage+5;
        
    }
    public void GetMoney(int money){
        scoreText.GetComponent<ScoreScript>().ScoreValue+=money;
        goldSound.Play();
    }

    public void GetExp(float _exp){
        
        exp+= _exp;
        if(exp >=10)
        {
            exp=0;
            level++;
            levelText.GetComponent<LevelScript>().levelValue++;
            attackCD=attackCD*0.95f;
            gameObject.GetComponentInChildren<PlayerController>().WaitTime=attackCD;

        }
        expBar.SetCurrentValue(exp);
    }

    void FixedUpdate()
    {

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
    void OnDestroy()
    {
        Instantiate(explosionPrefab,gameObject.transform.position,gameObject.transform.rotation);
        
    }
}
