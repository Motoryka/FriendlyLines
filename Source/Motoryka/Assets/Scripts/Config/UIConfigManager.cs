using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIConfigManager : MonoBehaviour {
    public ConfigCreator creator;
    public Toggle ShowStartPointToggle;
    public InputField ConfigNameInputField;
    public Dropdown DrawTimeoutDropdown;

    Config config;

    public void SaveConfig()
    {
        creator.SendMessage("SaveConfig");
    }

    public void SaveAsNewConfig()
    {
        creator.SendMessage("SaveAsNewConfig");
    }

    public void Cancel()
    {
        creator.SendMessage("Cancel");
    }

    public void Init(Config config)
    {
        ConfigNameInputField.text = config.Name;
        ShowStartPointToggle.isOn = config.DrawStartPoint;

        DrawTimeoutDropdown.onValueChanged.AddListener(i => UpdateDrawTimeout(i));
        ConfigNameInputField.onValueChange.AddListener(s => UpdateName(s));
        ShowStartPointToggle.onValueChanged.AddListener(b => UpdateShowStartPoint(b));

        this.config = config;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateDrawTimeout(int index)
    {
        
        string value = DrawTimeoutDropdown.options[index].text;
        float timeout = float.Parse(value.Split('s')[0]);

        Debug.Log("Draw timeout change " + timeout);
    }

    void UpdateName(string s)
    {
        config.Name = s;
    }

    void UpdateShowStartPoint(bool value)
    {
        config.DrawStartPoint = value;
    }
}
