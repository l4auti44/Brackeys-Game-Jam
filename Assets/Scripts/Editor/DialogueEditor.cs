using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogSystem))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogSystem dialogSys = (DialogSystem)target;
        if (GUILayout.Button("Test the dialogTEST selected event"))
        {
            dialogSys.TestEvent();
        }
    }
}
