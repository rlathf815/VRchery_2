using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class M_ArcherController : MonoBehaviour
{
    public GameObject ArrowPrefab;                  // �߻�� ȭ�� ������
    public float MaximalShootRange = 100f;          // �ִ� �����Ÿ�
    public float MinimalShootRange = 4f;            // �ּ� �����Ÿ�
    [Range(0, 10)]
    public float SpreadFactor = 0.5f;               // ��Ȯ��
    [Range(0f, 0.4f)]
    public float SpreadFactorDistanceImpact = 0.1f; // �Ÿ��� ���� ��Ȯ�� �������� factor
    public float HeightMultiplier = 2f;             // ���������� ȭ���� �������� ������
    public float ArrowFlightSpeed = 6f;             // ȭ�� �ӵ�
    public float ArrowLifeTime = 120f;              // ȭ�� ����� ������ �ð�
    [Space]
    public bool UseTarget;


    private float rKeyHoldTime = 0f;
    private float minFlightSpeed = 4f;
    private float maxFlightSpeed = 8f;
    // private float minShootRange = 5f;
    // private float maxShootRange = 50f;
    private InputDevice leftController;           //�޼� ��Ʈ�ѷ� input device ���� ����
    private InputDevice rightController;           //������ ��Ʈ�ѷ� input device ���� ����

    private bool wasGripPressed;

    public Vector3 rotationOffsetEuler; // ȸ�� offset��. ��Ȯ�� ������ �� ������ ����
    public Vector3 positionOffset; // ��ġ offset��. ��Ȯ�� ������ �� ������ ����
    public GameObject TargetObject; // ȭ�� ���� ������ ��Ÿ���� ���� ������Ʈ
    public Animator bowAnimator; // Ȱ �ִϸ����� ��Ʈ�ѷ�
    public float DistanceThreshold = 0.2f;
    public GameObject BowstringObject;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get the direction the player is looking at
            Vector3 targetDirection = Camera.main.transform.forward;

            // Create a new arrow at the Archer's position
            var arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);

            // Fire the arrow in the direction the player is looking at
            arrow.GetComponent<ArrowController>().Shoot(targetDirection, gameObject, ArrowFlightSpeed, HeightMultiplier, ArrowLifeTime);
        }
    }



    private void TryToShoot(float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        Vector3 direction = TargetObject.transform.forward; // Ÿ�� ������Ʈ�� ����

        Vector3 targetPos = transform.position + direction * modifiedMaxShootRange;

        ShootArrow(direction, modifiedMaxShootRange, modifiedFlightSpeed);
    }




    private void ShootArrow(Vector3 direction, float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        Quaternion rotationOffset = Quaternion.Euler(rotationOffsetEuler);
        var Arrow = Instantiate(ArrowPrefab, transform.position + positionOffset, Quaternion.LookRotation(direction) * rotationOffset);
        Debug.Log("instantiated");
        Arrow.name = "Arrow";
        Arrow.GetComponent<ArrowController>().Shoot(direction, gameObject, modifiedFlightSpeed, HeightMultiplier, ArrowLifeTime);


    }

}