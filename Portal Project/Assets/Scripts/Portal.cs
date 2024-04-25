using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField] public Portal otherPortal;
    
    public bool isPlaced = true;
    public Renderer Renderer { get; private set; }
    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        Renderer.enabled = otherPortal.isPlaced;
    }
}
