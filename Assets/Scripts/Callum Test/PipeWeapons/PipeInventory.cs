using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeInventory
{
   
    // Start is called before the first frame update
    public string name;
}

[System.Serializable]
public class Weapon : PipeInventory
{
    public int durability;
    public int value;
    public Sprite icon;

    // Constructor to set up name and durability
    public Weapon(string name, int durability, int value)
    {
        this.name = name;
        this.durability = durability;
        this.value = value;
        this.icon = icon;
    }
}
