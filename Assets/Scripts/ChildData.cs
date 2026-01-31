using System;
using System.Collections.Generic;

[Serializable]
public class Activity
{
    public string Description; // "Puzzle", "Memory"
    public string Difficulty; // "Easy", "Medium", or "Hard"
    public string Level; // "1", "3", "6"
    public string Note; // "Great performance!", "Poor memory skill."
    public string Timestamp; // Date and time of activity captured
}

[Serializable]
public class Child
{
    public string Name; // "John", "Alex", "Susan"
    public List<Activity> Activities = new(); // List of activities captured for each child
}