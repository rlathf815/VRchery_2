using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }
        public ThirdPersonCharacter character { get; private set; }
        public Transform target;
        public Animator animator;

        private bool rotated = false;
        private bool isWalking = false; // Track walking state

        private void Start()
        {
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

            agent.updateRotation = false;
            agent.updatePosition = true;
        }

        private void Update()
        {
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                if (!isWalking) // Check if walking state changed
                {
                    isWalking = true;
                    animator.SetBool("walked", true);
                }

                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                if (isWalking) // Check if walking state changed
                {
                    isWalking = false;
                    animator.SetBool("walked", false);
                }

                character.Move(Vector3.zero, false, false);
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        void kill()
        {
            target = null;
            if (!rotated)
            {
                // Rotate the capsule collider's GameObject by 90 degrees to the left
                //transform.Rotate(Vector3.right, -90f);
                //transform.localScale *= 0.5f;
                rotated = true; // Set to true to prevent repeated rotations
            }
            animator.SetTrigger("killed");
        }
    }
}



