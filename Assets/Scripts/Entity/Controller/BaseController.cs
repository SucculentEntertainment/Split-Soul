using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [HideInInspector] public BaseEntity parent;

    public void init(BaseEntity parent) { this.parent = parent; }
}
