using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform centerOfMass; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.05f; 
        rb.angularDrag = 0.05f; 
        
        rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
    }
    void FixedUpdate()
    {
        
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {   
        rb.isKinematic = true;
    }
}
