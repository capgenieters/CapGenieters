using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveDisplay : MonoBehaviour
{
    [SerializeField] private GameObject check;
    [SerializeField] private AudioClip checkSound;
    [SerializeField] private Objective objective;
    private AudioSource src;

    void Start()
    {
        CreateObjective();
    }

    public void CreateCheckbox(Objective.Tasks task, int i)
    {
        GameObject textObject = new GameObject("Objective " + i);
        GameObject checkBox = Instantiate(check, new Vector3(0, 0, 0), Quaternion.identity);
        textObject.transform.SetParent(this.transform);
        checkBox.transform.SetParent(textObject.transform);

        textObject.AddComponent<TextMeshProUGUI>().text = task.objective;
        textObject.GetComponent<TextMeshProUGUI>().fontSize = 20;
        textObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        textObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        textObject.transform.localPosition = new Vector3(0, (40 - (i * 30)), 0);
        checkBox.transform.position = textObject.transform.position + new Vector3(120, -7, 0);
        task.gameObject = textObject;
    }

    public void Complete(Objective.Tasks t)
    {
        if (checkSound != null)
        {
            src = gameObject.AddComponent<AudioSource>();
            src.clip = checkSound;
            src.PlayDelayed(1);
        }

        Transform transform = t.gameObject.transform;
        GameObject checkBox = transform.GetChild(0).gameObject;
        Debug.Log(checkBox.transform.GetChild(0).GetChild(1));
        checkBox.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        transform.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
    }

    void CreateObjective()
    {
        int i = 1;

        foreach (Objective.Tasks t in objective.tasks)
        {
            CreateCheckbox(t, i);
            i++;
        }
    }

}
