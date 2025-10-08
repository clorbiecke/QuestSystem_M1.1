using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestComponent : MonoBehaviour
{


    [SerializeField]
    [Tooltip("QuestData object that describes a quest.")]
    private QuestData quest = null;

    public ref QuestData ReadOnlyQuestData() 
    {
        return ref quest;
    }

//#if UNITY_EDITOR
//    // Refrences to all QuestData objects
//    public static Dictionary<QuestComponent, string> allQuests = new();
//    private void OnValidate()
//    {
//        // add quest to all quests
//        allQuests[this] = quest.GetID();
//    }
//#endif


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuestManager manager = QuestManager.Instance;
        if (manager != null )
        {
            manager.RegisterQuest(quest.GetID(), quest);
            quest.ResetQuest();
        }
        else
        {
            Debug.LogError("QuestManager.Instance is null!");
        }
        
    }




}
