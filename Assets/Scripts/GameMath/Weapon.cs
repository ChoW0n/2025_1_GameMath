using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string name;
    public float stdDev;
    public float critChance;
    public float critMultiplier;

    public Weapon(string name, float stdDev, float critChance, float critMultiplier)
    {
        this.name = name;
        this.stdDev = stdDev;
        this.critChance = critChance;
        this.critMultiplier = critMultiplier;
    }
}
