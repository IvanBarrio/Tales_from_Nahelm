using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected string iName;
    protected string iType;

    public string getName()
    {
        return iName;
    }

    public void setName(string name)
    {
        this.iName = name;
    }

    public string getiType()
    {
        return iType;
    }
}
