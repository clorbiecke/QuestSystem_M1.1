using UnityEditor;
using UnityEngine;
using System.Linq;


[CustomEditor(typeof(QuestCollideTrigger))]
public class QuestTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        QuestCollideTrigger myScript = (QuestCollideTrigger)target;

        // Draw default inspector elements
        DrawDefaultInspector();

        // Create the dropdown using EditorGUILayout.Popup
        if (myScript.availableOptions != null && myScript.availableOptions.Count > 0)
        {
            string[] optionsArray = myScript.availableOptions.ToArray();
            myScript.selectedOptionIndex = EditorGUILayout.Popup("Select Option", myScript.selectedOptionIndex, optionsArray);
        }
    }
}
