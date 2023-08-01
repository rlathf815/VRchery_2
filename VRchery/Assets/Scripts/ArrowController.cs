using UnityEngine;

public class ArrowController : MonoBehaviour
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
    // Gravity
    private float gravity = -9.81f;

    private void Awake()
    {
        // Initialize the Arrow
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
        // Make the Arrow destroy itself in x(LifeTime) seconds
        Destroy(gameObject, lifeTime);
    }


    private void FixedUpdate()
    {
        if (isInitialized)
        {
            // Reenable the renderers
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

            // Update the velocity with the force of gravity
            velocity.y += gravity * Time.deltaTime;

            // Move the Arrow along the Parabola
            transform.position += velocity * Time.deltaTime;

            // The direction the Arrow is currently flying
            Vector3 direction = transform.position - lastPosition;

            // Collision detection with raycast
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(lastPosition, direction);
            // Only collide with objects that are not tagged with "Projectile" and not the owner
            if (Physics.Raycast(ray, out hit, direction.magnitude, ArrowSO.CollisionLayerMask, QueryTriggerInteraction.Ignore))
            {
                // Here you might want to add more things the Arrow should ignore
                if (!hit.collider.CompareTag("Projectile") && hit.collider != ownerCollider)
                {
                    Arrive(hit);
                    return;
                }
            }

            // Rotate the arrow
            transform.rotation = Quaternion.LookRotation(direction);

            // Update the lastPosition
            lastPosition = transform.position;
        }
    }

    /// <summary>
    /// Shoot the Arrow
    /// </summary>
    /// <param name="target">The target the Arrow should be shot at</param>
    /// <param name="owner">The GameObject shooting the Arrow</param>
    /// <param name="flightSpeed">The speed of the Arrow</param>
    /// <param name="heightMultiplier">The parabola-hight of the flightpath</param>
    /// <param name="lifeTime">Time until the Arrow gets destroyed</param>
    public void Shoot(Vector3 direction, GameObject owner, float flightSpeed, float heightMultiplier, float lifeTime)
    {
        direction.y = 0;
        this.velocity = direction * flightSpeed;
        ownerCollider = owner.GetComponent<Collider>();
        this.flightSpeed = flightSpeed;
        this.heightMultiplier = heightMultiplier;
        this.lifeTime = lifeTime;

        // begin the arrow flight on the next FixedUpdate-step
        isInitialized = true;
    }


    private void Arrive(RaycastHit hit)
    {
        // Mark the Arrow as arrived
        readyToFly = false;

        // Stop emmitting the trail when stuck (so stuck arrows moving with its parent i.e. the enemy, do not emmit a trail)
        // This is done with a short delay to avoid unwated artifacts
        Invoke("DisableTrailEmission", ArrowSO.DisableTrailEmissionTime);
        // Make the trail fade out fast and then disable it
        trailRenderer.time = ArrowSO.TrailFadeoutTime;
        Invoke("DisableTrail", ArrowSO.DisableTrailTime);

        // 랜덤한 깊이로 박히도록
        transform.position = hit.point += transform.forward * Random.Range(ArrowSO.StuckDepthMin, ArrowSO.StuckDepthMax);

        // 충돌한 오브젝트의 하위로 들어가도록 함
        MakeChildOfHitObject(hit.transform);
    }

    private void DisableTrailEmission()
    {
        trailRenderer.emitting = false;
    }

    private void DisableTrail()
    {
        trailRenderer.enabled = false;
    }

    private void MakeChildOfHitObject(Transform parentTransform)
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