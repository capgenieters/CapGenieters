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
        private ObjectiveDisplay display;

        public string name;
        public string objective;
        public bool completed;
        public GameObject gameObject;

        public void CompleteObjective()
        {
            this.completed = true;

            if (display == null)
                display = GameObject.FindObjectOfType<ObjectiveDisplay>();

            display.Complete(this);
        }
    }

    private ObjectiveDisplay display;
}

