using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogSystem))]
public class DialogueEditor : Editor
{
    private SerializedProperty dialogEventsProperty;
    private void OnEnable()
    {
        dialogEventsProperty = serializedObject.FindProperty("dialogTEST"); // Asegúrate de que el nombre coincida
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        DialogSystem dialogSys = (DialogSystem)target;

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(dialogEventsProperty, new GUIContent("Test dialogue"), true);
        serializedObject.ApplyModifiedProperties();
        
        if (GUILayout.Button("TEST"))
        {
            dialogSys.TestEvent();
        }

        GUILayout.EndHorizontal();
    }
}
