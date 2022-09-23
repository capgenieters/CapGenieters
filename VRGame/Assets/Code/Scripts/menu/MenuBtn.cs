using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBtn: MonoBehaviour
{
    public GameObject panel;
    public Image img;

    public void Start()
    {
        img = panel.GetComponent<Image>();
    }
    public  void ColorChangeOnClick()
    {
        img.color = img.color == Color.red ? Color.blue : Color.red;
    }
}
