using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine.UI;

public class ConfigFile 
{
	public string Path;
	public string Name;
	public int NrOfLevels;
}

public class ConfigChooser : MonoBehaviour {
	
	public List<ConfigFile> ConfigFiles;
	public GameObject configButton;
	List<GameObject> ConfigButtons;
	public Config config;
	public GameObject canvas;
	public GameObject editButton;
	private ConfigCreator cc;
	public static string selectedConfigName;
	public Transform ContentPanel;

	// Use this for initialization
	void Start () {
		ConfigFiles = new List<ConfigFile>();
		ConfigButtons = new List<GameObject>();
		SetSelection();
		StoreAllConfigs();
		CreateConfigButtons();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackToTitle()
	{
		GameManager.Instance.fader.LoadSceneFading("title");
	}

	public void EditConfig()
	{
		GameManager.Instance.fader.LoadSceneFading("config");
	}

	public void ShowEditButton()
	{
		this.editButton.SetActive(true);
	}

	public void HideEditButton()
	{
		this.editButton.SetActive(false);
	}

	private void SetSelection()
	{
		switch(GameManager.Instance.GameConfig.Name){
		case "Easy Level":
			ChangeButtonColor("EasyLevelButton");
			break;
		case "Medium Level":
			ChangeButtonColor("MediumLevelButton");
			break;
		case "Hard Level":
			ChangeButtonColor("HardLevelButton");
			break;
		default:
			break;
		}
	}

	private void ChangeButtonColor(string buttonName)
	{
		Transform[] objs = canvas.GetComponentsInChildren<Transform>();
		foreach(var o in objs){
			var btn = o.transform.GetComponent<Button>();
			if(btn != null){
				if(btn.name == buttonName){
					btn.image.color = PastelColorFactory.Mint.Color;
				}
			}
		}
	}

	public void StoreAllConfigs()
	{
		string[] configFiles = Directory.GetFiles(Application.persistentDataPath + "/configs/");
		foreach(var file in configFiles)
		{
			string name = "";
			int nrOfLevels = 0;
			XmlTextReader reader = new XmlTextReader(file);
			while(reader.Read ())
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
					this.ConfigFiles.Add(new ConfigFile { Path = Path.GetFileNameWithoutExtension(file), Name = name, NrOfLevels = nrOfLevels });
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
		GameManager.Instance.GameConfig = ConfigLoader.DeserializeConfig(cf.Path);
		selectedConfigName = name;
	}

	public void EasyLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateEasyLevel();
	}

	public void MediumLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateMediumLevel();
	}

	public void HardLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateHardLevel();
	}

	public void ChooseSelectedButton(Button button)
	{
		RemoveSelectionOnButton();
		button.image.color = PastelColorFactory.Mint.Color;
	}

	private void RemoveSelectionOnButton()
	{
		Transform[] objs = canvas.GetComponentsInChildren<Transform>();
		foreach(var o in objs){
			var btn = o.transform.GetComponent<Button>();
			if(btn != null){
				if(btn.image.color == PastelColorFactory.Mint.Color){
					btn.image.color = Color.white;
				}
			}
		}
	}

	public void CreateConfigButtons()
	{
		int i = 0;
		foreach(var cf in this.ConfigFiles)
		{
			GameObject o = Instantiate<GameObject>(configButton);
			//var rect = o.GetComponent<RectTransform>();
			//rect.SetParent(canvas.GetComponent<RectTransform>(), false);
			o.transform.SetParent(ContentPanel);
			//rect.SetParent(ContentPanel, false);
			Text cn = o.GetComponentInChildren<Transform>().Find("configName").gameObject.GetComponent<Text>();
			cn.text = cf.Name;
			if(cf.Name == selectedConfigName){
				o.GetComponent<Button>().image.color = PastelColorFactory.Mint.Color;
			}
			Text cnof = o.GetComponentInChildren<Transform>().Find("numberOfLevels").gameObject.GetComponent<Text>();
			cnof.text = cf.NrOfLevels.ToString();
			o.GetComponent<Button>().onClick.AddListener(() => this.GetConfig(cn.text));
			o.GetComponent<Button>().onClick.AddListener(() => this.ChooseSelectedButton(o.GetComponent<Button>()));
			o.GetComponent<Button>().onClick.AddListener(() => this.ShowEditButton());
			ConfigButtons.Add(o);
			i++;
		}
	}

	public void AddConfig(){
		Config config = ConfigFactory.CreateNewConfig();
		GameManager.Instance.GameConfig = config;
		GameManager.Instance.fader.LoadSceneFading("config");
	}
}
