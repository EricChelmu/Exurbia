using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player movement parameters
    public CharacterController controller;
    private float speed = 12f;
    private float gravity = -15f;
    private float jumpHeight = 2;

    //Player location parameters
    public float playerX;
    public float playerZ;

    //Player health parameters
    public float minHealth = 75;
    public float maxHealth = 200;
    public float health;

    //Energy parameters
    public int maxEnergy = 50000;
    public int currentEnergy;
    public EnergyBar energyBar;

    //Flashlight parameters
    public GameObject lightSource;
    public bool flashlightOn;

    //Check if the player is on the ground
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        //Tie this script to the SelectionManager script
        GameObject selectionManagerObject = GameObject.FindWithTag("SelectionManager");
        if (selectionManagerObject != null)
        {
            SelectionManager selectionManagerScript = selectionManagerObject.GetComponent<SelectionManager>();
            if (selectionManagerScript != null)
            {
                selectionManagerScript.PlayerScript = this;

            }
        }

        //Tie this script to the CreatureSpawner script
        GameObject creatureSpawnerObject = GameObject.FindWithTag("CreatureSpawner");
        if (creatureSpawnerObject != null)
        {
            CreatureBehaviour spawnControllerScript = creatureSpawnerObject.GetComponent<CreatureBehaviour>();
            if (spawnControllerScript != null)
            {
                spawnControllerScript.PlayerScript = this;

            }
        }

        //Tie this script to the CreatureIsVisible script
        GameObject creatureIsVisibleObject = GameObject.FindWithTag("CreatureIsVisible");
        if (creatureIsVisibleObject != null)
        {
            CreatureIsVisible creatureIsVisibleScript = creatureIsVisibleObject.GetComponent<CreatureIsVisible>();
            if (creatureIsVisibleScript != null)
            {
                creatureIsVisibleScript.PlayerScript = this;
            }
        }
        health = minHealth;
        currentEnergy = 0;
        maxEnergy = 50000;
        energyBar.SetMaxEnergy(currentEnergy, maxEnergy);
    }
    // Update is called once per frame
    void Update()
    {
        //Save location of player into a variable
        playerX = transform.position.x;
        playerZ = transform.position.z;

        //Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Move the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //Player jumping
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
    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
