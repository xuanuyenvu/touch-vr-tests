using UnityEngine;
using UnityEngine.AI;

public class ManMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isMovingToTarget;

    private Animator animator;
    private GameObject player;
    private NavMeshAgent agent;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isMovingToTarget)
        {
            agent.SetDestination(player.transform.position);
            animator.SetFloat("speed", agent.velocity.magnitude);

            if (HasReachedDestination())
            {
                Debug.Log("Reached the player!");
                isMovingToTarget = false;
                animator.SetBool("isPunch", true);
            }
        }
        else
        {
            animator.SetFloat("speed", 0);
        }

        if (animator.GetBool("isPunch") &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.SetBool("isPunch", false);
        }
    }

    private bool HasReachedDestination()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                return true;
        }

        return false;
    }
}
