using RootMotion.FinalIK;
using RootMotion.Demos;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private GameObject agent;
    private ManMovement manMovement;
    private PushAnimation pushAnimation;
    private LookAtIK lookAtIK;
    private InteractionSystem interactionSystem;
    private InteractionSystemTestGUI interactionSystemTestGUI;

    private Vector3 originalPosition;
    private Vector3 originalRotation;

    public enum Mode
    {
        Animation_and_FBBIK,
        FBBIK_and_InteractionSystem
    }

    public enum TouchType
    {
        Punch,
        HandPush,
        FingerPush
    }

    [SerializeField] private Mode mode;
    [SerializeField] private TouchType touchType;



    [ContextMenu("Reset Transform")]
    public void ResetTransform()
    {
        agent.transform.position = originalPosition;
        agent.transform.eulerAngles = originalRotation;
    }

    void Awake()
    {
        agent = GameObject.FindGameObjectWithTag("Agent");

        manMovement = agent.GetComponent<ManMovement>();
        pushAnimation = agent.GetComponent<PushAnimation>();
        lookAtIK = agent.GetComponent<LookAtIK>();

        interactionSystem = agent.GetComponent<InteractionSystem>();
        interactionSystemTestGUI = agent.GetComponent<InteractionSystemTestGUI>();

        originalPosition = agent.transform.position;
        originalRotation = agent.transform.eulerAngles;
    }


    void Start()
    {
        HandleCaseChange();
    }

    private void OnValidate()
    {
        HandleCaseChange();
    }

    public void Move()
    {

    }

    private void HandleCaseChange()
    {
        if (manMovement == null || pushAnimation == null || lookAtIK == null)
            return;

        switch (mode)
        {
            case Mode.Animation_and_FBBIK:
                manMovement.enabled = true;
                pushAnimation.enabled = true;
                lookAtIK.enabled = true;

                interactionSystem.enabled = false;
                interactionSystemTestGUI.enabled = false;

                break;

            case Mode.FBBIK_and_InteractionSystem:
                manMovement.enabled = false;
                pushAnimation.enabled = false;
                lookAtIK.enabled = false;

                interactionSystem.enabled = true;
                interactionSystemTestGUI.enabled = true;
                break;
        }
    }
}