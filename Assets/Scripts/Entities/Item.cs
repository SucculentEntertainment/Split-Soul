using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private string _type;
    private int _amount;

    public string type {
        get { return _type; }
        set { _type = value; }
    }

    public int amount
    {
        get { return _amount; }
        set { _amount = value; }
    }

    public Item(string type, int amount = 1)
	{
        this.type = type;
        this.amount = amount;
	}
}
