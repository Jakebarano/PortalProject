using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHolder : MonoBehaviour
{
    public Portal[] Portals;

    private void Awake()
    {
        Portals = GetComponentsInChildren<Portal>();
    }
}
