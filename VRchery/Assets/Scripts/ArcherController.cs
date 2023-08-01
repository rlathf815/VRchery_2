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
    public Vector3 positionOffset; // 위치 offset값. 정확히 앞으로 잘 나가게 조정
    public GameObject TargetObject; // 화살 비행 방향을 나타내는 게임 오브젝트


    private void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            Quaternion rotationOffset = Quaternion.Euler(rotationOffsetEuler);

            InputDevice leftController = devices[0];
            leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion deviceRotation);
            leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePosition);

            deviceRotation *= rotationOffset;  // 회전 오프셋 적용
            Vector3 direction = deviceRotation * Vector3.forward;

            Vector3 rayOrigin = devicePosition;
            Vector3 rayDirection = deviceRotation * Vector3.forward;
            Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.red);  

            leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue);

            if (gripValue)
            {
                // hold time 증가
                rKeyHoldTime += Time.deltaTime;
            }
            else if (wasGripPressed)
            {
                // 수정된 비행 속도를 rKeyHoldTime에 따라 계산
                //float modifiedFlightSpeed = Mathf.Lerp(minFlightSpeed, maxFlightSpeed, rKeyHoldTime);
                float modifiedFlightSpeed = ArrowFlightSpeed; //일단 테스트용으로 스피드 에디터에서 지정

                // Calculate modifiedShootRange based on rKeyHoldTime
                float modifiedShootRange = Mathf.Lerp(MinimalShootRange, MaximalShootRange, rKeyHoldTime);

                // Shoot
                TryToShoot(modifiedShootRange, modifiedFlightSpeed);

                
                rKeyHoldTime = 0f;
            }

            // 그립 버튼의 현재 상태로 업데이트
            wasGripPressed = gripValue;
        }
    }



    private void TryToShoot(float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        Vector3 direction = TargetObject.transform.forward; // tell the direction the target object is pointing

        // Calculate the target position based on the direction the arrow should be fired
        Vector3 targetPos = transform.position + direction * modifiedMaxShootRange;

        ShootArrow(direction, modifiedMaxShootRange, modifiedFlightSpeed);
    }




    private void ShootArrow(Vector3 direction, float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        Quaternion rotationOffset = Quaternion.Euler(rotationOffsetEuler);
        var Arrow = Instantiate(ArrowPrefab, transform.position + positionOffset, Quaternion.LookRotation(direction) * rotationOffset);

        Arrow.name = "Arrow";
        Arrow.GetComponent<ArrowController>().Shoot(direction, gameObject, modifiedFlightSpeed, HeightMultiplier, ArrowLifeTime);
    }

}