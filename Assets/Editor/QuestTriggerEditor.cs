using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
//using Codice.Client.Common.WebApi.Responses;

[InitializeOnLoad]
[CustomEditor(typeof(QuestCollideTrigger))]
public class QuestTriggerEditor : Editor
{
    //private static QuestComponent[] availableOptions;
    private static HashSet<string> availableOptions;
    private static string[] optionsArray;
    private static string[] emptyOption = { "" };

    static QuestTriggerEditor()
    {
        EditorApplication.hierarchyChanged += refreshAvailableOptions;
        Undo.undoRedoPerformed += refreshAvailableOptions;
        //refreshAvailableOptions();
    }
    private void OnEnable()
    {

        refreshAvailableOptions();
    }
    public override void OnInspectorGUI()
    {
        QuestCollideTrigger myScript = (QuestCollideTrigger)target;

        // Draw default inspector elements
        DrawDefaultInspector();

        // Create the dropdown using EditorGUILayout.Popup
        if (availableOptions != null && availableOptions.Count > 0)
        {
            //string[] optionsArray = availableOptions.ToArray();
            myScript.selectedQuestIndex = EditorGUILayout.Popup("Select Quest", myScript.selectedQuestIndex, optionsArray);
            //myScript.testID = optionsArray[myScript.selectedQuestIndex];
            if (myScript.selectedQuestIndex != 0)
            {
                myScript.QuestID = optionsArray[myScript.selectedQuestIndex];
                myScript.selectedQuestIndex = 0;
            }
        }
    }
  
    static void refreshAvailableOptions()
    {
        QuestComponent[] components = FindObjectsByType<QuestComponent>(FindObjectsSortMode.None);
        availableOptions = new HashSet<string>();
        optionsArray = (string[])emptyOption.Clone();
        if (components == null) {
            //QuestTracker.AllQuestIDs = new string[0];
            return; 
        }
        foreach (QuestComponent component in components)
        {
            availableOptions.Add(component.ReadOnlyQuestData().GetID());
        }
        optionsArray = optionsArray.Concat(availableOptions.ToArray()).ToArray();
        //optionsArray.Concat(availableOptions.ToArray<string>());
        //EditorUtility.SetDirty(availableOptions);
    }
}

//class QuestTracker : MonoBehaviour
//{
//    public static string[] AllQuestIDs = { };
//}
