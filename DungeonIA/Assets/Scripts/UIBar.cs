using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{   
    private float maxBar;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    { 

        //slider= GetComponent<Slider>();
    }

    public void SetMax(float max){
        maxBar=max;
    }
    
    public void SetCurrentValue(float current){
        if(current==0) slider.value=0; 
        else slider.value=current/maxBar;
    }
    public void InitBar(float value){
        SetMax(value);
        SetCurrentValue(value);
    }
}
