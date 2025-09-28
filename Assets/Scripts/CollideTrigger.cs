using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class CollideTrigger : MonoBehaviour, IQuestTrigger
{
    [SerializeField]
    [Tooltip("The method(s) to call when the player collides with this trigger.")]
    private UnityEvent<string> _onTriggered;
    public UnityEvent<string> OnTriggered { get => _onTriggered; }

    [SerializeField]
    private string questID = "No ID Set";

    private bool sent = false;


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
            QuestManager.Instance.AddQuest(questID, this);
        }
        else
        {
            Debug.LogError("No QuestManager instance exists to subscribe to this trigger.");
        }

    }


    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        //Debug.Log($"{other.gameObject.name} triggered {gameObject.name}.");

        if (!sent && other.gameObject.tag.Equals("Interactor"))
        {
            Debug.Log($"CollideTrigger (ID: {questID}) collided with player. Invoking TriggerEvent.");
            OnTriggered.Invoke(questID);
            sent = true;
        }
    }




}
