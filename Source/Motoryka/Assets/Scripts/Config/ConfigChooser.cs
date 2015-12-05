using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;
using UnityEngine.UI;

public class ConfigFile 
{
	public string Path;
	public string Name;
	public int NrOfLevels;
}

public class ConfigChooser : MonoBehaviour 
{
	public List<ConfigFile> ConfigFiles;
	public GameObject configButton;
	List<GameObject> ConfigButtons;
	public GameObject canvas;
	public GameObject editButton;
	public GameObject removeButton;
	public GameObject removePanel;
	public GameObject blackImage;
	public static string selectedConfigName = "";
	public Transform ContentPanel;

	// Use this for initialization
	void Start()
    {
		Init();

		this.removePanel = GameObject.Find("RemovePanel");
        this.removePanel.SetActive(false);
        this.blackImage = GameObject.Find("BlackImage");
        this.blackImage.SetActive(false);
	}

	private void Init()
	{
        this.ConfigFiles = new List<ConfigFile>();
        this.ConfigButtons = new List<GameObject>();
        this.SetSelection();
        this.StoreAllConfigs();
        this.CreateConfigButtons();
	    if (selectedConfigName != "")
	    {
	        this.ShowButtons();
	    }
        else this.HideButtons();
	}

	private void SetInteractableOfAllSceneObjects(bool b)
	{
		GameObject canvas = GameObject.Find("Canvas");
		foreach(var button in canvas.GetComponentsInChildren<Button>())
        {
			button.interactable = b;
		}

		foreach(var input in canvas.GetComponentsInChildren<InputField>())
        {
			input.interactable = b;
		}

		foreach(var toggle in canvas.GetComponentsInChildren<Toggle>())
        {
			toggle.interactable = b;
		}

		foreach(var dropdown in canvas.GetComponentsInChildren<Dropdown>())
        {
			dropdown.interactable = b;
		}

		foreach(var slider in canvas.GetComponentsInChildren<Slider>())
        {
			slider.interactable = b;
		}
	}
	
	// Update is called once per frame
	void Update() 
    {
	}

	public void BackToTitle()
	{
		GameManager.Instance.fader.LoadSceneFading("title");
	}

	public void EditConfig()
	{
		GameManager.Instance.fader.LoadSceneFading("config");
	}

	public void ShowButtons()
	{
		this.editButton.SetActive(true);
		this.removeButton.SetActive(true);
	}

	public void HideButtons()
	{
		this.editButton.SetActive(false);
		this.removeButton.SetActive(false);
	}

	private void SetSelection()
	{
		switch(GameManager.Instance.GameConfig.Id)
        {
		case -3:
			this.ChooseButton("EasyLevelButton");
			break;
		case -2:
            this.ChooseButton("MediumLevelButton");
			break;
		case -1:
            this.ChooseButton("HardLevelButton");
			break;
		}
	}

	private void ChooseButton(string buttonName)
	{
        Transform[] objs = this.canvas.GetComponentsInChildren<Transform>();
		foreach(var o in objs)
        {
			var btn = o.transform.GetComponent<Button>();
			if(btn != null)
            {
				if(btn.name == buttonName)
                {
					if(btn.animator != null)
                    {
						btn.animator.SetBool("isChosen", true);
					}
				}
			}
		}
	}

