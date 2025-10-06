using UnityEngine;
using UnityEngine.Events;

public interface IQuestTrigger
{
    public UnityEvent<string, int, bool> QuestTriggerEvent { get; }

}
