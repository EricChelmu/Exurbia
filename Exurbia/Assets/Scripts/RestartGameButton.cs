using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGameButton : MonoBehaviour
{
    public void Restart_Button()
    {
        
        SceneManager.LoadScene(0);
        Debug.Log("next scene");
    }
}
    
