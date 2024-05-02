using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pewpew;
    Rigidbody rb;

    //Swaps between 0 and 1
    int currentPortal = 0;

    PortalHolder portalHolder;

    void Start()
    {
        portalHolder = GameObject.Find("Portals").GetComponent<PortalHolder>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody pew = Instantiate(pewpew, transform.position + (transform.forward * 2), transform.rotation)
                .GetComponent<Rigidbody>();
            Bullet b = pew.gameObject.GetComponent<Bullet>();
            b.setPortal(portalHolder.portals[currentPortal]);
            pew.velocity = rb.velocity + transform.forward * 3;

            if (currentPortal == 0)
            {
                currentPortal = 1;
            }
            else
            {
                currentPortal = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -2, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 2, 0);
        }

        Vector3 totalVel = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            totalVel += transform.TransformDirection(Vector3.left) * 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVel -= transform.TransformDirection(Vector3.left) * 3;
        }

        if (Input.GetKey(KeyCode.W))
        {
            totalVel += transform.forward * 3;
        } else
        if (Input.GetKey(KeyCode.S))
        {
            totalVel += transform.forward * -3;
        }

        rb.velocity = totalVel;
    }
}