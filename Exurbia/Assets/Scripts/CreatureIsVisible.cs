using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIsVisible : MonoBehaviour
{
    Renderer m_Renderer;
    //reference to the Player script
    public PlayerMovement PlayerScript;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Renderer.isVisible)
        {
            Debug.Log("Creature is visible");
        }
        else Debug.Log("Creature is no longer visible");
    }
}
