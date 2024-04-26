using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Portal pToWarp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setPortal(Portal portal)
    {
        pToWarp = portal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Portal"))
        { 
            ContactPoint contactPoint = collision.contacts[0];
            pToWarp.transform.position = contactPoint.point + (contactPoint.normal * 0.01f);
            pToWarp.transform.forward = -contactPoint.normal;
            //pToWarp.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
        }
        Destroy(gameObject);
    }
}
