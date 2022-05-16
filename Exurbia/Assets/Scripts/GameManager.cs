using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool cablePicked;
    public bool lightOn;
    public bool WMachineOn;
    public bool paperRead;
    private void Awake()
    {
        Instance = this;
    }

}
