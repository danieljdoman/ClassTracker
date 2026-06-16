using System;
using System.Collections.Generic;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    // Child management
    public List<Child> children = new();

    public void AddChild(string name)
    {
        Child newChild = new() { Name = name };
        children.Add(newChild);
    }

    public void RemoveChild(Child child)
    {
        children.Remove(child);
    }

    // Preset activity management
    public List<Activity> presetActivities = new();

    public void AddPresetActivity(string description, string difficulty = "", string level = "")
    {
        Activity newPreset = new()
        {
            Description = description,
            Difficulty = difficulty,
            Level = level,
            Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm") // Default timestamp for presets
        };
        presetActivities.Add(newPreset);
    }

    public void RemovePresetActivity(Activity activity)
    {
        presetActivities.Remove(activity);
    }
}
