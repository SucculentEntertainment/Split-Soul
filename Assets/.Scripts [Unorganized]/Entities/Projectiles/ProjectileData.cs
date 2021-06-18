using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    private string  _name;
    private string  _element;
    private float   _damage;

    public string   name        { get { return _name;       }   set { _name     = value;  }}
    public string   element     { get { return _element;    }   set { _element  = value;  }}
    public float    damage      { get { return _damage;     }   set { _damage   = value;  }}     

    public ProjectileData(string name, string element, float damage)
    {
        _name = name;
        _element = element;
        _damage = damage;
    }
};
