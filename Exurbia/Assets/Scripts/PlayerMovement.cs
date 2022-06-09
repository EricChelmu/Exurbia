using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Collecting references
    public GameObject fade;
    public GameObject Player;
    
    //Old Player movement stuff
    //public CharacterController characterController;
    //private float speed = 7f;
    //private float gravity = -15f;
    //private float jumpHeight = 2;

    //Player location parameters
    public float playerX;
    public float playerZ;

    //Player health parameters
    public float minHealth = 75;
    public float maxHealth = 200;
    public float health;

    //Energy parameters
    public int maxEnergy = 2000;
    public int currentEnergy;
    public EnergyBar energyBar;

    //Flashlight parameters
    public GameObject lightSource;
    public bool flashlightOn;

    //Old ground check
    //Check if the player is on the ground
    //public Transform groundCheck;
    //public float groundDistance = 0.4f;
    //public LayerMask groundMask;
    //Vector3 velocity;
    //bool isGrounded;

    //Player Inventory
    public Inventory inventory;
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool useFootsteps = true;


    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.1f;
    [SerializeField] private float sprintSpeed = 6.1f;
    [SerializeField] private float crouchSpeed = 1.6f;



    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.1f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.1f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.1f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.1f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.1f;
    [SerializeField] private float gravity = 30.1f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.6f;
    [SerializeField] private float standingHeight = 2.1f;
    [SerializeField] private float timeToCrouch = 0.26f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.6f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14.1f;
    [SerializeField] private float walkBobAmount = 0.06f;
    [SerializeField] private float sprintBobSpeed = 18.1f;
    [SerializeField] private float sprintBobAmount = 0.12f;
    [SerializeField] private float crouchBobSpeed = 8.1f;
    [SerializeField] private float crouchBobAmount = 0.026f;
    private float defaultYpos = 0;
    private float timer;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.6f;
    [SerializeField] private float crouchStepMultiplier = 1.6f;
    [SerializeField] private float sprintStepMultiplier = 1.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;
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
        //Base values
        health = minHealth;
        currentEnergy = 0;
        energyBar.SetMaxEnergy(currentEnergy, maxEnergy);

        //Updated movement stuff
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYpos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
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
        //Save location of player into a variable
        playerX = transform.position.x;
        playerZ = transform.position.z;

        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (canJump)
                HandleJump();

            if (canCrouch)
                HandleCrouch();

            if (canUseHeadbob)
                HandleHeadbob();
            if (useFootsteps)
                Handle_Footsteps();

            ApplyFinalMovements();

        }

        /*//Check if the player is grounded
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
        controller.Move(velocity * Time.deltaTime);*/

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
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventory.AddItem(item);
        }
    }
    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

    }

    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYpos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void Handle_Footsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/GRASS":
                        footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }
    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCentre = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCentre, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCentre;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }
}

