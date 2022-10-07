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
    public  void OnRedClick()
    {
        img.color = Color.red;
    }
    public void OnGreenClick()
    {
        img.color= Color.green;

    }
    public void OnBlueClick()
    {
        img.color=Color.blue;
    }
}
