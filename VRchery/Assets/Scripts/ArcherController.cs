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
        // If the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Create a new arrow at the Archer's position
                var Arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);

                // Shoot the arrow towards the point that was clicked
                Arrow.GetComponent<ArrowController>().Shoot(hit.point, gameObject, ArrowFlightSpeed, HeightMultiplier, ArrowLifeTime);
            }
        }
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