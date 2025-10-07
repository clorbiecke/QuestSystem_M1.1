using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData// : MonoBehaviour
{
//#if UNITY_EDITOR
//    // Refrences to all QuestData objects
//    public static Dictionary<QuestData, string> allQuests = new();
//#endif

    static readonly int START = 0;
    static readonly int NOT_STARTED = -1;

    [SerializeField]
    [Tooltip("Quest ID")]
    private string id = "not set";

    [SerializeField]
    [Tooltip("The quest name to display to players.")]
    private string displayName = "not set";



    [SerializeField]
    [Tooltip("An array of instructions, one for each step in the quest.")]
    private string[] stepInstructions = null;


    // Which step of the quest the player is on
    public int currentStep = -1;



    public QuestData(string id,  string displayName, string[] stepInstructions)
    {
        if (id == null || id == "")
        {
            Debug.LogError("Quest member variable 'id' is empty!");
        }
        else
        {
            this.id = id;
        }

        if (displayName == null || displayName == "")
        {
            Debug.LogWarning("Quest member variable 'displayName' is empty!");
        }
        else
        {
            this.displayName = displayName;
        }


        this.stepInstructions = stepInstructions;
        currentStep = -1;
    }

    // Getter for id
    public string GetID()
    {
        return id;
    }

    // Getter for displayName
    public string GetDisplayName()
    {
        return displayName;
    }

    // Setter for displayName
    public void SetDisplayName(string newName)
    {
        displayName = newName;
    }

    // Getter for currentStep
    public int GetCurrentStep()
    {
        return currentStep;
    }

    // Get instructions for current step
    public string GetCurrentInstructions()
    {
        // if quest not started..
        if (currentStep < 0)
        {
            return "--";
        }
        // else if quest completed...
        else if (currentStep >=  stepInstructions.Length)
        {
            return "COMPLETED";
        }
        // else (quest is in progress)
        else
        {
            return stepInstructions[currentStep];
        }
    }

    // Get total number of steps in quest
    public int GetTotalSteps()
    {
        return stepInstructions.Length;
    }

    // Resets currentStep to -1, marking the quest as not started
    public void ResetQuest()
    {
        Debug.Log($"Quest {id} reset.");
        currentStep = -1;
    }

    // Advances the quest to and returns the next step, or, if bool jumpToEnd is true, completes the quest
    public int NextStep(bool jumpToEnd = false)
    {
        Debug.Log($"Called NextStep({jumpToEnd}) on quest {id}");
        int prevStep = currentStep;
        currentStep = (jumpToEnd) ? stepInstructions.Length : currentStep + 1;
        Debug.Log($"Quest {id} progressed from step {prevStep} to step {currentStep}");
        return currentStep;
    }

    // Returns true if currentStep is greater than or equal to 0, and false otherwise
    public bool IsStarted()
    {
        return (currentStep >= 0);
    }

    // Returns true if currentStep is greater than or equal to the total number of steps
    public bool IsComplete()
    {
        return (currentStep >= stepInstructions.Length);
    }

    // Returns a single line including all member variables
    override public string ToString()
    {
        return $"{id}, {displayName}, {currentStep}, {stepInstructions}";
    }

    // Starts the quest. Returns true if successful and false otherwise (ex. quest already started)
    public bool StartQuest()
    {
        if (!IsStarted())
        {
            currentStep = START;
            return true;
        }
        return false ;
    }



}
