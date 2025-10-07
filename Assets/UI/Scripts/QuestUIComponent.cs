using System.Collections;
using UnityEngine;
using TMPro;

public class QuestUIComponent : MonoBehaviour
{
    // References to the child text objects
    private TMP_Text QuestTitle = null;
    private TMP_Text QuestInstructions = null;

    // The associated quest (be sure to set by calling Initialize() after instantiating)
    private string questId = "NotSet";

    // Retrieve references to the child text objects
    private bool EnsureComponentsSet ()
    {
        if (!transform.GetChild(0).gameObject.TryGetComponent(out QuestTitle)) {
            Debug.LogError("ERROR: Failed to find quest text child component for QuestUIComponent");
            return false;
        }

        if (!transform.GetChild(1).gameObject.TryGetComponent(out QuestInstructions)) {
            Debug.LogError("ERROR: Failed to find quest text child component for QuestUIComponent");
            return false;
        }
        return true;
    }

    // Set the associated quest and initialize all properties
    public void Initialize(string questId)
    {
        // Store ID
        this.questId = questId;

        // Observe the manager events
        QuestManager manager = QuestManager.GetInstance();
        manager.QuestUpdated.AddListener(OnQuestChanged);

        // Initialize all text
        UpdateText();

        // Make text invisible
        if (QuestTitle != null && QuestInstructions != null)
        {
            Color currentTitleColor = QuestTitle.color;
            Color currentInstColor = QuestInstructions.color;
            currentTitleColor.a = 0.0f;
            currentInstColor.a = 0.0f;
            QuestTitle.color = currentTitleColor;
            QuestInstructions.color = currentInstColor;
        }
    }

    // Update the text to match the state of the quest
    protected void UpdateText ()
    {
        // Need the text components to be set
        if (!EnsureComponentsSet())
        {
            return;
        }

        // Get reference to quest manager
        QuestManager manager = QuestManager.GetInstance();

        // Get quest data
        QuestData qData = manager.GetQuest(questId);
        if (qData == null || !qData.IsStarted())
        {
            // Update display name & instructions
            QuestTitle.text = "";
            QuestInstructions.text = "";
        }
        else if(qData != null)
        {
            // Update display name & instructions
            QuestTitle.text = qData.GetDisplayName();
            QuestInstructions.text = qData.GetCurrentInstructions();
        }
        else
        {
            Debug.LogWarning($"WARNING: Unknown quest id '{questId}' in QuestUIComponent");
        }
    }

    // Fade in the text
    public IEnumerator FadeIn (float duration)
    {
        return DoFade(1.0f, duration);
    }

    // Fade out the text
    public IEnumerator FadeOut (float duration)
    {
        return DoFade(0.0f, duration);
    }

    // Coroutine to apply the fade
    protected IEnumerator DoFade(float endAlpha, float duration)
    {
        // Get initial color alpha value
        Color currentTitleColor = QuestTitle.color;
        Color currentInstColor = QuestInstructions.color;
        float startAlphaTitle = currentTitleColor.a;
        float startAlphaInst = currentInstColor.a;

        // Apply the fade change over time
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            currentTitleColor.a = Mathf.Lerp(startAlphaTitle, endAlpha, timer / duration);
            currentInstColor.a = Mathf.Lerp(startAlphaInst, endAlpha, timer / duration);
            QuestTitle.color = currentTitleColor;
            QuestInstructions.color = currentInstColor;

            yield return null;
        }

        // Ensure final alpha is reached
        currentTitleColor.a = endAlpha;
        currentInstColor.a = endAlpha;
        QuestTitle.color = currentTitleColor;
        QuestInstructions.color = currentInstColor;
    }

    // Respond to quest update event
    protected void OnQuestChanged(string questId)
    {
        if (this.questId == questId)
        {
            UpdateText();
        }
    }
}
