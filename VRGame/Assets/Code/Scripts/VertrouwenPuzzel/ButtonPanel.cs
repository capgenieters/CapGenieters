using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    private VertrouwenButtonManager _buttonManager;
    private Animator _animator;

    public void SetData(VertrouwenButtonManager buttonManager)
    {
        _buttonManager = buttonManager;
        _buttonManager.ActivePanels.Add(this);
        _animator = GetComponentInChildren<Animator>();
    }
    public void ActivePanel()
    {
        _animator.Play("Open");
    }
    public void DeactivatePanel()
    {
        _buttonManager.ActivePanels.Remove(this);
        _animator.Play("Close");
    }
}
