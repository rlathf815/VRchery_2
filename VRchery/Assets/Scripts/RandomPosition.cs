using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public Transform referenceObject;
    private Vector3 originalPosition;
    public float randomRange = 5.0f;
    public float repositionInterval = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (referenceObject == null)
        {
            return;
        }

        originalPosition = referenceObject.position;
        StartCoroutine(RepositionWithDelay());
    }

    IEnumerator RepositionWithDelay()
    {
        while (true)
        {
            SetRandomPosition();
            yield return new WaitForSeconds(repositionInterval);
        }
    }

    void SetRandomPosition()
    {
        float x = Random.Range(-randomRange, randomRange);
        float z = Random.Range(-randomRange, randomRange);
        Vector3 newPosition = originalPosition + new Vector3(x, 0.0f, z);
        transform.position = newPosition;
    }
}