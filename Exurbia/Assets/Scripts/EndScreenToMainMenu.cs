using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenToMainMenu : MonoBehaviour

{
    public void Victory_return()
    {
        SceneManager.LoadScene(0);
    }
}