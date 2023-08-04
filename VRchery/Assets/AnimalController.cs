using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    public Animator animator;
    public float wanderRadius;
    public float wanderTimer;
    public GameObject t;
    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    // Start is called before the first frame update
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();

        NavMeshHit hit;

        if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError("This GameObject is not placed on a NavMesh");
        }

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(t.transform.position);
    }

    
    void kill()
    {
        animator.SetTrigger("killed");
    }
}
