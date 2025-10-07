using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class QuestUIManager : MonoBehaviour
{
    // Prefab for making quest entries
    [SerializeField]
    private GameObject QuestEntryPrefab;

    // List of active QuestUIEntires
    private readonly Dictionary<string, QuestUIComponent> QuestUIEntries = new();

    // Canvas group component for fading
    private CanvasGroup fadeGroup = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        // Get reference to CanvasGroup
        if (!TryGetComponent(out fadeGroup))
        {
            Debug.LogWarning("WARNING: No Canvas Group for QuestUIManager");
        }
        else
        {
            fadeGroup.alpha = 0.0f;
        }

        // Disable any existing quest entry children
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out QuestUIComponent component))
            {
                child.gameObject.SetActive(false);
            }
        }

        // Observe the manager events
        QuestManager manager = QuestManager.GetInstance();
        manager.QuestStarted.AddListener(OnQuestStarted);
        manager.QuestComplete.AddListener(OnQuestComplete);

        // Make sure the prefab is set
        if(QuestEntryPrefab == null)
        {
            Debug.LogWarning("WARNING: Prefab missing in QuestUIManager");
        }
    }

    protected void OnQuestStarted (string questId)
    {
        if (!QuestUIEntries.ContainsKey(questId))
        {
            // Create a new entry as a child
            GameObject newEntry = Instantiate(QuestEntryPrefab, transform);
            if (newEntry.TryGetComponent(out QuestUIComponent questText))
            {
                // Initialize the UI entry
                questText.Initialize(questId);
                StartCoroutine(questText.FadeIn(0.667f));
                UpdateQuestPanel(QuestUIEntries.Count + 1);
                QuestUIEntries.Add(questId, questText);
            }
        }
    }

    protected void UpdateQuestPanel (int count)
    {
        // Adjust height
        float newHeight = count * (95 + 10) + 10;
        newHeight = MathF.Max(newHeight, 115.0f);
        
        // Grow/Shrink if needed
        StartCoroutine(DoGrowShrink(newHeight, 0.667f));

        // Fade in if needed
        if (count > 0 && fadeGroup.alpha < 1.0)
        {
            StartCoroutine(DoFade(1.0f, 0.667f));
        }
    }

    protected void OnQuestComplete (string questId)
    {
        // Do the removal in a coroutine so we can delay it
        StartCoroutine(RemoveQuestDelayed(questId));
    }

    protected IEnumerator RemoveQuestDelayed(string questId)
    {
        // Try and retrieve the proper UI element for this quest
        if (QuestUIEntries.TryGetValue(questId, out QuestUIComponent questText))
        {
            // Wait for 3 seconds
            yield return new WaitForSeconds(3.0f);

            // If this is the last entry, fade before we remove it
            if (QuestUIEntries.Count == 1)
            {
                yield return DoFade(0.0f, 0.667f);
            }
            else
            {
                yield return questText.FadeOut(0.667f);
            }

            // Destroy and remove this quest entry
            Destroy(questText.gameObject);
            QuestUIEntries.Remove(questId);
            UpdateQuestPanel(QuestUIEntries.Count);
        }
    }

    protected IEnumerator DoFade(float endAlpha, float duration)
    {
        // Slowly change the alpha
        float startAlpha = fadeGroup.alpha;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }

        // Ensure final alpha is reached
        fadeGroup.alpha = endAlpha;
    }

    protected IEnumerator DoGrowShrink(float endHeight, float duration)
    {
        // Try and get the rect transform
        if (!TryGetComponent(out RectTransform rectTrans))
        {
            Debug.Log("WRANING: Failed to get rectTransform for UI Quest List");
            yield break;
        }

        // Loop to change the size
        float startHeight = rectTrans.sizeDelta.y;
        float timer = 0f;
        Vector2 size;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            size = rectTrans.sizeDelta;
            size.y = Mathf.Lerp(startHeight, endHeight, timer / duration);
            rectTrans.sizeDelta = size;

            yield return null;
        }

        // Ensure final size is reached
        size = rectTrans.sizeDelta;
        size.y = endHeight;
        rectTrans.sizeDelta = size;
    }
}
