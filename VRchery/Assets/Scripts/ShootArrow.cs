using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public Rigidbody arrowPrefab;  
    public float forceMultiplier = 1f;  
    public Vector3 shootDirection;  
    public Transform startPoint; 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
           
            Quaternion arrowRotation = Quaternion.LookRotation(shootDirection);
            Rigidbody currentArrow = Instantiate(arrowPrefab, startPoint.position, arrowRotation);          
            Shoot(currentArrow, shootDirection);
        }
    }

    private void Shoot(Rigidbody arrowRigidbody, Vector3 direction)
    {
        
        Vector3 normalizedDirection = direction.normalized;
        float forceMagnitude = direction.magnitude * forceMultiplier;      
        Vector3 force = (normalizedDirection + new Vector3(0, 1, 0)) * forceMagnitude;      
        arrowRigidbody.AddForce(force, ForceMode.Impulse);
    }
}
