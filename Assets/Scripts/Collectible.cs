using UnityEngine;

[RequireComponent (typeof(QuestCollideTrigger))]
public class Collectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<QuestCollideTrigger>().QuestTriggerEvent.AddListener(OnCollected);
    }

    private void OnCollected(string questID, int questTriggerStep, bool jumpToEnd)
    {
        gameObject.SetActive(false);
    }




}
