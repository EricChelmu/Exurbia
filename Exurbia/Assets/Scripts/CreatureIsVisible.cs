using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatureIsVisible : MonoBehaviour
{
    //reference to the Player script
    [SerializeField] GameObject Creature;
    public PlayerMovement PlayerScript;
    Renderer m_Renderer;
    CanvasGroup fadeAlpha;
    private float fadeSpeed = 0.007f;
    private float playerX;
    private float playerY;
    private float playerZ;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        fadeAlpha = PlayerScript.fade.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerX = PlayerScript.transform.position.x + 2;
        playerY = PlayerScript.transform.position.y;
        playerZ = PlayerScript.transform.position.z + 2;
        Debug.Log(PlayerScript.health);
        Debug.Log(fadeAlpha.alpha);
        if (m_Renderer.isVisible && !Physics.Linecast(Camera.main.transform.position, transform.position))
        {
            if (PlayerScript.health <= 200)
            {
                //soundhere
                PlayerScript.health += 45 * Time.deltaTime;
                fadeAlpha.alpha += fadeSpeed;
                if (PlayerScript.health >= 190)
                {
                    Creature.transform.position = new Vector3(playerX, playerY, playerZ);
                    Creature.transform.rotation = Camera.main.transform.rotation;
                }
                if (PlayerScript.health >= 200)
                {
                    SceneManager.LoadScene(3);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
        else if (!m_Renderer.isVisible && Physics.Linecast(Camera.main.transform.position, transform.position))
        {
            if (PlayerScript.health >= 75)
            {
                PlayerScript.health -= 35 * Time.deltaTime;
                fadeAlpha.alpha -= fadeSpeed;
            }
        }
    }
}
