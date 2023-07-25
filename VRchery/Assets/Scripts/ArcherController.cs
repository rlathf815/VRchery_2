using UnityEngine;

public class ArcherController : MonoBehaviour
{
    public GameObject ArrowPrefab;                  // The Arrow to shoot
    public float MaximalShootRange = 100f;          // Maximal distance to shoot
    public float MinimalShootRange = 4f;            // Minimal distance to shoot
    [Range(0,10)]
    public float SpreadFactor = 0.5f;               // Accuracy
    [Range(0f,0.4f)]
    public float SpreadFactorDistanceImpact = 0.1f; // Impact of the distance (from shooter to target) on the accuracy
    public float HeightMultiplier = 2f;             // Changes the parabola-height of the flightpath (Arrows fly in a higher arc)
    public float ArrowFlightSpeed = 6f;             // Speed of the Arrow
    public float ArrowLifeTime = 120f;              // Time until the Arrow gets destroyed (in seconds)  
    [Space]
    public GameObject Target;
    public bool UseTarget;

    private float rKeyHoldTime = 0f;
    private float minFlightSpeed = 4f;
    private float maxFlightSpeed = 8f;
   // private float minShootRange = 5f;
   // private float maxShootRange = 50f;

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
        if (Input.GetKey(KeyCode.R))
        {
            // Increase the hold time
            rKeyHoldTime += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            // Calculate modifiedFlightSpeed based on rKeyHoldTime
            float modifiedFlightSpeed = Mathf.Lerp(minFlightSpeed, maxFlightSpeed, rKeyHoldTime);

            // Calculate modifiedShootRange based on rKeyHoldTime
            float modifiedShootRange = Mathf.Lerp(MinimalShootRange, MaximalShootRange, rKeyHoldTime);

            // Shoot a single Arrow
            TryToShoot(modifiedShootRange, modifiedFlightSpeed);

            // Reset the hold time
            rKeyHoldTime = 0f;
        }
    }

    private void TryToShoot(float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        if (UseTarget && Target != null)
        {
            ShootArrow(Target.transform.position, modifiedMaxShootRange, modifiedFlightSpeed);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Don't let the ray collide with objects tagged 'Projectile'
                if (!hit.collider.CompareTag("Projectile"))
                {
                    ShootArrow(hit.point,modifiedMaxShootRange, modifiedFlightSpeed);
                }
            }
            else
            {
                // If no collider was hit, shoot in the direction of the ray up to the MaximalShootRange
                Vector3 targetPos = ray.GetPoint(MaximalShootRange);
                ShootArrow(targetPos, modifiedMaxShootRange, modifiedFlightSpeed);
            }
        }
    }



    private void ShootArrow(Vector3 targetPos, float modifiedMaxShootRange, float modifiedFlightSpeed)
    {
        // Calculate the distance to the target position
        float distance = Vector3.Distance(transform.position, targetPos);

        // If the target is too far, adjust it to the maximal shoot range
        if (distance > modifiedMaxShootRange)
        {
            // Calculate the direction vector to the target
            Vector3 direction = (targetPos - transform.position).normalized;

            // Calculate the position of the maximal shoot range in the direction of the target
            targetPos = transform.position + direction * modifiedMaxShootRange;

            // Update the distance to the target position
            distance = modifiedMaxShootRange;
        }

        // Calculate the spread-range relative to the distance
        float spreadFactorByDistance = SpreadFactor * (1f + (SpreadFactorDistanceImpact * distance));

        // Calculate inaccurate target (somewhere around the target position)
        Vector3 inaccurateTarget = (Random.insideUnitSphere * spreadFactorByDistance) + targetPos;

        // Create a new Arrow
        var Arrow = Instantiate(ArrowPrefab, transform.position, transform.rotation);

        // Name the Arrow "Arrow" to remove the default name with "(Clone)"
        Arrow.name = "Arrow";

        // Tell the Arrow to go shwoooosh
        //Arrow.GetComponent<ArrowController>().Shoot(inaccurateTarget, gameObject, ArrowFlightSpeed, HeightMultiplier, ArrowLifeTime);
               Arrow.GetComponent<ArrowController>().Shoot(inaccurateTarget, gameObject, modifiedFlightSpeed, HeightMultiplier, ArrowLifeTime);

    }


}