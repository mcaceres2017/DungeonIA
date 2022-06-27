using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealBar : MonoBehaviour
{
    private float maxheal;
    public Slider slider;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
    
        
    }
    public void SetMaxHeal(float max){
        maxheal=max;
    }
    
    public void SetCurrentHeal(float current){
        if(current==0) slider.value=0; 
        else slider.value=current/maxheal;
    }
    public void InitHealBar(float heal){
        SetMaxHeal(heal);
        SetCurrentHeal(heal);
    }
    


    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position+ offset);    
    }
}
