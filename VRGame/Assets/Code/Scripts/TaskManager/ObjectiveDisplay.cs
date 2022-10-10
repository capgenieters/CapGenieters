using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveDisplay : MonoBehaviour
{
    public GameObject check;
    private Objective obj;
    private Dictionary<int, GameObject> textObjects = new Dictionary<int, GameObject>();

    public AudioClip checkSound;
    AudioSource src;

    void Start()
    {
        CreateObjective();
        UpdateCheckbox();
    }

    public void UpdateCheckbox()
    {
        for (int i = 0; i < obj.tasks.Length; i++)
        {
            GameObject textObject = new GameObject("Objective " + (i + 1));
            GameObject checkBox = Instantiate(check, new Vector3(0, 0, 0), Quaternion.identity);
            textObject.transform.SetParent(this.transform);
            checkBox.transform.SetParent(textObject.transform);

            if (obj.tasks.Length >= transform.childCount / 2)
            {
                textObject.AddComponent<TextMeshProUGUI>().text = obj.tasks[i].objective;
            }

            textObject.GetComponent<TextMeshProUGUI>().fontSize = 20;
            textObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            textObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            textObject.transform.localPosition = new Vector3(0, (40 - (i * 30)), 0);
            checkBox.transform.position = textObject.transform.position + new Vector3(120, -7, 0);
            textObjects.Add(i, textObject);
        }
    }

    public void CheckComplete()
    {
        foreach (KeyValuePair<int, GameObject> v in textObjects)

            if (obj.tasks[v.Key].completed)
            {
                if (checkSound != null)
                {
                    src = gameObject.AddComponent<AudioSource>();
                    src.clip = checkSound;
                    src.PlayDelayed(1);
                }
                GameObject checkBox = v.Value.transform.GetChild(0).gameObject;
                checkBox.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                v.Value.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            }
    }

    void CreateObjective()
    {
        obj = Resources.Load<Objective>("First");
    }

}
