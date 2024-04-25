using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHolder : MonoBehaviour
{
    public Portal[] portals;

    private void Awake()
    {
        portals = GetComponentsInChildren<Portal>();
    }
}
