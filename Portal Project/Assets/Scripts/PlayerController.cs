using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pewpew;
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -2, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 2, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * 2;
        } else
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = transform.forward * -2;
        } else
        {
            rb.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Rigidbody pew = Instantiate(pewpew, transform.position + (transform.forward * 2), transform.rotation).GetComponent<Rigidbody>();
            pew.velocity = rb.velocity = transform.forward * 3;
        }
    }
}