using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadArrow : MonoBehaviour
{
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
    private void OnTriggerEnter(Collider other)
    {
        // Replace "ArrowHand" with the actual tag of the specific object
        if (other.CompareTag("ArrowHand"))
        {
            // Disable the trigger object
            other.gameObject.SetActive(false);

            // Enable the arrowPrefab
            activeArrow.SetActive(true);
        }
    }
}




