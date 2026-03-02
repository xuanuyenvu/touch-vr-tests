using UnityEngine;
using RootMotion.FinalIK;

public class PushAnimation : MonoBehaviour
{
    private FullBodyBipedIK fbbik;
    [SerializeField] private AnimationCurve pushCurve;
    [SerializeField] private AnimationCurve returnCurve;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private bool isPushing;
    [SerializeField] private float blendSpeed;
    private float t = 0f;

    private enum PushState
    {
        Idle,
        Pushing,
        Returning
    }

    private PushState currentState = PushState.Idle;

    void Start()
    {
        fbbik = GetComponent<FullBodyBipedIK>();
    }

    void Update()
    {
        float finalWeight = 0f;
        if (isPushing)
        {
            currentState = PushState.Pushing;
            isPushing = false; 
        }
        switch (currentState)
        {
            case PushState.Pushing:
                t += Time.deltaTime * blendSpeed;
                finalWeight = pushCurve.Evaluate(t);

                if (t >= 1f)
                {
                    t = 1f;
                    currentState = PushState.Returning; 
                }
                break;

            case PushState.Returning:
                t -= Time.deltaTime * blendSpeed;
                finalWeight = returnCurve.Evaluate(1 - t);

                if (t <= 0f)
                {
                    t = 0f;
                    currentState = PushState.Idle; 
                }
                break;
        }

        fbbik.solver.rightHandEffector.positionWeight = finalWeight;
        fbbik.solver.rightHandEffector.target = rightHandTarget;
    }
}
