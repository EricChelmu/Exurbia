using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start_Game_Button()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("next scene");
    }
}
