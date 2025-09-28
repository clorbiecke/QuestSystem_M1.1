using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;

    private Dictionary<string, bool> quests = new ();
    public static QuestManager Instance
    {
        get => instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log($"QuestManager instance created:\n\t{instance}");
        }
        else
        {
            Debug.LogError($"A QuestManager instance already exists on {instance.gameObject.name}. " +
                $"The instance on {gameObject.name} will not work.");
        }
    }



    // Add new quest to quest manager. Returns true if successful, otherwise returns false.
    public bool AddQuest(string questID, IQuestTrigger questTrigger)
    {
        if (quests.TryAdd(questID, false))
        {
            Debug.Log($"Quest \'{questID}\' added to quest manager");
            questTrigger.OnTriggered.AddListener(OnQuestCompleted);
            return true;
        }
        else
        {
            Debug.LogError($"Quest (questID: {questID}) could not be added to quest manager on {gameObject.name}.");
            return false;
        }
    }

    // Get quest information by questID
    public bool GetQuestInfo(string questID)
    {
        bool info;

        if (quests.TryGetValue(questID, out info))
        {
            return info;
        }
        else
        {
            Debug.LogError($"Quest ID ({questID}) was not found in QuestManager of {gameObject.name}.");
            return false;
        }
    }

    // Called when a quest is completed
    protected void OnQuestCompleted(string questID)
    {
        try
        {
            quests[questID] = true;
            Debug.Log($"Quest \'{questID}\' complete!" +
                $"\nProgress: {GetCompletedQuestCount()}/{GetQuestCount()} quests completed.");
            if (AllQuestsCompleted())
            {
                OnAllQuestsComplete();
            }
        }
        catch (KeyNotFoundException) { }
    }

    // Subscribe
    public void SubscribeToTrigger(IQuestTrigger trigger)
    {
        trigger.OnTriggered.AddListener(this.OnQuestCompleted);
    }

    private bool AllQuestsCompleted()
    {
        foreach (string questID in quests.Keys)
        {
            if (quests[questID] == false)
            {
                return false;
            }
        }
        return true;
    }

    private void OnAllQuestsComplete()
    {
        Debug.Log("YOU HAVE COMPLETED ALL OF THE QUESTS!!!");
    }

    public int GetQuestCount()
    {
        return quests.Count;
    }

    public int GetCompletedQuestCount()
    {
        int count = 0;
        foreach (string questID in quests.Keys)
        {
            if (quests[questID] == true)
            {
                count++;
            }
        }
        return count;
    }



}


