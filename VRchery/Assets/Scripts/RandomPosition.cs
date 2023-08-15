using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public Transform referenceObject;
    private Vector3 currentPosition;

    // Start is called before the first frame update
    void Start()
    {

        currentPosition = referenceObject.position;
        StartCoroutine(RePositionWithDelay());
    }

    IEnumerator RePositionWithDelay()
    {
        while (true)
        {
            SetRandomPosition();
            yield return new WaitForSeconds(5);
        }
    }

    void SetRandomPosition()
    {
        float x = Random.Range(-5.0f, 5.0f);
        float z = Random.Range(-5.0f, 5.0f);
        Debug.Log("x,z: " + x.ToString("F2") + ", " + z.ToString("F2"));
        transform.position = new Vector3(currentPosition.x + x, 0.0f, currentPosition.z + z);
    }
}
