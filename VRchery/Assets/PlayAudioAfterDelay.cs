using System.Collections;
using UnityEngine;

public class PlayAudioAfterDelay : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1; 
    public AudioClip clip2; 
    public float delay = 2.0f;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        StartCoroutine(PlayAudioClipsInSequence());
    }

    IEnumerator PlayAudioClipsInSequence()
    {
        yield return new WaitForSeconds(delay);

        audioSource.clip = clip1;
        audioSource.Play();
        yield return new WaitForSeconds(clip1.length);
        yield return new WaitForSeconds(1.0f);

        audioSource.clip = clip2;
        audioSource.Play();
    }
}
