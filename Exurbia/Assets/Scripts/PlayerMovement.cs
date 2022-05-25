using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private float speed = 7f;
    private float gravity = -15f;
    private float jumpHeight = 2;
    public float playerX;
    public float playerZ;
    public int health = 100;

    public int maxEnergy = 50000;
    public int currentEnergy;
    public EnergyBar energyBar;
    public GameObject lightSource;
    public bool flashlightOn;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        GameObject selectionManagerObject = GameObject.FindWithTag("SelectionManager");
        if (selectionManagerObject != null)
        {
            SelectionManager gameControllerScript = selectionManagerObject.GetComponent<SelectionManager>();
            if (gameControllerScript != null)
            {
                gameControllerScript.PlayerScript = this;

            }
        }
        GameObject creatureSpawnerObject = GameObject.FindWithTag("CreatureSpawner");
        if (creatureSpawnerObject != null)
        {
            CreatureBehaviour spawnControllerScript = creatureSpawnerObject.GetComponent<CreatureBehaviour>();
            if (spawnControllerScript != null)
            {
                spawnControllerScript.PlayerScript = this;

            }
        }
        currentEnergy = 0;
        maxEnergy = 50000;
        energyBar.SetMaxEnergy(currentEnergy, maxEnergy);
    }
    // Update is called once per frame
    void Update()
    {
        playerX = transform.position.x;
        playerZ = transform.position.z;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //Turn on flashlight
        if (Input.GetKeyDown("f"))
        {
            if (flashlightOn == false)
            {
                lightSource.GetComponent<Light>().enabled = true;
                flashlightOn = true;
            }
            else if (flashlightOn == true)
            {
                lightSource.GetComponent<Light>().enabled = false;
                flashlightOn = false;
            }
        }
    }
    public void GenerateEnergy(int generateEnergy)
    {
        currentEnergy += generateEnergy;
        energyBar.SetEnergy(currentEnergy);
    }
    public void DepleteEnergy(int depleteEnergy)
    {
        currentEnergy -= depleteEnergy;
        energyBar.SetEnergy(currentEnergy);
    }
}
