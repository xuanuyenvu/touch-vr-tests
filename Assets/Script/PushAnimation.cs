using UnityEngine;
using RootMotion.FinalIK;
using Unity.XR.CoreUtils;

public class PushAnimation : MonoBehaviour
{
    private FullBodyBipedIK fbbik;
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

    public bool useTarget;
    public bool useRotation;

    void Start()
    {
        fbbik = GetComponent<FullBodyBipedIK>();
        animator = GetComponent<Animator>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        lookAtIK = GetComponent<LookAtIK>();
    }

    public bool isResetting;
    public float offsetCurve;

    void LateUpdate()
    {
        lookAtIK.solver.Update();
        lookAtIK.solver.IKPosition = player.transform.position;
        if(!useTarget) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("punch"))
        {
            if(useRotation)
            {
                LookAtTargetSmooth(rightHandTarget, 5f);
            }
            float weight = blendCurve.Evaluate(state.normalizedTime * offsetCurve); 
            fbbik.solver.rightHandEffector.positionWeight = weight;
            fbbik.solver.rightHandEffector.target = rightHandTarget;
        }
        else
        {
            if(state.IsName("walking"))
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }
            fbbik.solver.rightHandEffector.positionWeight = 0f;
            fbbik.solver.rightHandEffector.rotationWeight = 0f;
        }
    }



    void LookAtTargetSmooth(Transform target, float rotateSpeed)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        Quaternion lookRot = Quaternion.LookRotation(dir);

        Quaternion offset = Quaternion.Euler(0f, -5f, 0f);

        Quaternion targetRot = lookRot * offset;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotateSpeed
        );
    }
}
