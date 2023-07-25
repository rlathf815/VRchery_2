using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public Rigidbody arrowPrefab;  // Prefab of your arrow with a Rigidbody and Collider attached
    public float forceMultiplier = 1f;  // Multiplier to adjust the force applied
    public Vector3 shootDirection;  // Direction to shoot the arrows
    public Transform startPoint;  // Starting point of the arrow

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Create a new arrow at the position of the startPoint game object
            Quaternion arrowRotation = Quaternion.LookRotation(shootDirection);
            Rigidbody currentArrow = Instantiate(arrowPrefab, startPoint.position, arrowRotation);

            // Then shoot the instantiated arrow
            Shoot(currentArrow, shootDirection);
        }
    }

    private void Shoot(Rigidbody arrowRigidbody, Vector3 direction)
    {
        // Ensure the direction vector is normalized to only get the direction
        Vector3 normalizedDirection = direction.normalized;

        // Calculate the force to be applied
        float forceMagnitude = direction.magnitude * forceMultiplier;

        // Calculate the initial force vector with an upward component
        Vector3 force = (normalizedDirection + new Vector3(0, 1, 0)) * forceMagnitude;

        // Apply the force to the arrow
        arrowRigidbody.AddForce(force, ForceMode.Impulse);
    }
}
