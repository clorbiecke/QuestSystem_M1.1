using UnityEngine;

[RequireComponent (typeof(CollideTrigger))]
public class Collectible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<CollideTrigger>().OnTriggered.AddListener(OnCollected);
    }

    private void OnCollected(string questID)
    {
        gameObject.SetActive(false);
    }




}
