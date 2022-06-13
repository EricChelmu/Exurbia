using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour

{
    public void Victory_return()
    {

        SceneManager.LoadScene(0);
        Debug.Log("next scene");
    }
}