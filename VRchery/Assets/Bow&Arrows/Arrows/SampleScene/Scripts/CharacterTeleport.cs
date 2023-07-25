using UnityEngine;

public class CharacterTeleport : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Shoot a ray from the camera to the cursor to get the position of the cursor 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Teleport the player to the cursor
                gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
            }
        }
    }
}
