using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective")]
public class Objective : ScriptableObject
{
    public Tasks[] tasks;


    [System.Serializable]
    public class Tasks
    {
        public string name;
        public string objective;
        public bool completed;
    }

    private ObjectiveDisplay display;

    public void CompleteObjective(string name)
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (name == tasks[i].name)
                tasks[i].completed = true;
        }

        if (display == null)
            display = GameObject.FindObjectOfType<ObjectiveDisplay>();

        display.CheckComplete();
    }


    //obj.CompleteObjective();
}

