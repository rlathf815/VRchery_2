using UnityEngine;

public class RemoveObject : MonoBehaviour
{
    private bool hasBeenKilled = false;
    private float disableTimer = 2.0f;

    private void Update()
    {
        if (!hasBeenKilled && gameObject.CompareTag("animal"))
        {
            Animator animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("killed"))
            {
                hasBeenKilled = true;
                StartCoroutine(DisableAfterDelay());
            }
        }
    }

    private System.Collections.IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(disableTimer);
        gameObject.SetActive(false);
    }
}