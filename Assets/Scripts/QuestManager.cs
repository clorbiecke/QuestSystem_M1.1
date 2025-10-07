using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    //TODO: create class for QuestID? ensure that ids are unique?
    private static QuestManager instance;

    private Dictionary<string, QuestData> quests = new ();

    private UnityEvent<string> _questStarted;
    public UnityEvent<string> QuestStarted { get => _questStarted; set => _questStarted = value; }

    private UnityEvent<string> _questUpdated;
    public UnityEvent<string> QuestUpdated { get => _questUpdated; set => _questUpdated = value; } 

    private UnityEvent<string> _questComplete;
    public UnityEvent<string> QuestComplete { get => _questComplete; set => _questComplete = value; }



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

    public static QuestManager Instance
    {
        get => instance;
    }
    public static QuestManager GetInstance()
    {
        return instance;
    }

    public UnityEvent<string> GetQuestStartedEvent()
    {
        return QuestStarted;
    }

    public UnityEvent<string> GetQuestUpdatedEvent()
    {
        return QuestUpdated;
    }

    public UnityEvent<string> GetQuestCompleteEvent()
    {
        return QuestComplete;
    }

    // Add quest to dictionary, if questID is unique
    public void RegisterQuest(string questID, QuestData data)
    {
        if (quests.TryAdd(questID, data))
        {
            Debug.Log($"Quest \'{questID}\' added to quest manager");
        }
        else
        {
            Debug.LogError($"Quest (questID: {questID}) could not be added to quest manager on {gameObject.name}.");
        }
    }

    // Get quest data by questID
    public QuestData GetQuest(string questID)
    {
        QuestData data = null;

        if (!quests.TryGetValue(questID, out data))
        {
            Debug.LogError($"Could not get data for Quest ID ({questID}) in QuestManager of {gameObject.name}.");
        }
        
        return data;
    }

    // Add QuestManager as an observer to given IQuestTrigger's event
    public void ObserveTrigger(IQuestTrigger trigger)
    {
        trigger.QuestTriggerEvent.AddListener(OnQuestTrigger);
    }


    protected void OnQuestTrigger(string questID, int step, bool jumpToEnd)
    {
        QuestData questData = null;
        if (!quests.TryGetValue(questID, out questData))
        {
            Debug.LogWarning($"Could not get data for questID {questID}");
            return;
        }
        //if (questData == null)
        //{
        //    Debug.LogWarning($"Could not get data for questID {questID}");
        //}
        if (!jumpToEnd)
        {
            if ((!questData.IsStarted() && step == 0) || (!questData.IsComplete() && questData.GetCurrentStep() == step - 1))
            {
                questData.NextStep(false);
                if (step == 0) { QuestStarted.Invoke(questID); }
                QuestUpdated.Invoke(questID);
                if (questData.IsComplete()) { QuestComplete.Invoke(questID); }
            }
        }
        else if (questData.IsStarted())
        {
            questData.NextStep(true);
        }
    }



    //// Called when a quest is completed
    //protected void OnQuestCompleted(string questID)
    //{
    //    try
    //    {
    //        quests[questID] = true;
    //        Debug.Log($"Quest \'{questID}\' complete!" +
    //            $"\nProgress: {GetCompletedQuestCount()}/{GetQuestCount()} quests completed.");
    //        if (AllQuestsCompleted())
    //        {
    //            OnAllQuestsComplete();
    //        }
    //    }
    //    catch (KeyNotFoundException) { }
    //}

    //private bool AllQuestsCompleted()
    //{
    //    foreach (string questID in quests.Keys)
    //    {
    //        if (quests[questID] == false)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //private void OnAllQuestsComplete()
    //{
    //    Debug.Log("YOU HAVE COMPLETED ALL OF THE QUESTS!!!");
    //}

    //public int GetQuestCount()
    //{
    //    return quests.Count;
    //}

    //public int GetCompletedQuestCount()
    //{
    //    int count = 0;
    //    foreach (string questID in quests.Keys)
    //    {
    //        if (quests[questID] == true)
    //        {
    //            count++;
    //        }
    //    }
    //    return count;
    //}



}


