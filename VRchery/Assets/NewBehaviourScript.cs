using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private bool rotated = false;
    private BoxCollider collider; // ����Ƽ �ν����Ϳ��� �ݶ��̴��� �Ҵ��մϴ�.
    public float newSizeMultiplier = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void turn()
    {
        //if (!rotated)
        //{
        // Rotate the capsule collider's GameObject by 90 degrees to the left
        //transform.localScale *= 0.5f;
        //rotated = true; // Set to true to prevent repeated rotations
        //}

        transform.Rotate(Vector3.right, -90f);

    }
}
