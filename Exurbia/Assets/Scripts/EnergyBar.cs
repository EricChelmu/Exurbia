using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEnergy(int energy, int energyMax)
    {
        slider.value = energy;
        slider.maxValue = energyMax;
    }
    
    public void SetEnergy(int energy)
    {
        slider.value = energy;
    }
}
