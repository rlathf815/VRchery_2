using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class ArcherController : MonoBehaviour
{
    public GameObject ArrowPrefab;                  // 발사될 화살 프리팹
    public float MaximalShootRange = 100f;          // 최대 사정거리
    public float MinimalShootRange = 4f;            // 최소 사정거리
    [Range(0, 10)]
    public float SpreadFactor = 0.5f;               // 정확도
    [Range(0f, 0.4f)]
    public float SpreadFactorDistanceImpact = 0.1f; // 거리에 따라 정확도 낮아지는 factor
    public float HeightMultiplier = 2f;             // 높아질수록 화살의 포물선이 높아짐
    public float ArrowFlightSpeed = 6f;             // 화살 속도
    public float ArrowLifeTime = 120f;              // 화살 사라질 때까지 시간
    [Space]
    public GameObject Target;
    public bool UseTarget;

    public XRBaseController leftCon,rightCon;

    private float rKeyHoldTime = 0f;
    private float minFlightSpeed = 4f;
    private float maxFlightSpeed = 8f;
    // private float minShootRange = 5f;
    // private float maxShootRange = 50f;
    private InputDevice leftController;           //왼손 컨트롤러 input device 정보 저장
    private InputDevice rightController;           //오른손 컨트롤러 input device 정보 저장

    private bool wasGripPressed;

    public Vector3 rotationOffsetEuler; // 회전 offset값. 정확히 앞으로 잘 나가게 조정
    public Vector3 positionOffset; // 위치 offset값. 정확히 앞으로 잘 나가게 조정
    public GameObject TargetObject; // 화살 비행 방향을 나타내는 게임 오브젝트
    public Animator bowAnimator; // 활 애니메이터 컨트롤러
    public float DistanceThreshold = 0.2f;
    public GameObject BowstringObject;
    public GameObject RightHandObject;
    public GameObject RightXR;
    public GameObject HoldArrow;

    private void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();

        // 왼손 컨트롤러
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
        InputDevice leftController = devices[0];
        leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftDevicePosition);

        // 오른손 컨트롤러
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        rightController = devices[0];
        rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightDevicePosition);

        // 컨트롤러 간 거리 측정
        float distance = CalculateDistance(leftDevicePosition, rightDevicePosition);

        rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGripValue);
        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue);

        if (HoldArrow.activeSelf && rightGripValue && triggerValue)
        {
            RightHandObject.transform.SetParent(BowstringObject.transform);
            //HoldArrow.SetActive(true);
            if (distance >= DistanceThreshold)
            {
                // hold 시간을 늘리고 "hold" 애니메이터 파라미터 true
                rKeyHoldTime += Time.deltaTime / 5;
                bowAnimator.SetBool("hold", true);
                if (rKeyHoldTime <= 0.1f)
                {
                    leftCon.SendHapticImpulse(0.1f, 0.5f);
                    rightCon.SendHapticImpulse(0.1f, 0.5f);
                }
                else if (0.1f < rKeyHoldTime && rKeyHoldTime < 0.3f)
                {
                    leftCon.SendHapticImpulse(0.5f, 0.5f);
                    rightCon.SendHapticImpulse(0.5f, 0.5f);
                }
                else
                {
                    leftCon.SendHapticImpulse(0.9f, 0.5f);
                    rightCon.SendHapticImpulse(0.9f, 0.5f);
                }
            }
        }

        else if (HoldArrow.activeSelf && wasGripPressed && (!rightGripValue || !triggerValue)) //전 프레임에선 두 버튼 모두 눌려있음&&현 프레임에선 둘 다 놓아있음
        {
            // "fire" 애니메이터 트리거 설정
            HoldArrow.SetActive(false);
            bowAnimator.SetTrigger("fire");
            bowAnimator.SetBool("hold", false);

            // maximum 속도는 40, minimum 15
            float modifiedFlightSpeed = Mathf.Clamp(rKeyHoldTime * ArrowFlightSpeed, 15f, 40f);

            // 사정거리도 hold 시간에 따라 조정
            float modifiedShootRange = Mathf.Lerp(MinimalShootRange, MaximalShootRange, rKeyHoldTime);

            // Shoot
            TryToShoot(modifiedShootRange, modifiedFlightSpeed);

            rKeyHoldTime = 0f;
            RightHandObject.transform.SetParent(RightXR.transform);
            RightHandObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            RightHandObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
        }

        // 현 프레임 상태로 업데이트
        wasGripPressed = rightGripValue && triggerValue;
    }
    private float CalculateDistance(Vector3 position1, Vector3 position2)
    {
        return Vector3.Distance(position1, position2);
    }


    private void TryToShoot(float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        Vector3 direction = TargetObject.transform.forward; // 타겟 오브젝트의 방향

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