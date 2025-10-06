using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class QuestCollideTrigger : MonoBehaviour, IQuestTrigger
{
    [SerializeField]
    [Tooltip("The method(s) to call when the player collides with this trigger.")]
    private UnityEvent<string, int, bool> _questTriggerEvent;
    public UnityEvent<string, int, bool> QuestTriggerEvent { get => _questTriggerEvent; }


    [SerializeField]
    [Tooltip("Go to this step of the quest when triggered.")]
    private int questTriggerStep = 0;

    [SerializeField]
    [Tooltip("If true, completes entire quest when activated, rather than going to the next step.")]
    private bool jumpToEnd = false;

    [SerializeField]
    [Tooltip("The quest ID this will act as a trigger for.")]
    private string questID = "not set";

    private bool triggerSent = false;


    void Awake() 
    {
    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QuestManager manager = QuestManager.Instance;
        //Debug.Log(manager);
        if (manager != null)
        {
            QuestManager.Instance.ObserveTrigger(this);
        }
        else
        {
            Debug.LogError("No QuestManager instance exists to subscribe to this trigger.");
        }

    }


    private bool CanTrigger()
    {
        QuestManager mgr = QuestManager.Instance;
        QuestData quest = mgr.GetQuest(questID);

        if (quest != null)
        {
            if ((jumpToEnd && quest.IsStarted) || (quest.GetCurrentStep() == questTriggerStep -1))
            {
                return true;
            }
        }
        return false;
    }


    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        //Debug.Log($"{other.gameObject.name} triggered {gameObject.name}.");

        if (!triggerSent && CanTrigger() && other.gameObject.CompareTag("PlayerHands"))
        {
            Debug.Log($"CollideTrigger (ID: {questID}) collided with player. Invoking TriggerEvent.");
            QuestTriggerEvent.Invoke(questID, questTriggerStep, jumpToEnd);
            triggerSent = true;
        }
    }



}
