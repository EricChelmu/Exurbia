using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOffOn : MonoBehaviour
{
    private void Start()
    {
        GameObject selectionManagerObject = GameObject.FindWithTag("SelectionManager");
        if (selectionManagerObject != null)
        {
            SelectionManager gameControllerScript = selectionManagerObject.GetComponent<SelectionManager>();
            if (gameControllerScript != null)
            {
                gameControllerScript.LightScript = this;
            }
        }
    }
    
    public void TurnOnLight()
    {
        this.GetComponent<Light>().enabled = true;
    }
    public void TurnOffLight()
    {
        this.GetComponent<Light>().enabled = false;
    }
}
