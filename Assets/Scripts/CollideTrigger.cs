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
        //OnTriggered.AddListener(QuestManager.Instance.ON)
        QuestManager.Instance.SubscribeToTrigger(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!sent && collision.gameObject.tag.ToLower().Equals("player"))
        {
            Debug.Log($"CollideTrigger (ID: {questID}) collided with player. Invoking TriggerEvent.");
            OnTriggered.Invoke(questID);
        }
    }




}
