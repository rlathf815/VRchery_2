using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadArrow : MonoBehaviour
{
    public GameObject activePrefab; // Prefab of the object you want to spawn
    public GameObject activeArrow; // Prefab of the object you want to spawn

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnCollisionEnter is called when the Collider other enters the trigger.
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the specific object you want to detect
        // Replace "SpecificObjectTag" with the actual tag of the specific object
        if (collision.collider.CompareTag("ArrowHand"))
        {
            // Disable the destroyPrefab
            collision.collider.gameObject.SetActive(false);

            // Enable the arrowPrefab
            //activeArrow.SetActive(true);
            activePrefab.SetActive(true);

        }
    }
}




