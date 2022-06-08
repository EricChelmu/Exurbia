using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject CablePicked;
    [SerializeField] private GameObject GenRestored;
    [SerializeField] private GameObject Light1;
    [SerializeField] private GameObject PaperGen;
    [SerializeField] private GameObject PaperGenText;
    [SerializeField] private GameObject GenNoCable;
    [SerializeField] private GameObject a;
    [SerializeField] private GameObject b;
    [SerializeField] private GameObject c;
    [SerializeField] private GameObject d;
    [SerializeField] private GameObject e;
    [SerializeField] private GameObject f;

    public float distPlayerObj;

    private Transform _selection;
    private GameObject PaperGenClone;
    private GameObject PaperGenTextClone;
    public PlayerMovement PlayerScript;
    public LightOffOn LightScript;
    // Start is called before the first frame update
    void Start()
    {

    }

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
            Debug.Log(distPlayerObj);
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag) && distPlayerObj <= 3f)
            {
                //Check if you pick up a cable
                if (hit.transform.gameObject.name == "Cable")
                {
                    //Highlight the object yellow
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the item selected gets destroyed and picked up
                        if (Input.GetKeyDown("e"))
                        {
                            Destroy(hit.transform.gameObject);
                            GameManager.Instance.cablePicked = true;
                            //Display text on screen
                            GameObject Clone = Instantiate(CablePicked, new Vector3(0, -316, 0), transform.rotation);
                            Clone.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                            Destroy(Clone, 3);
                        }
                    }
                }
                //Check if you interact with a generator
                if (hit.transform.gameObject.name == "Generator")
                {
                    //Highlight the object yellow
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the generator gets repaired and it produces energy
                        if (Input.GetKeyDown("e"))
                        {
                            //Fix generator by having cables
                            if (GameManager.Instance.cablePicked == false && GameManager.Instance.generatorOn == false)
                            {
                                GameObject Clone2 = Instantiate(GenNoCable, new Vector3(0, -316, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, 3);
                            }
                            
                            if (GameManager.Instance.cablePicked == true && GameManager.Instance.generatorOn == false)
                            {
                                CreateEnergy(2000);
                                //Display text on screen
                                GameObject Clone2 = Instantiate(GenRestored, new Vector3(0, -316, 0), transform.rotation);
                                Clone2.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform, false);
                                Destroy(Clone2, 3);
                                GameManager.Instance.generatorOn = true;
                            }
                        }
                    }
                }
                //Check if you interact with light switch
                if (hit.transform.gameObject.name == "Light Switch1" || hit.transform.gameObject.name == "Light Switch2" || hit.transform.gameObject.name == "Light Switch3" || hit.transform.gameObject.name == "Light Switch4" || hit.transform.gameObject.name == "Light Switch5")
                {
                    //Highlight the object yellow
                    var selectionRenderer = selection.GetComponent<Renderer>();
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
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the washing machine turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.WMachineOn == false)
                            {
                                ReduceEnergy(400);
                                GameManager.Instance.WMachineOn = true;
                            }
                        }
                    }
                }
                //Check if you interact with a paper
                if (hit.transform.gameObject.name == "PaperGen")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
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
                            }
                        }
                    }
                }
                //Check if you interact with a TV
                if (hit.transform.gameObject.name == "TV")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the TV is turned on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.tvOn == false)
                            {
                                ReduceEnergy(100);
                                GameManager.Instance.tvOn = true;
                            }
                            else if (GameManager.Instance.tvOn == true)
                            {
                                GameManager.Instance.tvOn = false;
                            }
                        }
                    }
                }
                //Check if you interact with a Radio
                if (hit.transform.gameObject.name == "Radio")
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        //Highlight the object yellow
                        selectionRenderer.material = highlightMaterial;
                        //When E is pressed the radio turns on/off
                        if (Input.GetKeyDown("e"))
                        {
                            if (GameManager.Instance.radioOn == false)
                            {
                                ReduceEnergy(10);
                                GameManager.Instance.radioOn = true;
                            }
                            else if (GameManager.Instance.radioOn == true)
                            {
                                GameManager.Instance.radioOn = false;
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