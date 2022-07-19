using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class ManagerScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartGame(){
        SceneManager.LoadScene("game");

    }
    public void QuitGame(){
        Debug.Log("Quit");
        Application.Quit();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
