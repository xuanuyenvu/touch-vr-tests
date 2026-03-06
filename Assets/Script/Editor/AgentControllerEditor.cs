using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AgentController))]
public class AgentControllerEditor : Editor
{
    SerializedProperty mode;
    SerializedProperty touchType;
    GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);

    private void Start()
    {
        headerStyle.fontSize = 12;
        headerStyle.alignment = TextAnchor.MiddleLeft;
    }

    private void OnEnable()
    {
        mode = serializedObject.FindProperty("mode");
        touchType = serializedObject.FindProperty("touchType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Interaction Settings", headerStyle);
        EditorGUILayout.Space(2);

        EditorGUILayout.PropertyField(mode);
        EditorGUILayout.Space(2);

        if ((AgentController.Mode)mode.enumValueIndex == AgentController.Mode.Animation_and_FBBIK)
        {
            touchType.enumValueIndex = (int)AgentController.TouchType.Punch;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(touchType);
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            string[] options = { "HandPush", "FingerPush" };

            int selected = touchType.enumValueIndex - 1;
            if (selected < 0) selected = 0;

            selected = EditorGUILayout.Popup("Touch Type", selected, options);

            touchType.enumValueIndex = selected + 1;
        }

        EditorGUILayout.Space(6);
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Agent Actions", headerStyle);
        EditorGUILayout.Space(2);

        AgentController manager = (AgentController)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Move To User"))
        {
            manager.Move();
        }
        if (GUILayout.Button("Reset Transform"))
        {
            manager.ResetTransform();
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}