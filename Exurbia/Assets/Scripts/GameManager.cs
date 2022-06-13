using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool cablePicked;
    public bool lightOn;
    public bool WMachineOn = false;
    public bool paperRead;
    public bool tvOn = false;
    public bool generatorOn = false;
    public bool radioOn = false;
    public bool microwaveOn = false;
    public bool laptopOn = false;
    public bool fridgeOn = false;
    public bool breakerBoxOn = false;
    public bool carOn = false;
    public bool radioTowerOn = false;
    public bool paperGenRead = false;
    public bool paperCarRead = false;
    public bool paperFridgeRead = false;
    public bool paperLaptopRead = false;
    public bool paperMicrowaveRead = false;
    public bool paperRadioRead = false;
    public bool paperTVRead = false;
    public bool paperWMachineRead = false;
    [SerializeField] private GameObject Player;

    void Start()
    {

    }
    private void Awake()
    {
        Instance = this;
    }

}
