using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ActionBasedController))]
public class ArrowSpawner : MonoBehaviour
{
    ActionBasedController controller;
    public GameObject arrowPrefab; // Prefab of the object you want to spawn
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnCollisionEnter is called when this object collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the specific object you want to detect
        if (collision.collider.CompareTag("Quiver") && controller.activateAction.action.ReadValue<float>() > 0.5f && !arrowPrefab.activeSelf)
        {
            // Enable the arrowPrefab
            arrowPrefab.SetActive(true);
            count++;

            TryLoadEndScene();
        }
    }

    private void TryLoadEndScene()
    {
        if (count > 10)
        {
            SceneManager.LoadScene("End_scene");
        }
    }
}