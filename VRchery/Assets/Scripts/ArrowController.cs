using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Trail")]
    public TrailRenderer trailRenderer; 

    public ArrowSO ArrowSO;            
    public GameObject MeshParent;      

    private Vector3 target;            
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

    private void Awake()
    {
        // Initialize the Arrow
        target = Vector3.zero;
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

        // Calculate the distance to the target
        targetDistance = Vector3.Distance(transform.position, target);

        // Calculate the flight-speed relative to the distance
        speedToDistance = flightSpeed / targetDistance * flightSpeed;
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

        if (readyToFly && target != Vector3.zero)
        {
            flightTimer += Time.deltaTime;

            // Move the Arrow along the Parabola
            transform.position = MathParabola.Parabola(startPoint, target, (targetDistance / 5f) * heightMultiplier, flightTimer * speedToDistance);

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
    public void Shoot(Vector3 target, GameObject owner, float flightSpeed, float heightMultiplier, float lifeTime)
    {
        this.target = target;
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

        // Make the arrow stuck at a random depth
        // Modify these values to change how deep into the object the arrows gets stuck (0 is the arrow-tip)
        transform.position = hit.point += transform.forward * Random.Range(ArrowSO.StuckDepthMin, ArrowSO.StuckDepthMax);

        // Make the Arrow a child of the object it has hit (this causes the stuck arrow to move with its parent)
        MakeChildOfHitObject(hit.transform);
    }

    // Disable the arrow trail emission
    private void DisableTrailEmission()
    {
        trailRenderer.emitting = false;
    }

    // Disable the arrow trail
    private void DisableTrail()
    {
        trailRenderer.enabled = false;
    }

    private void MakeChildOfHitObject(Transform parentTransform)
    {
        // Only make the arrow a child if the object is suited to 'become a parent'
        // For more info this see the documentation
        if (IsSuitedParent(parentTransform))
        {
            Quaternion originalRotation = transform.rotation;

            // Reset the rotation of the arrow to get rid of mesh-deformation when parenting
            transform.rotation = new Quaternion();

            // Set the parent and keep the world position
            transform.SetParent(parentTransform, true);

            // Rotate the mesh and the trail
            MeshParent.transform.rotation = originalRotation;
        }
    }

    // Check whether the transform is suited to become the parent of the arrow
    private bool IsSuitedParent(Transform parent)
    {
        if (IsUniformScaled(parent) || IsUniformRotated(parent))
            return true;
        else
        {
            //When the parent is non uniform scaled and rotated giving it a child will result in wierd mesh-deformation of the arrow
            return false;
        }
    }

    private bool IsUniformScaled(Transform parent)
    {
        // When x,y and z are all the same it means the scale is uniform
        if (parent.localScale.x == parent.localScale.y && parent.localScale.x == parent.localScale.z)
            return true;
        else
            return false;
    }

    private bool IsUniformRotated(Transform parent)
    {
        var rotation = parent.rotation.eulerAngles;

        // When x,y and z are all the same it means the rotation is uniform
        if (parent.rotation.x == parent.rotation.y && parent.rotation.x == parent.rotation.z)
            return true;
        else
        {
            // When each axis is a multiple of 90° it is uniform aswell (or atleast suitable)
            if (Mathf.Round(rotation.x) % 90f == 0 && Mathf.Round(rotation.y) % 90f == 0 && Mathf.Round(rotation.z) % 90f == 0)
                return true;
            else
                return false;
        }
    }
}