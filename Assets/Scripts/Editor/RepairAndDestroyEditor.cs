using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DestroyAndRepearSys))]
public class RepairAndDestroyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DestroyAndRepearSys destroyAndRepairSys = (DestroyAndRepearSys)target;
        if (GUILayout.Button("Destroy a Sys"))
        {
            destroyAndRepairSys.BreakSomething();
        }


    }
}
