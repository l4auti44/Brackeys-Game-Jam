using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DestroyAndRepearSys))]
public class RepairAndDestroyEditor : Editor
{

    private DestroyAndRepearSys.Modules selectedModule;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DestroyAndRepearSys destroyAndRepairSys = (DestroyAndRepearSys)target;


        if (GUILayout.Button("Destroy a random Sys"))
        {
            destroyAndRepairSys.BreakSomething();
        }

        GUILayout.BeginHorizontal();


        selectedModule = (DestroyAndRepearSys.Modules)EditorGUILayout.EnumPopup("Select Module", selectedModule);


        if (GUILayout.Button("Destroy Sys"))
        {
            destroyAndRepairSys.TestBreakSomething(selectedModule);
        }

        GUILayout.EndHorizontal();
    }
}

