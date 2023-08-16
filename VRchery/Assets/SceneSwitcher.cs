using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string forestSceneName = "Forest";
    public GameObject circle;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered) 
        {
            isTriggered = true;
            StartCoroutine(SwitchToForestScene());
        }
    }

    IEnumerator SwitchToForestScene()
    {
        circle.SetActive(true);
        yield return new WaitForSeconds(5); 
        SceneManager.LoadScene(forestSceneName);
    }
}
