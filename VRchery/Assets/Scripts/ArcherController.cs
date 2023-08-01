using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ArcherController : MonoBehaviour
{
    public GameObject ArrowPrefab;                  // 발사될 화살 프리팹
    public float MaximalShootRange = 100f;          // 최대 사정거리
    public float MinimalShootRange = 4f;            // 최소 사정거리
    [Range(0,10)]
    public float SpreadFactor = 0.5f;               // 정확도
    [Range(0f,0.4f)]
    public float SpreadFactorDistanceImpact = 0.1f; // 거리에 따라 정확도 낮아지는 factor
    public float HeightMultiplier = 2f;             // 높아질수록 화살의 포물선이 높아짐
    public float ArrowFlightSpeed = 6f;             // 화살 속도
    public float ArrowLifeTime = 120f;              // 화살 사라질 때까지 시간
    [Space]
    public GameObject Target;
    public bool UseTarget;


    private float rKeyHoldTime = 0f;
    private float minFlightSpeed = 4f;
    private float maxFlightSpeed = 8f;
    // private float minShootRange = 5f;
    // private float maxShootRange = 50f;
    private InputDevice targetDevice;           //컨트롤러 input device 정보 저장
    private bool wasGripPressed;

    public Vector3 rotationOffsetEuler; // 회전 offset값. 정확히 앞으로 잘 나가게 조정
    //private void FixedUpdate()
    //{
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        // Shoot an Arrow every fixed tick while the key is pressed
    //        TryToShoot();
    //    }
    //}

    private void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            Quaternion rotationOffset = Quaternion.Euler(rotationOffsetEuler); // Use the offset from the public field

            InputDevice leftController = devices[0];
            leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion deviceRotation);
            leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePosition);

            deviceRotation *= rotationOffset;  // apply the rotation offset
            Vector3 direction = deviceRotation * Vector3.forward;

            Vector3 rayOrigin = devicePosition;
            Vector3 rayDirection = deviceRotation * Vector3.forward;
            Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.red);  // Multiplied by 10 to make it visible in the scene view

            leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue);

            if (gripValue)
            {
                // Increase the hold time
                rKeyHoldTime += Time.deltaTime;
            }
            else if (wasGripPressed)
            {
                // Calculate modifiedFlightSpeed based on rKeyHoldTime
                //float modifiedFlightSpeed = Mathf.Lerp(minFlightSpeed, maxFlightSpeed, rKeyHoldTime);
                float modifiedFlightSpeed = 20f;

                // Calculate modifiedShootRange based on rKeyHoldTime
                float modifiedShootRange = Mathf.Lerp(MinimalShootRange, MaximalShootRange, rKeyHoldTime);

                // Shoot a single Arrow
                TryToShoot(modifiedShootRange, modifiedFlightSpeed);

                // Reset the hold time
                rKeyHoldTime = 0f;
            }

            // Update wasGripPressed to the current state of the grip button
            wasGripPressed = gripValue;
        }
    }



    private void TryToShoot(float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            InputDevice leftController = devices[0];
            leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion deviceRotation);
            Vector3 direction = deviceRotation * Vector3.forward; // this gives us the direction in which the controller is pointing

            // Calculate the target position based on the direction in which the arrow should be shot
            Vector3 targetPos = transform.position + direction * modifiedMaxShootRange;

            ShootArrow(direction, modifiedMaxShootRange, modifiedFlightSpeed);
        }
    }





    private void ShootArrow(Vector3 direction, float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        // Calculate the distance to the target position
      //  float distance = Vector3.Distance(transform.position, targetPos);
      //
      //  // If the target is too far, adjust it to the maximal shoot range
      //  if (distance > modifiedMaxShootRange)
      //  {
      //      // Calculate the direction vector to the target
      //      Vector3 direction = (targetPos - transform.position).normalized;
      //
      //      // Calculate the position of the maximal shoot range in the direction of the target
      //      targetPos = transform.position + direction * modifiedMaxShootRange;
      //
      //      // Update the distance to the target position
      //      distance = modifiedMaxShootRange;
      //  }
      //
      //  // Calculate the spread-range relative to the distance
      //  float spreadFactorByDistance = SpreadFactor * (1f + (SpreadFactorDistanceImpact * distance));
      //
      //  // Calculate inaccurate target (somewhere around the target position)
      //  Vector3 inaccurateTarget = (Random.insideUnitSphere * spreadFactorByDistance) + targetPos;

        // Create a new Arrow
        var Arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.LookRotation(direction));

        // Name the Arrow "Arrow" to remove the default name with "(Clone)"
        Arrow.name = "Arrow";

        // Tell the Arrow to go shwoooosh
        //Arrow.GetComponent<ArrowController>().Shoot(inaccurateTarget, gameObject, ArrowFlightSpeed, HeightMultiplier, ArrowLifeTime);
        //Arrow.GetComponent<ArrowController>().Shoot(inaccurateTarget, gameObject, modifiedFlightSpeed, HeightMultiplier, ArrowLifeTime);
        Arrow.GetComponent<ArrowController>().Shoot(direction, gameObject, modifiedFlightSpeed, HeightMultiplier, ArrowLifeTime);

    }


}