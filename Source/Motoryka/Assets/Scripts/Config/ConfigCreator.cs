using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class ConfigCreator : MonoBehaviour {
    public GameObject canvas;
    public GameObject uiLevelManagerPrefab;

    Config config;
    List<UILevelManager> _levelManagers;
    int activeLevelManager;

    Vector3 previousPoint = new Vector3(-Screen.width, 0f, 0f);
    //Vector3 activePoint = Vector3.zero;
    Vector3 nextPoint = new Vector3(Screen.width, 0f, 0f);

	public T GetUIElement<T>(string name)
	{
		var go = GameObject.Find (name);
		var element = go.GetComponent<T>();
		return (T) element;
	}

	// Use this for initialization
	void Start () {
        config = GameManager.Instance.GameConfig;

        _levelManagers = new List<UILevelManager>();
        int i = 0;


        foreach(LevelConfig l in config.Levels)
        {
            GameObject o = Instantiate<GameObject>(uiLevelManagerPrefab);
            o.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>(), false);
            //o.transform.SetParent(canvas.transform);
            o.transform.localPosition = nextPoint * i;
            //o.GetComponent<RectTransform>().localPosition = nextPoint * i;
            /*
            if (i == 0)
                o.transform.localPosition = activePoint;
            else
                o.transform.localPosition = nextPoint;*/
            UILevelManager mng = o.GetComponent<UILevelManager>();

            mng.Init(this, l);

            _levelManagers.Add(mng);
            i++;
        }

        activeLevelManager = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddLevel()
    {
        var level = new LevelConfig { levelNumber = ++config.NrOfLevels, shape = Shape.VerticalLine, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = PastelColorFactory.DarkBlue, brushColor = PastelColorFactory.DarkGreen, difficulty = 2 };
        config.Levels.Add(level);
    }

	public void RemoveLevel()
	{
		if(config.NrOfLevels > 1)
		{
			config.Levels.RemoveAt(activeLevelManager-1);
			config.NrOfLevels--;
			if (activeLevelManager < _levelManagers.Count -1)
			{
				activeLevelManager++;
			}
			else
			{
				activeLevelManager--;
			}
		}
	}

    public void SetActiveLevelNext()
    {
        if (activeLevelManager < _levelManagers.Count -1)
        {
            foreach (var mng in _levelManagers)
            {
                mng.MoveToPoint(mng.StayingPoint + previousPoint);
            }
            activeLevelManager++;
        /*
            _levelManagers[activeLevelManager].MoveToPoint(previousPoint);
            activeLevelManager++;
            _levelManagers[activeLevelManager].MoveToPoint(activePoint);*/
        }
    }

    public void SetActiveLevelPrevious()
    {
        if (activeLevelManager > 0)
        {

            foreach (var mng in _levelManagers)
            {
                mng.MoveToPoint(mng.StayingPoint + nextPoint);
            }
            activeLevelManager--;
            /*
            _levelManagers[activeLevelManager].MoveToPoint(nextPoint);
            activeLevelManager--;
            _levelManagers[activeLevelManager].MoveToPoint(activePoint);*/
        }
    }

    public void SaveConfig()
    {
		ConfigLoader.SerializeConfig(config, config.Id.ToString());
        GameManager.Instance.fader.LoadSceneFading("configChoice");
    }
    
	public void SaveAsNewConfig()
	{
		config.Id = SetUniqueId();
		ConfigLoader.SerializeConfig(config, config.Id.ToString());
		GameManager.Instance.fader.LoadSceneFading("configChoice");
	}

	private int SetUniqueId()
	{
		List<int> ids = new List<int>();
		string[] configFiles = Directory.GetFiles(Application.persistentDataPath + "/configs/");
		foreach(var file in configFiles)
		{
			int id = 0;
			XmlTextReader reader = new XmlTextReader(file);
			while(reader.Read ())
			{
				reader.MoveToContent();
				if(reader.NodeType == XmlNodeType.Element && reader.Name == "Id")
				{
					reader.Read();
					int.TryParse(reader.Value, out id);
					ids.Add(id);
					break;
				}
			}
			reader.Close();
		}

		ids.Sort((a,b) => a.CompareTo(b));
		int minId = 0;
		while(true){
			minId++;
			if(ids.IndexOf(minId) == -1)
				return minId;
		}
		return ++minId;
	}
}
