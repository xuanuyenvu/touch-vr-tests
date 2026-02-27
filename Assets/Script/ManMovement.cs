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
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
    }
}
