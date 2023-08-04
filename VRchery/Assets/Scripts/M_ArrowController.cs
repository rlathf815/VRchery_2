using UnityEngine;

public class M_ArrowController : MonoBehaviour
{
    [Header("Trail")]
    public TrailRenderer trailRenderer;

    public ArrowSO ArrowSO;
    public GameObject MeshParent;

    private Vector3 velocity;
    private Collider ownerCollider;
    private float flightSpeed;
    private float heightMultiplier;
    private float lifeTime;

    private float flightTimer;
    private Vector3 startPoint;
    private float targetDistance;
    private float speedToDistance;
    private Vector3 lastPosition;
    private bool readyToFly;
    private bool isInitialized;
    private Vector3 direction;
    // 중력
    private float gravity = -9.81f;
    public PlayerData playerData;

    private void Awake()
    {
        // 초기화
        startPoint = transform.position;
        lastPosition = transform.position;
        readyToFly = false;
        isInitialized = false;

        MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            renderer.enabled = false;
        }
    }

    private void Start()
    {
        // lifeTime 지난 후 사라짐
        Destroy(gameObject, lifeTime);
    }


    private void FixedUpdate()
    {
        if (isInitialized)
        {
            MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in meshRenderers)
            {
                renderer.enabled = true;
            }

            readyToFly = true;
            isInitialized = false;
        }

        if (readyToFly)
        {
            flightTimer += Time.deltaTime;

            // 중력으로 속도를 업데이트
            velocity.y += gravity * Time.deltaTime;

            // 포물선을 따라 화살을 이동
            transform.position += velocity * Time.deltaTime;

            // 화살이 현재 날고 있는 방향
            Vector3 direction = transform.position - lastPosition;

            // 레이캐스트를 통한 충돌 감지
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(lastPosition, direction);
            if (Physics.Raycast(ray, out hit, direction.magnitude, ArrowSO.CollisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                // 여기에 화살표가 무시해야 할 항목을 더 추가 가능
                if (!hit.collider.CompareTag("Arrow") && hit.collider != ownerCollider) //화살끼리는 충돌 x ->겹쳐질수있음
                {
                    Arrive(hit);
                    return;
                }
            }

            // 화살 회전
            transform.rotation = Quaternion.LookRotation(direction);

            // lastPosition 업데이트
            lastPosition = transform.position;
        }
    }

    /// <param name="target">화살을 쏴야 하는 대상</param>
    /// <param name="owner">화살을 쏘는 게임 오브젝트</param>
    /// <param name="flightSpeed">화살의 속도</param>
    /// <param name="heightMultiplier">비행 경로의 포물선 높이</param>
    /// <param name="lifeTime">화살이 파괴될 때까지의 시간</param>
    public void Shoot(Vector3 targetPos, GameObject shooter, float flightSpeed, float heightMultiplier, float lifeTime)
    {
        Vector3 direction = (targetPos - shooter.transform.position).normalized;

        direction.y = 0;
        this.velocity = direction * flightSpeed;
        ownerCollider = shooter.GetComponent<Collider>();
        this.flightSpeed = flightSpeed;
        this.heightMultiplier = heightMultiplier;
        this.lifeTime = lifeTime;

        // 다음 FixedUpdate 단계에서 화살 비행 시작
        isInitialized = true;
    }


    private void Arrive(RaycastHit hit)
    {
        // 화살을 도착으로 표시
        readyToFly = false;

        // 고착 시 trail 중지(특정 오브젝트에 고착된 화살은 더이상 trail x )
        Invoke("DisableTrailEmission", ArrowSO.DisableTrailEmissionTime);
        // 꼬리 페이드아웃
        trailRenderer.time = ArrowSO.TrailFadeoutTime;
        Invoke("DisableTrail", ArrowSO.DisableTrailTime);

        // 랜덤한 깊이로 박히도록
        transform.position = hit.point += transform.forward * Random.Range(ArrowSO.StuckDepthMin, ArrowSO.StuckDepthMax);

        // 충돌한 오브젝트의 하위로 들어가도록 함
        MakeChildOfHitObject(hit.transform);

        if (hit.transform.CompareTag("1p"))
        {
            // Increase score. This assumes you have a reference to the PlayerData instance.
            playerData.score += 1;
        }
        else if (hit.transform.CompareTag("3p"))
        {
            // Increase score. This assumes you have a reference to the PlayerData instance.
            playerData.score += 3;
        }
        else if (hit.transform.CompareTag("5p"))
        {
            // Increase score. This assumes you have a reference to the PlayerData instance.
            playerData.score += 5;
        }
        else if (hit.transform.CompareTag("7p"))
        {
            // Increase score. This assumes you have a reference to the PlayerData instance.
            playerData.score += 7;
        }
        else if (hit.transform.CompareTag("10p"))
        {
            // Increase score. This assumes you have a reference to the PlayerData instance.
            playerData.score += 10;
        }
        Debug.Log("tag : " + hit.transform.tag);
    }

    private void DisableTrailEmission()
    {
        trailRenderer.emitting = false;
    }

    private void DisableTrail()
    {
        trailRenderer.enabled = false;
    }

    private void MakeChildOfHitObject(Transform parentTransform) //화살 도착 시 고착된 물체의 하위(자식) 오브젝트로 만든다
    {

        if (IsSuitedParent(parentTransform))
        {
            Quaternion originalRotation = transform.rotation;

            transform.rotation = new Quaternion();

            transform.SetParent(parentTransform, true);

            MeshParent.transform.rotation = originalRotation;
        }
    }

    private bool IsSuitedParent(Transform parent)
    {
        if (IsUniformScaled(parent) || IsUniformRotated(parent))
            return true;
        else
        {
            return false;
        }
    }

    private bool IsUniformScaled(Transform parent)
    {

        if (parent.localScale.x == parent.localScale.y && parent.localScale.x == parent.localScale.z)
            return true;
        else
            return false;
    }

    private bool IsUniformRotated(Transform parent)
    {
        var rotation = parent.rotation.eulerAngles;

        if (parent.rotation.x == parent.rotation.y && parent.rotation.x == parent.rotation.z)
            return true;
        else
        {

            if (Mathf.Round(rotation.x) % 90f == 0 && Mathf.Round(rotation.y) % 90f == 0 && Mathf.Round(rotation.z) % 90f == 0)
                return true;
            else
                return false;
        }
    }
}