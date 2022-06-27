using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{   public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(gameObject,0.3f);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
