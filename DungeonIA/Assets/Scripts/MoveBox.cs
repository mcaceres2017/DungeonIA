using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{   public bool isX=true;
    public float force=10f;
    public Rigidbody2D rb;

    public void Move(Vector3 bullet){
        //X
        if(isX){
            //izquierda
            if(bullet.x< gameObject.transform.position.x){
                //abajo
                if(bullet.y< gameObject.transform.position.y){
                    //"izquierda-abajo";
                }
                //arriba
                else{
                    //"izquierda-arriba";
                }
                rb.AddForce(new Vector3(-1,0,0) *force,ForceMode2D.Impulse);

            }
            //derecha
            else{
                //abajo
                if(bullet.y< gameObject.transform.position.y){
                    //"derecha-abajo";
                }
                //arriba
                else{
                    //"derecha arriba";
                }
                rb.AddForce(new Vector3(1,0,0) *force,ForceMode2D.Impulse);
            }
        }
        //Y
        else{
            //abajo
            if(bullet.y< gameObject.transform.position.y){
                rb.AddForce(new Vector3(0,-1,0) *force,ForceMode2D.Impulse);    

            }
            //arriba
            else{
                rb.AddForce(new Vector3(0,1,0) *force,ForceMode2D.Impulse);      

            }
            

        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.tag == "bullet" )
        {  
            Move(other.gameObject.transform.position);
                 
        }
        
        
    }
}
