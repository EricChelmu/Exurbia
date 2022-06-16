using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    //Reference to texts when objects are interacted with
    [SerializeField] private GameObject ObjectNoPaper;
    [SerializeField] private GameObject NoPower;
    [SerializeField] private GameObject CablePicked;
    [SerializeField] private GameObject GenRestored;
    [SerializeField] private GameObject Light1;
    [SerializeField] private GameObject GenNoCable;
    [SerializeField] private GameObject WMachineText;
    [SerializeField] private GameObject MicrowaveText;
    [SerializeField] private GameObject TVText;
    [SerializeField] private GameObject LaptopText;
    [SerializeField] private GameObject RadioText;
    [SerializeField] private GameObject BreakerBoxText;
    [SerializeField] private GameObject FridgeText;
    [SerializeField] private GameObject CarText;

    //Reference to text on papers related to objects
    [SerializeField] private GameObject PaperGenText;
    [SerializeField] private GameObject PaperCarText;
    [SerializeField] private GameObject PaperFridgeText;
    [SerializeField] private GameObject PaperLaptopText;
    [SerializeField] private GameObject PaperMicrowaveText;
    [SerializeField] private GameObject PaperRadioText;
    [SerializeField] private GameObject PaperTVText;
    [SerializeField] private GameObject PaperWMachineText;

    //Reference to papers
    [SerializeField] private GameObject PaperGen;
    [SerializeField] private GameObject PaperCar;
    [SerializeField] private GameObject PaperFridge;
    [SerializeField] private GameObject PaperLaptop;
    [SerializeField] private GameObject PaperMicrowave;
    [SerializeField] private GameObject PaperRadio;
    [SerializeField] private GameObject PaperTV;
    [SerializeField] private GameObject PaperWMachine;

    //Buttons & Questions
    [SerializeField] private GameObject GenButton1;
    [SerializeField] private GameObject GenButton2Right;
    [SerializeField] private GameObject GenButton3;
    [SerializeField] private GameObject GenButton4;
    [SerializeField] private GameObject WMachineButton1;
    [SerializeField] private GameObject WMachineButton2;
    [SerializeField] private GameObject WMachineButton3;
    [SerializeField] private GameObject WMachineButton4Right;
    [SerializeField] private GameObject MicrowaveButton1Right;
    [SerializeField] private GameObject MicrowaveButton2;
    [SerializeField] private GameObject MicrowaveButton3;
    [SerializeField] private GameObject MicrowaveButton4;
    [SerializeField] private GameObject TVButton1;
    [SerializeField] private GameObject TVButton2;
    [SerializeField] private GameObject TVButton3Right;
    [SerializeField] private GameObject TVButton4;
    [SerializeField] private GameObject LaptopButton1;
    [SerializeField] private GameObject LaptopButton2;
    [SerializeField] private GameObject LaptopButton3;
    [SerializeField] private GameObject LaptopButton4Right;
    [SerializeField] private GameObject FridgeButton1Right;
    [SerializeField] private GameObject FridgeButton2;
    [SerializeField] private GameObject FridgeButton3;
    [SerializeField] private GameObject FridgeButton4;
    [SerializeField] private GameObject RadioButton1;
    [SerializeField] private GameObject RadioButton2Right;
    [SerializeField] private GameObject RadioButton3;
    [SerializeField] private GameObject RadioButton4;
    [SerializeField] private GameObject CarButton1;
    [SerializeField] private GameObject CarButton2;
    [SerializeField] private GameObject CarButton3Right;
    [SerializeField] private GameObject CarButton4;
    [SerializeField] private GameObject GenQuestion;
    [SerializeField] private GameObject WMachineQuestion;
    [SerializeField] private GameObject MicrowaveQuestion;
    [SerializeField] private GameObject TVQuestion;
    [SerializeField] private GameObject LaptopQuestion;
    [SerializeField] private GameObject FridgeQuestion;
    [SerializeField] private GameObject RadioQuestion;
    [SerializeField] private GameObject CarQuestion;

    private Transform _selection;
    private GameObject PaperGenClone;
    private GameObject PaperGenTextClone;
    public PlayerMovement PlayerScript;
    public LightOffOn LightScript;

    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] public TMP_Text pageCounter;
    [SerializeField] public TMP_Text partCounter;
    private int pages = 0;
    private int parts = 0;
    private int readTimer = 6;
    private float distPlayerObj;
    bool alreadyCountedGen = false;
    bool alreadyCountedCar = false;
    bool alreadyCountedWMachine = false;
    bool alreadyCountedLaptop = false;
    bool alreadyCountedMicrowave = false;
    bool alreadyCountedRadio = false;
    bool alreadyCountedFridge = false;
    bool alreadyCountedTV = false;
    bool alreadyCountedCablePart = false;
    bool alreadyCountedCarPart = false;
    bool alreadyCountedWMachinePart = false;
    bool alreadyCountedLaptopPart = false;
    bool alreadyCountedMicrowavePart = false;
    bool alreadyCountedRadioPart = false;
    bool alreadyCountedFridgePart = false;
    bool alreadyCountedTVPart = false;
    bool quizOpen = false;
   
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.paperRead == true && Input.GetKeyDown("e"))
        {
            Destroy(PaperGenClone);
            Destroy(PaperGenTextClone);
            GameManager.Instance.paperRead = false;
        }
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        //Create the ray which is used to check if the player is interacting with an interactible object
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            distPlayerObj = Vector3.Distance(PlayerScript.transform.position, hit.transform.position);
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag) && distPlayerObj <= 7f && quizOpen == false)
            {
                //Check if you pick up a cable
                if (hit.transform.gameObject.name == "Cable")
                {
                    //Highlight the object yellow
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the item selected gets destroyed and picked up
                        if (Input.GetKeyDown("e"))
                        {
                            Destroy(hit.transform.gameObject);
                            GameManager.Instance.cablePicked = true;
                            //Display text on screen
                            GameObject Clone = Instantiate(CablePicked, new Vector3(0, -709, 0), transform.rotation);
                            Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                            Destroy(Clone, readTimer);
                            if (GameManager.Instance.cablePicked == true && alreadyCountedCablePart == false)
                            {
                                alreadyCountedCablePart = true;
                                parts++;
                            }
                            partCounter.text = parts.ToString();
                        }
                    }
                }
                //Check if you interact with a generator
                if (hit.transform.gameObject.name == "Generator")
                {
                    //Highlight the object yellow
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the generator gets repaired and it produces energy
                        if (Input.GetKeyDown("e"))
                        {
                            //If interacted with without cable
                            if (GameManager.Instance.cablePicked == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(GenNoCable, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }


                            //Start quiz if cable was picked and paper was read
                            if (GameManager.Instance.cablePicked == true && GameManager.Instance.generatorOn == false && GameManager.Instance.paperGenRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(GenButton1, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(GenButton2Right, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(GenButton3, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(GenButton4, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(GenQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;
                                
                                Button GenButton2RightComp = ButtonClone2.GetComponent<Button>();
                                GenButton2RightComp.onClick.AddListener(runGenerator);
                                Button GenButton1Comp = ButtonClone1.GetComponent<Button>();
                                GenButton1Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3Comp = ButtonClone3.GetComponent<Button>();
                                GenButton3Comp.onClick.AddListener(quitQuiz);
                                Button GenButton4Comp = ButtonClone4.GetComponent<Button>();
                                GenButton4Comp.onClick.AddListener(quitQuiz);


                                void quitQuiz()
                                {
                                    Debug.Log("Quit Quiz");
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runGenerator()
                                {
                                    CreateEnergy(2000);
                                    //Display text on screen
                                    GameObject Clone2 = Instantiate(GenRestored, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.generatorOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }
                            }
                        }
                    }
                }
                //Check if you interact with light switch
                if (hit.transform.gameObject.name == "Light Switch1")
                {
                    //Highlight the object yellow
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the light switch turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.lightOn == false)
                            {
                                //turn light on
                                GameManager.Instance.lightOn = true;
                                LightScript.TurnOnLight();
                                ReduceEnergy(10);
                            }
                            else if (GameManager.Instance.lightOn == true)
                            {
                                //turn light off
                                GameManager.Instance.lightOn = false;
                                LightScript.TurnOffLight();
                            }
                        }
                    }
                }
                //Check if you interact with washing machine
                if (hit.transform.gameObject.name == "Washing Machine")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the washing machine turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.WMachineOn == false && GameManager.Instance.paperWMachineRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.WMachineOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.WMachineOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperWMachineRead == true)
                            
                            {
                                GameObject ButtonClone1 = Instantiate(WMachineButton1, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(WMachineButton2, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(WMachineButton3, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(WMachineButton4Right, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(WMachineQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                
                                Button GenButton1Comp = ButtonClone1.GetComponent<Button>();
                                GenButton1Comp.onClick.AddListener(quitQuiz);
                                Button GenButton2Comp = ButtonClone2.GetComponent<Button>();
                                GenButton2Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3Comp = ButtonClone3.GetComponent<Button>();
                                GenButton3Comp.onClick.AddListener(quitQuiz);
                                Button GenButton4RightComp = ButtonClone4.GetComponent<Button>();
                                GenButton4RightComp.onClick.AddListener(runWMachine);


                                void quitQuiz()
                                {
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runWMachine()
                                {
                                    ReduceEnergy(400);
                                    GameObject Clone2 = Instantiate(WMachineText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, readTimer);
                                    GameManager.Instance.WMachineOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.WMachineOn == true && alreadyCountedWMachinePart == false)
                                    {
                                        alreadyCountedWMachinePart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperGen")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperGen, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperGenText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperGenRead = true;
                                if (GameManager.Instance.paperGenRead == true && alreadyCountedGen == false)
                                {
                                    alreadyCountedGen = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a TV
                if (hit.transform.gameObject.name == "TV")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the TV is turned on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.tvOn == false && GameManager.Instance.paperTVRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.tvOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.tvOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperTVRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(TVButton1, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(TVButton2, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(TVButton3Right, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(TVButton4, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(TVQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                Button GenButton1Comp = ButtonClone1.GetComponent<Button>();
                                GenButton1Comp.onClick.AddListener(quitQuiz);
                                Button GenButton2Comp = ButtonClone2.GetComponent<Button>();
                                GenButton2Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3CompRight = ButtonClone3.GetComponent<Button>();
                                GenButton3CompRight.onClick.AddListener(runTV);
                                Button GenButton4Comp = ButtonClone4.GetComponent<Button>();
                                GenButton4Comp.onClick.AddListener(quitQuiz);


                                void quitQuiz()
                                {
                                    Debug.Log("Quit Quiz");
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runTV()
                                {
                                    ReduceEnergy(100);
                                    GameObject Clone2 = Instantiate(TVText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.tvOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.tvOn == true && alreadyCountedTVPart == false)
                                    {
                                        alreadyCountedTVPart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with a Radio
                if (hit.transform.gameObject.name == "Radio")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.radioOn == false && GameManager.Instance.paperRadioRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.radioOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.radioOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperRadioRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(RadioButton1, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(RadioButton2Right, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(RadioButton3, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(RadioButton4, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(RadioQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                Button GenButton1Comp = ButtonClone1.GetComponent<Button>();
                                GenButton1Comp.onClick.AddListener(quitQuiz);
                                Button GenButton2CompRight = ButtonClone2.GetComponent<Button>();
                                GenButton2CompRight.onClick.AddListener(runRadio);
                                Button GenButton3Comp = ButtonClone3.GetComponent<Button>();
                                GenButton3Comp.onClick.AddListener(quitQuiz);
                                Button GenButton4Comp = ButtonClone4.GetComponent<Button>();
                                GenButton4Comp.onClick.AddListener(quitQuiz);


                                void quitQuiz()
                                {
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runRadio()
                                {
                                    ReduceEnergy(10);
                                    GameObject Clone2 = Instantiate(RadioText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.radioOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.radioOn == true && alreadyCountedRadioPart == false)
                                    {
                                        alreadyCountedRadioPart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with a Fridge
                if (hit.transform.gameObject.name == "Fridge")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the microwave turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.fridgeOn == false && GameManager.Instance.paperFridgeRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.fridgeOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.fridgeOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperFridgeRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(FridgeButton1Right, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(FridgeButton2, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(FridgeButton3, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(FridgeButton4, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(FridgeQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                Button GenButton1CompRight = ButtonClone1.GetComponent<Button>();
                                GenButton1CompRight.onClick.AddListener(runFridge);
                                Button GenButton2Comp = ButtonClone2.GetComponent<Button>();
                                GenButton2Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3Comp = ButtonClone3.GetComponent<Button>();
                                GenButton3Comp.onClick.AddListener(quitQuiz);
                                Button GenButton4Comp = ButtonClone4.GetComponent<Button>();
                                GenButton4Comp.onClick.AddListener(quitQuiz);


                                void quitQuiz()
                                {
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runFridge()
                                {
                                    ReduceEnergy(250);
                                    GameObject Clone2 = Instantiate(FridgeText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.fridgeOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.fridgeOn == true && alreadyCountedFridgePart == false)
                                    {
                                        alreadyCountedFridgePart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with a Microwave
                if (hit.transform.gameObject.name == "Microwave")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.microwaveOn == false && GameManager.Instance.paperMicrowaveRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.microwaveOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.microwaveOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperMicrowaveRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(MicrowaveButton1Right, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(MicrowaveButton2, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(MicrowaveButton3, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(MicrowaveButton4, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(MicrowaveQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                Button GenButton1CompRight = ButtonClone1.GetComponent<Button>();
                                GenButton1CompRight.onClick.AddListener(runMicrowave);
                                Button GenButton2Comp = ButtonClone2.GetComponent<Button>();
                                GenButton2Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3Comp = ButtonClone3.GetComponent<Button>();
                                GenButton3Comp.onClick.AddListener(quitQuiz);
                                Button GenButton4Comp = ButtonClone4.GetComponent<Button>();
                                GenButton4Comp.onClick.AddListener(quitQuiz);


                                void quitQuiz()
                                {
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runMicrowave()
                                {
                                    ReduceEnergy(600);
                                    GameObject Clone2 = Instantiate(MicrowaveText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.microwaveOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.microwaveOn == true && alreadyCountedMicrowavePart == false)
                                    {
                                        alreadyCountedMicrowavePart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with a Laptop
                if (hit.transform.gameObject.name == "Laptop")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.laptopOn == false && GameManager.Instance.paperLaptopRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.laptopOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.laptopOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperLaptopRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(LaptopButton1, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(LaptopButton2, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(LaptopButton3, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(LaptopButton4Right, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(LaptopQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                Button GenButton1Comp = ButtonClone1.GetComponent<Button>();
                                GenButton1Comp.onClick.AddListener(quitQuiz);
                                Button GenButton2Comp = ButtonClone2.GetComponent<Button>();
                                GenButton2Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3Comp = ButtonClone3.GetComponent<Button>();
                                GenButton3Comp.onClick.AddListener(quitQuiz);
                                Button GenButton4CompRight = ButtonClone4.GetComponent<Button>();
                                GenButton4CompRight.onClick.AddListener(runLaptop);


                                void quitQuiz()
                                {
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runLaptop()
                                {
                                    ReduceEnergy(100);
                                    GameObject Clone2 = Instantiate(LaptopText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.laptopOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.laptopOn == true && alreadyCountedLaptopPart == false)
                                    {
                                        alreadyCountedLaptopPart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with a Breaker Box
                if (hit.transform.gameObject.name == "Breaker Box")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.breakerBoxOn == false && GameManager.Instance.generatorOn == true)
                            {
                                ReduceEnergy(100);
                                GameObject Clone2 = Instantiate(BreakerBoxText, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, 3);
                                GameManager.Instance.breakerBoxOn = true;
                            }
                        }
                    }
                }
                //Check if you interact with a Car
                if (hit.transform.gameObject.name == "Car")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.carOn == false && GameManager.Instance.paperCarRead == false)
                            {
                                GameObject Clone = Instantiate(ObjectNoPaper, new Vector3(0, -709, 0), transform.rotation);
                                Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone, readTimer);
                            }

                            if (GameManager.Instance.carOn == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(NoPower, new Vector3(0, -709, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, readTimer);
                            }

                            if (GameManager.Instance.carOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.paperCarRead == true)
                            {
                                GameObject ButtonClone1 = Instantiate(CarButton1, new Vector3(-319, -192, 0), transform.rotation);
                                ButtonClone1.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone2 = Instantiate(CarButton2, new Vector3(394, -192, 0), transform.rotation);
                                ButtonClone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone3 = Instantiate(CarButton3Right, new Vector3(-319, -387, 0), transform.rotation);
                                ButtonClone3.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject ButtonClone4 = Instantiate(CarButton4, new Vector3(394, -387, 0), transform.rotation);
                                ButtonClone4.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameObject QuestionClone = Instantiate(CarQuestion, new Vector3(39.87759f, 22.2507f, 0), transform.rotation);
                                QuestionClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                quizOpen = true;

                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;

                                Button GenButton1Comp = ButtonClone1.GetComponent<Button>();
                                GenButton1Comp.onClick.AddListener(quitQuiz);
                                Button GenButton2Comp = ButtonClone2.GetComponent<Button>();
                                GenButton2Comp.onClick.AddListener(quitQuiz);
                                Button GenButton3CompRight = ButtonClone3.GetComponent<Button>();
                                GenButton3CompRight.onClick.AddListener(runCar);
                                Button GenButton4Comp = ButtonClone4.GetComponent<Button>();
                                GenButton4Comp.onClick.AddListener(quitQuiz);


                                void quitQuiz()
                                {
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                }

                                void runCar()
                                {
                                    ReduceEnergy(100);
                                    GameObject Clone2 = Instantiate(CarText, new Vector3(0, -709, 0), transform.rotation);
                                    Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                    Destroy(Clone2, 3);
                                    GameManager.Instance.carOn = true;
                                    Destroy(ButtonClone1);
                                    Destroy(ButtonClone2);
                                    Destroy(ButtonClone3);
                                    Destroy(ButtonClone4);
                                    Destroy(QuestionClone);
                                    Cursor.visible = false;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    quizOpen = false;
                                    if (GameManager.Instance.carOn == true && alreadyCountedCarPart == false)
                                    {
                                        alreadyCountedCarPart = true;
                                        parts++;
                                    }
                                    partCounter.text = parts.ToString();
                                }
                            }
                        }
                    }
                }
                //Check if you interact with the Radio Tower
                if (hit.transform.gameObject.name == "RadioTower")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.radioTowerOn == false && GameManager.Instance.generatorOn == true && GameManager.Instance.WMachineOn == true && GameManager.Instance.carOn == true && GameManager.Instance.laptopOn == true && GameManager.Instance.fridgeOn == true && GameManager.Instance.microwaveOn == true && GameManager.Instance.radioOn == true && GameManager.Instance.tvOn == true)
                            {
                                GameManager.Instance.radioTowerOn = true;
                                SceneManager.LoadScene(4);
                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperCar")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperCar, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperCarText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperCarRead = true;
                                if (GameManager.Instance.paperCarRead == true && alreadyCountedCar == false)
                                {
                                    alreadyCountedCar = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperFridge")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperFridge, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperFridgeText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperFridgeRead = true;
                                if (GameManager.Instance.paperFridgeRead == true && alreadyCountedFridge == false)
                                {
                                    alreadyCountedFridge = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperLaptop")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperLaptop, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperLaptopText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperLaptopRead = true;
                                if (GameManager.Instance.paperLaptopRead == true && alreadyCountedLaptop == false)
                                {
                                    alreadyCountedLaptop = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperMicrowave")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperMicrowave, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperMicrowaveText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperMicrowaveRead = true;
                                if (GameManager.Instance.paperMicrowaveRead == true && alreadyCountedMicrowave == false)
                                {
                                    alreadyCountedMicrowave = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperRadio")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperRadio, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperRadioText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperRadioRead = true;
                                if (GameManager.Instance.paperRadioRead == true && alreadyCountedRadio == false)
                                {
                                    alreadyCountedRadio = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperTV")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperTV, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperTVText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperTVRead = true;
                                if (GameManager.Instance.paperTVRead == true && alreadyCountedTV == false)
                                {
                                    alreadyCountedTV = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperWMachine")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the paper is picked up and showed first person
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.paperRead == false)
                            {
                                PaperGenClone = Instantiate(PaperWMachine, new Vector3(0.22f, 0.15f, 0.778f), new Quaternion(-250.297f, -240f, -45.60501f, 0));
                                PaperGenClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform, false);
                                PaperGenTextClone = Instantiate(PaperWMachineText, new Vector3(0, 0, 0), transform.rotation);
                                PaperGenTextClone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                GameManager.Instance.paperRead = true;
                                GameManager.Instance.paperWMachineRead = true;
                                if (GameManager.Instance.paperWMachineRead == true && alreadyCountedWMachine == false)
                                {
                                    alreadyCountedWMachine = true;
                                    pages++;
                                }
                                pageCounter.text = pages.ToString();
                            }
                        }
                    }
                }
                _selection = selection;
            }

        }
    }
    public void CreateEnergy(int generateEnergy)
    {
        if (PlayerScript != null)
        {
            PlayerScript.GenerateEnergy(generateEnergy);
        }
    }
    public void ReduceEnergy(int depleteEnergy)
    {
        if (PlayerScript != null)
        {
            PlayerScript.DepleteEnergy(depleteEnergy);
        }
    }
}