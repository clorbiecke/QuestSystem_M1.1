using UnityEngine;
using UnityEngine.Events;

public interface IQuestTrigger
{
    public UnityEvent<string> OnTriggered { get; }

}
