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
	public GameObject removeButton;
	public GameObject removePanel;
	public GameObject blackImage;
	private ConfigCreator cc;
	public static string selectedConfigName = "";
	public Transform ContentPanel;

	// Use this for initialization
	void Start () {
		Init ();

		removePanel = GameObject.Find ("RemovePanel");
		removePanel.SetActive(false);
		blackImage = GameObject.Find ("BlackImage");
		blackImage.SetActive(false);
	}

	private void Init()
	{
		ConfigFiles = new List<ConfigFile>();
		ConfigButtons = new List<GameObject>();
		SetSelection();
		StoreAllConfigs();
		CreateConfigButtons();
		if(selectedConfigName != "") ShowButtons();
		else HideButtons();
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
		switch(GameManager.Instance.GameConfig.Id){
		case -3:
			ChangeButtonColor("EasyLevelButton");
			break;
		case -2:
			ChangeButtonColor("MediumLevelButton");
			break;
		case -1:
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
					if(btn.animator != null){
						btn.animator.SetBool("isChosen", true);
					}
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
		HideButtons ();
	}

	public void MediumLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateMediumLevel();
		HideButtons ();
	}

	public void HardLevelChoose()
	{
		selectedConfigName = "";
		GameManager.Instance.GameConfig = ConfigFactory.CreateHardLevel();
		HideButtons ();
	}

	public void ChooseSelectedButton(Button button)
	{
		RemoveSelectionOnButton();
		if(button.animator != null){
			button.animator.SetBool ("isChosen", true);
		}
	}

	private void RemoveSelectionOnButton()
	{
		Transform[] objs = canvas.GetComponentsInChildren<Transform>();
		foreach(var o in objs){
			var btn = o.transform.GetComponent<Button>();
			if(btn != null){
				if (btn.animator != null){
					btn.animator.SetBool ("isChosen", false);
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
			o.transform.SetParent(ContentPanel);
			Text cn = o.GetComponentInChildren<Transform>().Find("configName").gameObject.GetComponent<Text>();
			cn.text = cf.Name;
			if(cf.Name == selectedConfigName){
				o.GetComponent<Button>().image.color = PastelColorFactory.Mint.Color;
			}
			Text cnof = o.GetComponentInChildren<Transform>().Find("numberOfLevels").gameObject.GetComponent<Text>();
			cnof.text = cf.NrOfLevels.ToString();
			o.GetComponent<Button>().onClick.AddListener(() => this.GetConfig(cn.text));
			o.GetComponent<Button>().onClick.AddListener(() => this.ChooseSelectedButton(o.GetComponent<Button>()));
			o.GetComponent<Button>().onClick.AddListener(() => this.ShowButtons());
			ConfigButtons.Add(o);
			i++;
		}
	}

	public void AddConfig(){
		Config config = ConfigFactory.CreateNewConfig();
		GameManager.Instance.GameConfig = config;
		GameManager.Instance.fader.LoadSceneFading("config");
	}

	public void RemoveConfig()
	{
		removePanel.SetActive(true);
		SetInteractableOfAllSceneObjects(false);
		blackImage.SetActive(true);
	}

	public void RemoveForSure()
	{
		var configFileToRemove = this.ConfigFiles.Find (x => x.Name == selectedConfigName);
		string path = configFileToRemove.Path;
		this.ConfigFiles.Remove (configFileToRemove);
		bool removed = false;
		foreach(var cb in ConfigButtons)
		{
			Text[] texts = cb.GetComponent<Button>().GetComponentsInChildren<Text>();
			foreach(var t in texts){
				if(t.name == "configName"){
					if(t.text == selectedConfigName){
						this.ConfigButtons.Remove (cb);
						GameObject.DestroyImmediate(cb);
						removed = true;
						break;
					}
					break;
				}
			}
			if(removed) break;
		}
		if(removed && path != null){
			if(File.Exists(path)){
				File.Delete(path);
				EasyLevelChoose();
				SetSelection();
			}
		}

		removePanel.SetActive(false);
		SetInteractableOfAllSceneObjects(true);
		blackImage.SetActive(false);
	}

	public void CancelRemoving()
	{
		removePanel.SetActive(false);
		SetInteractableOfAllSceneObjects(true);
		blackImage.SetActive(false);
	}
}
