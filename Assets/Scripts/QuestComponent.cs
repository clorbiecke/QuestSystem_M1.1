using UnityEngine;

public class QuestComponent : MonoBehaviour
{
    [SerializeField]
    [Tooltip("QuestData object that describes a quest.")]
    private QuestData quest = null;


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
