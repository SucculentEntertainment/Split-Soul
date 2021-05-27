using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable
{
    private string _id;
    private int _amount;

    public string id {
        get { return _id; }
        set { _id = value; }
    }

    public int amount
    {
        get { return _amount; }
        set { _amount = value; }
    }

    public Collectable(string id, int amount = 1)
	{
        this.id = id;
        this.amount = amount;
	}
}
