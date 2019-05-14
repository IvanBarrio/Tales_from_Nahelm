using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string iName;

    public Item()
    { }

    public Item(string name)
    {
        this.iName = name;
    }

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.iName = name;
    }
}
