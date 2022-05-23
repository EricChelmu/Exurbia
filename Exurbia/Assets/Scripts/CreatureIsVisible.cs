using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIsVisible : MonoBehaviour
{
    //reference to the Player script
    public PlayerMovement PlayerScript;
    Renderer m_Renderer;
    CanvasGroup fadeAlpha;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        fadeAlpha = PlayerScript.fade.GetComponent<CanvasGroup>();
        fadeAlpha.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Renderer.isVisible)
        {
            if (PlayerScript.health <= 200)
            {
                PlayerScript.health += 20 * Time.deltaTime;
                if (PlayerScript.health >= 125)
                {
                    fadeAlpha.alpha += 0.001f;
                }
                if (PlayerScript.health >= 200)
                {
                    Destroy(PlayerScript.Player);
                }
            }
            Debug.Log(PlayerScript.health);
        }
        else if (!m_Renderer.isVisible)
        {
            if (PlayerScript.health >= 75)
            {
                PlayerScript.health -= 20 * Time.deltaTime;
                if (PlayerScript.health >= 125)
                {
                    fadeAlpha.alpha -= 0.001f;
                }
            }
            Debug.Log(PlayerScript.health);
        }
    }
}
