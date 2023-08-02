using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class GripChecker : MonoBehaviour
{
    ActionBasedController controller;
    public GameObject targetObject; // Reference to the specific object you want to check
    public GameObject enableObject;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            if (targetObject.activeSelf)
            {
                // Assuming you have a reference to the controller
                // Replace 'ControllerType' with the actual type of your controller script.

                if (controller.activateAction.action.ReadValue<float>() < 0.3f)
                {
                    // Deactivate the object if it is active and the condition is met.
                    targetObject.SetActive(false);
                }

                //enableObject.SetActive(true);
            }
        }
    }
}