using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatureIsVisible : MonoBehaviour
{
    //reference to the Player script
    [SerializeField] private GameObject Creature;
    [SerializeField] private AudioSource creatureScreamSource;
    [SerializeField] private AudioClip creatureScreamClip;
    public PlayerMovement PlayerScript;
    Renderer m_Renderer;
    CanvasGroup fadeAlpha;
    private float fadeSpeed = 0.007f;
    private float playerPosX;
    private float playerPosY;
    private float playerPosZ;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        fadeAlpha = PlayerScript.fade.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPosX = PlayerScript.transform.position.x + 2;
        playerPosY = PlayerScript.transform.position.y;
        playerPosZ = PlayerScript.transform.position.z + 2;
        if (m_Renderer.isVisible && !Physics.Linecast(Camera.main.transform.position, transform.position))
        {
            creatureScreamSource.PlayOneShot(creatureScreamClip);
            if (PlayerScript.health <= 200)
            {
                PlayerScript.health += 46 * Time.deltaTime;
                fadeAlpha.alpha += fadeSpeed;
                if (PlayerScript.health >= 190)
                {
                    Creature.transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
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
        else if (!m_Renderer.isVisible)
        {
            creatureScreamSource.Stop();
            if (PlayerScript.health >= 75)
            {
                PlayerScript.health -= 35 * Time.deltaTime;
                fadeAlpha.alpha -= fadeSpeed;
            }
        }
    }
}
