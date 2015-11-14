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
	
	List<ConfigFile> ConfigFiles;
	public GameObject configButton;
	List<GameObject> ConfigButtons;
	public Config config;
	public GameObject canvas;

	// Use this for initialization
	void Start () {
		ConfigFiles = new List<ConfigFile>();
		ConfigButtons = new List<GameObject>();
		StoreAllConfigs();
		CreateConfigButtons();
	}
	
	// Update is called once per frame
	void Update () {
	
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
					ConfigFiles.Add(new ConfigFile { Path = Path.GetFileNameWithoutExtension(file), Name = name, NrOfLevels = nrOfLevels });
					break;
				}
			}
			reader.Close();
		}
	}

	public void GetConfig(Text name)
	{
		ConfigFile cf = this.ConfigFiles.Find(x => x.Name == name.text);
		if(cf == null) return;
		this.config =  ConfigLoader.DeserializeConfig(cf.Path);
		GameManager.Instance.fader.LoadSceneFading("level");
	}

	public void EasyLevelRun()
	{
		this.config = ConfigFactory.CreateEasyLevel();
		GameManager.Instance.fader.LoadSceneFading("level");
	}

	public void MediumLevelRun()
	{
		this.config = ConfigFactory.CreateMediumLevel();
		GameManager.Instance.fader.LoadSceneFading("level");
	}

	public void HardLevelRun()
	{
		this.config = ConfigFactory.CreateHardLevel();
		GameManager.Instance.fader.LoadSceneFading("level");
	}

	public void CreateConfigButtons()
	{
		int i = 0;
		foreach(var cf in ConfigFiles)
		{
			GameObject o = Instantiate<GameObject>(configButton);
			//GameObject o = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ConfigButton"));
			o.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>(), false);
			o.transform.localPosition = new Vector3(0f, i * (-30f));
			Text cn = o.GetComponentInChildren<Transform>().Find("configName").gameObject.GetComponent<Text>();
			cn.text = cf.Name;
			Text cnof = o.GetComponentInChildren<Transform>().Find("numberOfLevels").gameObject.GetComponent<Text>();
			cnof.text = cf.NrOfLevels.ToString();
			ConfigButtons.Add(o);
			i++;
		}
	}
}