    private void StoreAllConfigs()
	{
		string[] configFiles = Directory.GetFiles(Application.persistentDataPath + "/configs/");
		foreach(var file in configFiles)
		{
			string name = "";
			int nrOfLevels = 0;
			XmlTextReader reader = new XmlTextReader(file);
			while(reader.Read())
			{
				reader.MoveToContent();
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "Name")
				{
					reader.Read();
					name = reader.Value;
				}

				if(reader.NodeType == XmlNodeType.Element && reader.Name == "NrOfLevels")
				{
					reader.Read();
					int intp = 0;
					int.TryParse(reader.Value, out intp);
					nrOfLevels = intp;
				}

				if(name != "" && nrOfLevels != 0)
				{
					this.ConfigFiles.Add(new ConfigFile { Path = file, Name = name, NrOfLevels = nrOfLevels });
					break;
				}
			}

			reader.Close();
		}
	}

	public void GetConfig(string name)
	{
		ConfigFile cf = this.ConfigFiles.Find(x => x.Name == name);
		if(cf == null) return;
		GameManager.Instance.GameConfig = ConfigLoader.DeserializeConfig(Path.GetFileNameWithoutExtension(cf.Path));
		selectedConfigName = name;
	}

	public void EasyLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateEasyLevel();
        this.HideButtons();
	}

	public void MediumLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateMediumLevel();
        this.HideButtons();
	}

	public void HardLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateHardLevel();
        this.HideButtons();
	}

	public void ChooseSelectedButton(Button button)
	{
        this.RemoveSelectionOnButton();
		if(button.animator != null)
        {
			button.animator.SetBool("isChosen", true);
		}
		else
        {
			button.image.color = PastelColorFactory.Mint.Color;
		}
	}

	private void RemoveSelectionOnButton()
	{
        Transform[] objs = this.canvas.GetComponentsInChildren<Transform>();
		foreach(var o in objs)
        {
			var btn = o.transform.GetComponent<Button>();
			if(btn != null)
            {
				if(btn.animator != null)
                {
					btn.animator.SetBool("isChosen", false);
				}
				else
                {
					btn.image.color = Color.white;
				}
			}
		}
	}

    private void CreateConfigButtons()
	{
		int i = 0;
		foreach(var cf in this.ConfigFiles)
		{
            GameObject o = Instantiate<GameObject>(this.configButton);
			o.transform.SetParent(ContentPanel);
			Text cn = o.GetComponentInChildren<Transform>().Find("configName").gameObject.GetComponent<Text>();
			cn.text = cf.Name;
			if(cf.Name == selectedConfigName)
            {
				o.GetComponent<Button>().image.color = PastelColorFactory.Mint.Color;
			}
			Text cnof = o.GetComponentInChildren<Transform>().Find("numberOfLevels").gameObject.GetComponent<Text>();
			cnof.text = cf.NrOfLevels.ToString();
			o.GetComponent<Button>().onClick.AddListener(() => this.GetConfig(cn.text));
			o.GetComponent<Button>().onClick.AddListener(() => this.ChooseSelectedButton(o.GetComponent<Button>()));
			o.GetComponent<Button>().onClick.AddListener(() => this.ShowButtons());
            this.ConfigButtons.Add(o);
			i++;
		}
	}

	public void AddConfig()
    {
		GameManager.Instance.oldConfig = GameManager.Instance.GameConfig;
		GameManager.Instance.GameConfig = ConfigFactory.CreateNewConfig();
		GameManager.Instance.fader.LoadSceneFading("config");
	}

	public void RemoveConfig()
	{
        this.removePanel.SetActive(true);
        this.SetInteractableOfAllSceneObjects(false);
        this.blackImage.SetActive(true);
	}

	public void RemoveForSure()
	{
		var configFileToRemove = this.ConfigFiles.Find (x => x.Name == selectedConfigName);
		string path = configFileToRemove.Path;
		this.ConfigFiles.Remove (configFileToRemove);
		bool removed = false;
        foreach (var cb in this.ConfigButtons)
		{
			Text[] texts = cb.GetComponent<Button>().GetComponentsInChildren<Text>();
			foreach(var t in texts)
            {
				if(t.name == "configName")
                {
					if(t.text == selectedConfigName)
                    {
						this.ConfigButtons.Remove(cb);
						GameObject.DestroyImmediate(cb);
						removed = true;
						break;
					}

					break;
				}
			}

		    if (removed)
		    {
		        break;
		    }
		}
		if(removed && path != null)
        {
			if(File.Exists(path))
            {
				File.Delete(path);
                this.EasyLevelChoose();
                this.SetSelection();
			}
		}

        this.removePanel.SetActive(false);
        this.SetInteractableOfAllSceneObjects(true);
        this.blackImage.SetActive(false);
	}

	public void CancelRemoving()
	{
        this.removePanel.SetActive(false);
        this.SetInteractableOfAllSceneObjects(true);
        this.blackImage.SetActive(false);
	}
}
