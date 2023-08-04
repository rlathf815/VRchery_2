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
    // �߷�
    private float gravity = -9.81f;
    public PlayerData playerData;

    private void Awake()
    {
        // �ʱ�ȭ
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
        // lifeTime ���� �� �����
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

            // �߷����� �ӵ��� ������Ʈ
            velocity.y += gravity * Time.deltaTime;

            // �������� ���� ȭ���� �̵�
            transform.position += velocity * Time.deltaTime;

            // ȭ���� ���� ���� �ִ� ����
            Vector3 direction = transform.position - lastPosition;

            // ����ĳ��Ʈ�� ���� �浹 ����
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(lastPosition, direction);
            if (Physics.Raycast(ray, out hit, direction.magnitude, ArrowSO.CollisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                // ���⿡ ȭ��ǥ�� �����ؾ� �� �׸��� �� �߰� ����
                if (!hit.collider.CompareTag("Arrow") && hit.collider != ownerCollider) //ȭ�쳢���� �浹 x ->������������
                {
                    Arrive(hit);
                    return;
                }
            }

            // ȭ�� ȸ��
            transform.rotation = Quaternion.LookRotation(direction);

            // lastPosition ������Ʈ
            lastPosition = transform.position;
        }
    }

    /// <param name="target">ȭ���� ���� �ϴ� ���</param>
    /// <param name="owner">ȭ���� ��� ���� ������Ʈ</param>
    /// <param name="flightSpeed">ȭ���� �ӵ�</param>
    /// <param name="heightMultiplier">���� ����� ������ ����</param>
    /// <param name="lifeTime">ȭ���� �ı��� �������� �ð�</param>
    public void Shoot(Vector3 targetPos, GameObject shooter, float flightSpeed, float heightMultiplier, float lifeTime)
    {
        Vector3 direction = (targetPos - shooter.transform.position).normalized;

        direction.y = 0;
        this.velocity = direction * flightSpeed;
        ownerCollider = shooter.GetComponent<Collider>();
        this.flightSpeed = flightSpeed;
        this.heightMultiplier = heightMultiplier;
        this.lifeTime = lifeTime;

        // ���� FixedUpdate �ܰ迡�� ȭ�� ���� ����
        isInitialized = true;
    }


    private void Arrive(RaycastHit hit)
    {
        // ȭ���� �������� ǥ��
        readyToFly = false;

        // ���� �� trail ����(Ư�� ������Ʈ�� ������ ȭ���� ���̻� trail x )
        Invoke("DisableTrailEmission", ArrowSO.DisableTrailEmissionTime);
        // ���� ���̵�ƿ�
        trailRenderer.time = ArrowSO.TrailFadeoutTime;
        Invoke("DisableTrail", ArrowSO.DisableTrailTime);

        // ������ ���̷� ��������
        transform.position = hit.point += transform.forward * Random.Range(ArrowSO.StuckDepthMin, ArrowSO.StuckDepthMax);

        // �浹�� ������Ʈ�� ������ ������ ��
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

    private void MakeChildOfHitObject(Transform parentTransform) //ȭ�� ���� �� ������ ��ü�� ����(�ڽ�) ������Ʈ�� �����
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