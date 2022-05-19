using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        //on load generates the next scene in the queue (being the game) also possible to call a named scene or indexed scene.
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(); 
        //closes the application, debug for checking if it works.
    }

}
