using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform centerOfMass;  // Drag your new GameObject here
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.05f; // Linear drag
        rb.angularDrag = 0.05f; // Angular drag
        // Change the center of mass to the local position of the new GameObject
        rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
    }
    void FixedUpdate()
    {
        // Make the arrow point in the direction it's travelling
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Make the arrow stick when it hits a collider
        rb.isKinematic = true;
    }
}
