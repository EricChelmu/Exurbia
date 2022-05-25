using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIsVisible : MonoBehaviour
{
    //reference to the Player script
    public PlayerMovement PlayerScript;
    Renderer m_Renderer;
    CanvasGroup fadeAlpha;
    private float fadeSpeed = 0.0038f;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        fadeAlpha = PlayerScript.fade.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(PlayerScript.health);
        Debug.Log(fadeAlpha.alpha);
        if (m_Renderer.isVisible && !Physics.Linecast(Camera.main.transform.position, transform.position))
        {
            if (PlayerScript.health <= 200)
            {
                PlayerScript.health += 23 * Time.deltaTime;
                fadeAlpha.alpha += fadeSpeed;
                if (PlayerScript.health >= 200)
                {
                    Destroy(PlayerScript.Player);
                }
            }
        }
        else if (!m_Renderer.isVisible && Physics.Linecast(Camera.main.transform.position, transform.position))
        {
            if (PlayerScript.health >= 75)
            {
                PlayerScript.health -= 15 * Time.deltaTime;
                fadeAlpha.alpha -= fadeSpeed;
            }
        }
    }
}
