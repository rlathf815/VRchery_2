using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ArrowSO", order = 1)]
public class ArrowSO : ScriptableObject
{
    public float DisableTrailEmissionTime = 0.01f;  // Time until the trail emission is disabled when the Arrow arrived (in seconds) [Default = 0.01]
    public float TrailFadeoutTime = 0.4f;           // Trail lifetime left when the Arrow arrived (in seconds) [Default = 0.4]
    public float DisableTrailTime = 0.8f;           // Time until the trail is completely disabled after the Arrow arrived (in seconds) [Default = 0.8]
    [Range(0, 1)]
    public float StuckDepthMin = 0.25f;             // Minimal depth the arrow can get stuck. 0 = the tip of the arrow / not entered. [Default = 0.25]
    [Range(0, 1)]
    public float StuckDepthMax = 0.6f;              // Maximal depth the arrow can get stuck. [Default = 0.6]
    public LayerMask CollisionLayerMask;            // LayerMask for Arrow collisions
}