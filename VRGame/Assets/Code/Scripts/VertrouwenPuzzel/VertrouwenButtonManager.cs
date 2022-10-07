using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertrouwenButtonManager : MonoBehaviour
{
    public List<ButtonPanel> ActivePanels;

    [SerializeField]
    private ButtonPanel _firstPanel;
    [SerializeField]
    private List<ButtonPanel> _panels;
    private ButtonPanel _currentPanel;
    private TempElevator _tempElevator;
    
    private void Start()
    {
        _tempElevator = GameObject.FindObjectOfType<TempElevator>();
        foreach (ButtonPanel panel in _panels)
        {
            panel.SetData(this);
        }
        if (_firstPanel == null)
        {
            _firstPanel = _panels[Random.Range(0, _panels.Count - 1)];
        }        
        _currentPanel = _firstPanel;
        _currentPanel.ActivePanel();
    }

    public void SwitchToPanel(ButtonPanel newPanel = null)
    {
        _currentPanel.DeactivatePanel();
        _currentPanel = newPanel;
        _currentPanel.ActivePanel();
    }

    public void SwitchRandomPanel()
    {
        if (_currentPanel == null)
        {
            return;
        }
        _currentPanel.DeactivatePanel();
        if (ActivePanels.Count > 0)
        {
            _currentPanel = ActivePanels[Random.Range(0, ActivePanels.Count - 1)];
            _currentPanel.ActivePanel();
        }
        else
        {
            _firstPanel = null;
            _currentPanel = null;
            _tempElevator.OpenElevator();
        }
        
    }
}
