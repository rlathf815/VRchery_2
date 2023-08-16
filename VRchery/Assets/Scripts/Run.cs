using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Run : MonoBehaviour
{
    private InputDevice rightController;

    // Reference to the ContinuousMoveProvider that you want to modify
    public ActionBasedContinuousMoveProvider continuousMoveProvider;

    public AudioClip soundClip; // Reference to your sound clip
    private AudioSource audioSource;
    private bool isPlayingSound = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        rightController = devices[0];

        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool AButton))
        {
            if (AButton)
            {
                Debug.Log("Pressed");

                // Change the move speed of the ContinuousMoveProvider to 5 when AButton is pressed.
                if (continuousMoveProvider != null)
                {
                    continuousMoveProvider.moveSpeed = 5.0f;
                }

                if (!isPlayingSound)
                {
                    // Play the sound if it's not already playing
                    isPlayingSound = true;
                    audioSource.loop = true;
                    audioSource.clip = soundClip;
                    audioSource.Play();
                }
            }
            else
            {
                // Reset the move speed to its default value when AButton is released.
                if (continuousMoveProvider != null)
                {
                    continuousMoveProvider.moveSpeed = 1.0f; // Change this to your default move speed.
                }

                isPlayingSound = false;
                audioSource.loop = false; // Disable looping
                audioSource.Stop();
            }
        }
    }
}