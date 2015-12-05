using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class UIConfigManager : MonoBehaviour {
    public ConfigCreator creator;
    public Toggle ShowStartPointToggle;
    public InputField ConfigNameInputField;
    public Dropdown DrawTimeoutDropdown;
	public GameObject CannotSavePanel;
	public GameObject CancelPanel;
	public GameObject EmptyNamePanel;
	private GameObject BlackImage;
	private int configReplaceId;

    Config config;

    public void SaveConfig()
    {
		if(ConfigNameInputField.text == ""){
			EmptyNamePanel.SetActive(true);
			SetInteractableOfAllSceneObjects(false);
			BlackImage.SetActive(true);
			return;
		}
		configReplaceId = DoesConfigNameExist(config.Name);
		if(configReplaceId != -1){
			CannotSavePanel.SetActive(true);
			SetInteractableOfAllSceneObjects(false);
			BlackImage.SetActive(true);
		}
		else{
        	creator.SendMessage("SaveConfig");
		}
    }

	public void SaveConfigForSure()
	{
		creator.SendMessage("SaveAndReplaceConfig", configReplaceId);
	}

	public void CloseCannotSavePanel()
	{
		CannotSavePanel.SetActive(false);
		SetInteractableOfAllSceneObjects(true);
		BlackImage.SetActive(false);
	}

	public void CancelForSure()
	{
		creator.SendMessage("Cancel");
	}

	public void CloseCancelPanel()
	{
		CancelPanel.SetActive(false);
		SetInteractableOfAllSceneObjects(true);
		BlackImage.SetActive(false);
	}

	public void CloseEmptyNamePanel()
	{
		EmptyNamePanel.SetActive(false);
		SetInteractableOfAllSceneObjects(true);
		BlackImage.SetActive(false);
	}

	private void SetInteractableOfAllSceneObjects(bool b)
	{
		GameObject canvas = GameObject.Find ("Canvas");
		foreach(var button in canvas.GetComponentsInChildren<Button>()){
			button.interactable = b;
		}
		foreach(var input in canvas.GetComponentsInChildren<InputField>()){
			input.interactable = b;
		}
		foreach(var toggle in canvas.GetComponentsInChildren<Toggle>()){
			toggle.interactable = b;
		}
		foreach(var dropdown in canvas.GetComponentsInChildren<Dropdown>()){
			dropdown.interactable = b;
		}
		foreach(var slider in canvas.GetComponentsInChildren<Slider>()){
			slider.interactable = b;
		}
	}

    public void Cancel()
    {
		CancelPanel.SetActive(true);
		SetInteractableOfAllSceneObjects(false);
		BlackImage.SetActive(true);
    }

    public void Init(Config config)
    {
        ConfigNameInputField.text = config.Name;
        ShowStartPointToggle.isOn = config.DrawStartPoint;

        DrawTimeoutDropdown.onValueChanged.AddListener(i => UpdateDrawTimeout(i));
        ConfigNameInputField.onValueChange.AddListener(s => UpdateName(s));
        ShowStartPointToggle.onValueChanged.AddListener(b => UpdateShowStartPoint(b));

		CannotSavePanel = GameObject.Find ("CannotSavePanel");
		CannotSavePanel.SetActive(false);

		CancelPanel = GameObject.Find ("CancelPanel");
		CancelPanel.SetActive(false);

		EmptyNamePanel = GameObject.Find ("EmptyNamePanel");
		EmptyNamePanel.SetActive(false);

		BlackImage = GameObject.Find ("BlackImage");
		BlackImage.SetActive(false);

        this.config = config;

        int waitingSecs = (int)config.WaitingTime;
        int index = 0;
        foreach( var option in DrawTimeoutDropdown.options)
        {
            if(getSecsFromDrawTimeout(option.text) == waitingSecs)
            {
                DrawTimeoutDropdown.value = index;
                break;
            }

            index++;
        }
    }
	// Use this for initialization
	void Start () {
	
	}

	private int DoesConfigNameExist(string name)
	{
		List<KeyValuePair<int, string>> ConfigIdAndName = new List<KeyValuePair<int, string>>();
		List<string> configNames = new List<string>();
		string[] configFiles = Directory.GetFiles(Application.persistentDataPath + "/configs/");
		foreach(var file in configFiles)
		{
			int cid = -1;
			string cname = "";
			XmlTextReader reader = new XmlTextReader(file);
			while(reader.Read ())
			{
				reader.MoveToContent();
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "Id")
				{
					reader.Read();
					int id = -1;
					int.TryParse(reader.Value, out id);
					cid = id;
				}
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "Name")
				{
					reader.Read();
					cname = reader.Value;
				}
				if(cid != -1 && cname != "")
				{
					ConfigIdAndName.Add(new KeyValuePair<int, string>(cid, cname));
					break;
				}
			}
			reader.Close();
		}
		int configId =  ConfigIdAndName.Find (x => x.Value == name).Key;
		if(configId != 0)
		{
			return configId;
		}

		return -1;
	}

    void UpdateDrawTimeout(int index)
    {
        
        string value = DrawTimeoutDropdown.options[index].text;
        float timeout = getSecsFromDrawTimeout(value);

        Debug.Log("Draw timeout change " + timeout);

        config.WaitingTime = timeout;
    }

    int getSecsFromDrawTimeout(string value)
    {
        return int.Parse(value.Split('s')[0]);
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
