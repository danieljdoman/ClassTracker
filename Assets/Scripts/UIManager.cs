using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("Scripts")]
    public ChildManager childManager;

    [Header("Settings")]
    public GameObject settingsPanel;

    [Header("Dashboard")]
    public GameObject dashboardPanel;
    public GameObject childListContainer; // The container for displaying children
    public GameObject childListItemPrefab; // A prefab for each child's list item

    [Header("Add Child Panel")]
    public GameObject addChildPanel;
    public TMP_InputField childNameInput;

    [Header("Child Details Panel")]
    public GameObject childDetailsPanel;
    public TMP_Text childNameText;
    public GameObject activityListContainer; // Content container for activities
    public GameObject activityListItemPrefab; // Prefab for each activity
    Child selectedChild;

    [Header("Debug")]
    public TMP_Text debugText;
    Coroutine debugCoroutine;

    [Header("Choose Activity Type Panel")]
    public GameObject chooseActivityTypePanel;

    [Header("Preset Activity Selection Panel")]
    public GameObject presetActivitySelectionPanel; // Panel to show preset activity options
    public GameObject presetActivitySelectionListContainer;

    [Header("Custom Activity Selection Panel")]
    public GameObject customActivitySelectionPanel; // Panel to add custom activity
    public TMP_InputField activityDescriptionInput;
    public TMP_Dropdown activityDifficultyDropdown;
    public TMP_InputField activityLevelInput;

    Activity selectedPresetActivity;

    [Header("Preset Activities Panel")]
    public GameObject presetActivitiesPanel;
    public GameObject presetActivityListContainer; // Scroll View for presets
    public GameObject presetActivityItemPrefab; // Prefab for preset activity items
    public GameObject createPresetActivityPanel;
    public TMP_InputField presetActivityDescriptionInput;
    public TMP_Dropdown presetActivityDifficultyDropdown;
    public TMP_InputField presetActivityLevelInput;

    [Header("Note Prompt Panel")]
    public GameObject notePromptPanel; // UI panel for prompting a custom note
    public TMP_InputField noteInputField; // Input field for entering the note
    Activity pendingActivity; // Temporarily store the activity being added

    [Header("Quit Dialog Panel")]
    [SerializeField] GameObject quitDialogPanel; // Assign your dialog panel in the Inspector
    bool isQuitDialogActive = false;

    InputAction quitAction;

    void OnEnable()
    {
        InputSystem.actions.FindActionMap("UI").Enable();
    }

    void OnDisable()
    {
        InputSystem.actions.FindActionMap("UI").Disable();
    }

    void Awake()
    {
        quitAction = InputSystem.actions.FindAction("Quit");
    }

    void Start()
    {
        DataManager.LoadData(childManager);
        UpdateChildList(); // Update UI with the loaded children
    }

    void Update()
    {
        if (quitAction.WasPressedThisFrame()) // Android back button/keyboard escape
        {
            if (!isQuitDialogActive)
            {
                ShowQuitAppDialog();
            }
        }
    }

    public void ShowAddChildPanel()
    {
        addChildPanel.SetActive(true);
        dashboardPanel.SetActive(false);
    }

    public void HideAddChildPanel()
    {
        addChildPanel.SetActive(false);
        dashboardPanel.SetActive(true);
    }

    public void AddChild()
    {
        string childName = childNameInput.text;
        if (string.IsNullOrEmpty(childName))
        {
            Debug.LogWarning("Child name is required!");
            ShowDebugMessage("Child name is required!");
            return;
        }
        else
        {
            childManager.AddChild(childName);
            UpdateChildList();
            HideAddChildPanel();

            // Clear the input field
            childNameInput.text = "";

            DataManager.SaveData(childManager);
        }
    }

    public void UpdateChildList()
    {
        // Clear the list
        foreach (Transform child in childListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate the list
        foreach (Child child in childManager.children)
        {
            GameObject listItem = Instantiate(childListItemPrefab, childListContainer.transform);
            listItem.GetComponentInChildren<TMP_Text>().text = child.Name;

            // Capture child in a local variable
            Child capturedChild = child;

            // Assign the show details button
            Button detailsButton = listItem.GetComponent<Button>();
            detailsButton.onClick.AddListener(() => ShowChildDetails(capturedChild));

            // Add a delete button
            Button deleteButton = listItem.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => RemoveChild(capturedChild));
        }
    }

    public void RemoveChild(Child child)
    {
        childManager.RemoveChild(child);
        UpdateChildList();
        HideChildDetails();

        DataManager.SaveData(childManager);
    }

    public void UpdateActivityList(Child child)
    {
        // Clear the list
        foreach (Transform activity in activityListContainer.transform)
        {
            Destroy(activity.gameObject);
        }

        // Populate the list with activities
        foreach (Activity activity in child.Activities)
        {
            GameObject listItem = Instantiate(activityListItemPrefab, activityListContainer.transform);
            TMP_Text[] textFields = listItem.GetComponentsInChildren<TMP_Text>();
            textFields[0].text = activity.Description;
            textFields[1].text = activity.Difficulty;
            textFields[2].text = activity.Level;
            textFields[3].text = activity.Timestamp;
            textFields[4].text = activity.Note;

            // Add a delete button
            Button deleteButton = listItem.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => RemoveChildActivity(child, activity));
        }
    }

    public void RemoveChildActivity(Child child, Activity activity)
    {
        child.Activities.Remove(activity);
        UpdateActivityList(child);

        DataManager.SaveData(childManager);
    }

    public void ShowChildDetails(Child child)
    {
        selectedChild = child;
        childNameText.text = child.Name;
        UpdateActivityList(child);
        childDetailsPanel.SetActive(true);
        dashboardPanel.SetActive(false);
    }

    public void HideChildDetails()
    {
        childDetailsPanel.SetActive(false);
        dashboardPanel.SetActive(true);
    }

    public void ShowChooseActivityTypePanel()
    {
        chooseActivityTypePanel.SetActive(true);
        childDetailsPanel.SetActive(false);
    }

    public void HideChooseActivityTypePanel()
    {
        chooseActivityTypePanel.SetActive(false);
        childDetailsPanel.SetActive(true);
    }    

    public void SelectPresetActivityType()
    {
        presetActivitySelectionPanel.SetActive(true);
        customActivitySelectionPanel.SetActive(false);
        chooseActivityTypePanel.SetActive(false);
        UpdatePresetActivitySelectionList(); // Refresh preset list
    }

    public void HidePresetActivitySelectionPanel()
    {
        presetActivitySelectionPanel.SetActive(false);
        chooseActivityTypePanel.SetActive(true);
    }

    public void UpdatePresetActivitySelectionList()
    {
        // Clear the existing list
        foreach (Transform child in presetActivitySelectionListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate the list with preset activities
        foreach (Activity activity in childManager.presetActivities)
        {
            GameObject listItem = Instantiate(presetActivityItemPrefab, presetActivitySelectionListContainer.transform);
            TMP_Text[] textFields = listItem.GetComponentsInChildren<TMP_Text>();
            textFields[0].text = activity.Description;
            textFields[1].text = activity.Difficulty;
            textFields[2].text = activity.Level;

            // Add a button listener to select this preset
            Button selectButton = listItem.GetComponent<Button>();
            Activity capturedActivity = activity; // Capture in a local variable for the lambda
            selectButton.onClick.AddListener(() => SelectPresetActivity(capturedActivity));
        }
    }

    public void SelectPresetActivity(Activity activity)
    {
        selectedPresetActivity = activity; // Store the selected activity
        ShowDebugMessage($"Selected preset: {activity.Description}");
        AddActivity();
    }

    public void SelectCustomActivityType()
    {
        presetActivitySelectionPanel.SetActive(false);
        chooseActivityTypePanel.SetActive(false);
        ShowCustomActivitySelectionPanel();
    }

    public void ShowCustomActivitySelectionPanel()
    {
        customActivitySelectionPanel.SetActive(true);

        activityDescriptionInput.text = "";
        activityDifficultyDropdown.value = 0; // Resets to the first option
        activityDifficultyDropdown.RefreshShownValue(); // Updates the UI to reflect the change
        activityLevelInput.text = "";
    }

    public void HideCustomActivitySelectionPanel()
    {
        customActivitySelectionPanel.SetActive(false);
        chooseActivityTypePanel.SetActive(true);
    }

    public void AddActivity()
    {
        Activity newActivity;

        if (selectedPresetActivity == null)
        {
            // Create a custom activity
            string description = activityDescriptionInput.text;
            string difficulty = activityDifficultyDropdown.options[activityDifficultyDropdown.value].text;
            string level = activityLevelInput.text;

            if (string.IsNullOrEmpty(description))
            {
                Debug.LogWarning("Activity description is required!");
                ShowDebugMessage("Activity description is required!");
                return;
            }

            newActivity = new Activity
            {
                Description = description,
                Difficulty = difficulty,
                Level = level,
                Note = "", // Note will be provided by the user
            };
        }
        else
        {
            // Clone the selected preset and proceed
            newActivity = new Activity
            {
                Description = selectedPresetActivity.Description,
                Difficulty = selectedPresetActivity.Difficulty,
                Level = selectedPresetActivity.Level,
                Note = "", // Note will be provided by the user
            };
        }
        // Prompt the user for a custom note
        PromptForNote(newActivity);
    }

    public void ShowPresetActivitiesPanel()
    {
        settingsPanel.SetActive(false);
        presetActivitiesPanel.SetActive(true);
        UpdatePresetActivityList();
    }

    public void HidePresetActivitiesPanel()
    {
        presetActivitiesPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowCreatePresetActivityPanel()
    {
        createPresetActivityPanel.SetActive(true);
        presetActivitiesPanel.SetActive(false);
    }

    public void HideCreatePresetActivityPanel()
    {
        createPresetActivityPanel.SetActive(false);
        presetActivitiesPanel.SetActive(true);
    }

    public void CreatePresetActivity()
    {
        string description = presetActivityDescriptionInput.text;
        string difficulty = presetActivityDifficultyDropdown.options[presetActivityDifficultyDropdown.value].text;
        string level = presetActivityLevelInput.text;

        if (string.IsNullOrEmpty(description))
        {
            Debug.LogWarning("Preset activity description is required!");
            ShowDebugMessage("Preset activity description is required!");
            return;
        }

        childManager.AddPresetActivity(description, difficulty, level);
        UpdatePresetActivityList();
        HideCreatePresetActivityPanel();

        DataManager.SaveData(childManager);
    }

    public void UpdatePresetActivityList()
    {
        // Clear the list
        foreach (Transform child in presetActivityListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate the list
        foreach (Activity activity in childManager.presetActivities)
        {
            GameObject listItem = Instantiate(presetActivityItemPrefab, presetActivityListContainer.transform);
            TMP_Text[] textFields = listItem.GetComponentsInChildren<TMP_Text>();
            textFields[0].text = activity.Description;
            textFields[1].text = activity.Difficulty;
            textFields[2].text = activity.Level;

            // Add a delete button
            Button deleteButton = listItem.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => RemovePresetActivity(activity));
        }
    }

    public void RemovePresetActivity(Activity activity)
    {
        childManager.RemovePresetActivity(activity);
        UpdatePresetActivityList();

        DataManager.SaveData(childManager);
    }

    public void PromptForNote(Activity activity)
    {
        pendingActivity = activity; // Store the activity temporarily
        noteInputField.text = ""; // Clear the note input
        notePromptPanel.SetActive(true); // Show the note prompt panel
        presetActivitySelectionPanel.SetActive(false);
        customActivitySelectionPanel.SetActive(false);
    }

    public void SaveNote()
    {
        if (pendingActivity != null)
        {
            // Assign the user-provided note to the activity
            pendingActivity.Note = noteInputField.text;
            pendingActivity.Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // Add the activity to the selected child's activity list
            selectedChild.Activities.Add(pendingActivity);
            UpdateActivityList(selectedChild);

            // Clear the pending activity and close the note prompt
            pendingActivity = null;
            selectedPresetActivity = null;
            notePromptPanel.SetActive(false);
            childDetailsPanel.SetActive(true);

            DataManager.SaveData(childManager);
        }
    }

    public void OpenSettingsPanel()
    {
        dashboardPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        dashboardPanel.SetActive(true);
    }

    public void ShowQuitAppDialog()
    {
        isQuitDialogActive = true;
        quitDialogPanel.SetActive(true);
    }

    public void HideQuitAppDialog()
    {
        isQuitDialogActive = false;
        quitDialogPanel.SetActive(false);
    }

    public void QuitApplication()
    {
        DataManager.SaveData(childManager);  // Save the childManager before quitting
        Application.Quit();
    }

    public void ShowDebugMessage(string message)
    {
        // Stop any currently running debug message coroutine
        if (debugCoroutine != null)
        {
            StopCoroutine(debugCoroutine);
        }

        // Start a new coroutine to display the message
        debugCoroutine = StartCoroutine(DisplayDebugMessage(message));
    }

    IEnumerator DisplayDebugMessage(string message)
    {
        debugText.text = message; // Set the debug message
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        debugText.text = ""; // Clear the text
        debugCoroutine = null; // Reset the coroutine reference
    }
}
