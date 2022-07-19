using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelScript : MonoBehaviour
{
    public int levelValue=1;
    Text score;
    // Start is called before the first frame update
    void Start()
    {
        score = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text="" + levelValue;
    }
}