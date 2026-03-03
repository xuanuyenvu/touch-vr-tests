using UnityEngine;
using RootMotion.FinalIK;
using Unity.XR.CoreUtils;

public class PushAnimation : MonoBehaviour
{
    private FullBodyBipedIK fbbik;
    private Vector3 originalPosition;
    [SerializeField] private AnimationCurve pushCurve;
    [SerializeField] private AnimationCurve returnCurve;
    [SerializeField] private AnimationCurve blendCurve;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private bool isPushing;
    [SerializeField] private float blendSpeed;
    private GameObject player;
    private LookAtIK lookAtIK;

    private Animator animator;
    private float t = 0f;

    private enum PushState
    {
        Idle,
        Pushing,
        Returning
    }

    private PushState currentState = PushState.Idle;
    public bool useTarget;

    void Start()
    {
        fbbik = GetComponent<FullBodyBipedIK>();
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        lookAtIK = GetComponent<LookAtIK>();
    }

    public bool isResetting;

    void Update()
    {
        // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        // {
        //     PunchAnimation();
        // }
        if(isResetting)
        {
            ResetPosition();
            isResetting = false;
        }
    }

    // void LateUpdate()
    // {
    //     if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Punch")) return;

    //     float finalWeight = 0f;
    //     if (isPushing)
    //     {
    //         currentState = PushState.Pushing;
    //         isPushing = false; 
    //     }
    //     switch (currentState)
    //     {
    //         case PushState.Pushing:
    //             t += Time.deltaTime * blendSpeed;
    //             finalWeight = pushCurve.Evaluate(t);

    //             if (t >= 1f)
    //             {
    //                 t = 1f;
    //                 currentState = PushState.Returning; 
    //             }
    //             break;

    //         case PushState.Returning:
    //             t -= Time.deltaTime * blendSpeed;
    //             finalWeight = returnCurve.Evaluate(1 - t);

    //             if (t <= 0f)
    //             {
    //                 t = 0f;
    //                 currentState = PushState.Idle; 
    //             }
    //             break;
    //     }

    //     fbbik.solver.rightHandEffector.positionWeight = finalWeight;
    //     fbbik.solver.rightHandEffector.target = rightHandTarget;
    // }

    // private void PunchAnimation()
    // {
    //     Debug.Log("Punch animation triggered");
    //     fbbik.solver.rightHandEffector.positionWeight = 1f;
    //     fbbik.solver.rightHandEffector.target = rightHandTarget;
    // }

    private void ResetPosition()
    {
        transform.position = originalPosition;
    }

    void LateUpdate()
    {
        lookAtIK.solver.Update();
        lookAtIK.solver.IKPosition = player.transform.position;
        if(!useTarget) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("punch"))
        {
            // RotateManTowardsTarget();
            // aimIK.enabled = true;
            // LookAtTargetSmooth(rightHandTarget, 5f);
            float weight = blendCurve.Evaluate(state.normalizedTime * 0.8f); 
            fbbik.solver.rightHandEffector.positionWeight = weight;
            // fbbik.solver.rightHandEffector.rotationWeight = weight;
            fbbik.solver.rightHandEffector.target = rightHandTarget;
        }
        else
        {
            // aimIK.enabled = false;
            // transform.LookAt(player.transform);
            fbbik.solver.rightHandEffector.positionWeight = 0f;
            fbbik.solver.rightHandEffector.rotationWeight = 0f;
        }
    }



    void LookAtTargetSmooth(Transform target, float rotateSpeed)
    {
        Debug.Log("Looking");
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        Quaternion lookRot = Quaternion.LookRotation(dir);

        // 🔥 Tạo offset 90 độ quanh trục Y
        Quaternion offset = Quaternion.Euler(0f, 90f, 0f);

        Quaternion targetRot = lookRot * offset;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotateSpeed
        );
    }
}
