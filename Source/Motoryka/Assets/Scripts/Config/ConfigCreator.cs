using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class ConfigCreator : MonoBehaviour {
    public delegate void LevelAddHandler(int atIndex);
    public event LevelAddHandler LevelAdded;

    public delegate void LevelDeleteHandler(int atIndex);
    public event LevelDeleteHandler LevelDeleted;


    public GameObject canvas;
    public GameObject uiLevelManagerPrefab;
    public UIConfigManager configManager;

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
        activeLevelManager = 0;
        var gconfig = GameManager.Instance.GameConfig;

        config = gconfig.Copy();

        _levelManagers = new List<UILevelManager>();
        int i = 0;

        configManager.Init(config);

        foreach(LevelConfig l in config.Levels)
        {
            _levelManagers.Add( InstantiateLevelManager(l, i) );
            i++;
        }


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    UILevelManager InstantiateLevelManager(LevelConfig l, int i)
    {
        GameObject o = Instantiate<GameObject>(uiLevelManagerPrefab);
        o.GetComponent<RectTransform>().SetParent(canvas.GetComponent<RectTransform>(), false);

        SetPosition(o, i);

        UILevelManager mng = o.GetComponent<UILevelManager>();

        mng.Init(this, l, i, config.Levels.Count - 1);

        return mng;
    }

    public void SetPosition(GameObject o, int i)
    {
        o.transform.localPosition = nextPoint * (i - activeLevelManager);
        SetStayingPosition(o.GetComponent<UILevelManager>(), i);
    }

    void SetStayingPosition(UILevelManager o, int i)
    {
        o.GetComponent<UILevelManager>().StayingPoint = nextPoint * (i - activeLevelManager);
    }

    public void AddLevel(int pos)
    {
		var level = new LevelConfig { levelNumber = pos + 1, shape = Shape.HorizontalLine, lineStroke = LineStroke.Medium, shapeColor = PastelColorFactory.RandomColor, brushColor = PastelColorFactory.RandomColor };
        
        config.Levels.Insert(pos, level);

        config.NrOfLevels++;

        OnLevelAdded(pos);

        _levelManagers.Insert(pos, InstantiateLevelManager(level, pos) );

        int from = 0;
        int to = 0;

        if (pos <= activeLevelManager)
        {
            activeLevelManager++;
            from = 0;
            to = activeLevelManager;

            config.Levels[activeLevelManager].levelNumber = activeLevelManager + 1;
            _levelManagers[activeLevelManager].UpdateTitle();
        }
        else
        {
            from = pos + 1;
            to = _levelManagers.Count;
        }

        for (int i = from; i < to; ++i)
        {
            SetPosition(_levelManagers[i].gameObject, i);
        }

        for (int i = pos; i < config.Levels.Count;  ++i)
        {
            config.Levels[i].levelNumber = i + 1;
            _levelManagers[i].UpdateTitle();
        }

        if (pos <= activeLevelManager)
        {
            SetActiveLevelPrevious();
        }
        else
        {
            SetActiveLevelNext();
        }
    }

	public void RemoveLevel(int pos)
	{
		if(config.NrOfLevels > 1)
		{
            config.Levels.RemoveAt(pos);
            _levelManagers.RemoveAt(pos);
			config.NrOfLevels--;
			if (activeLevelManager >= _levelManagers.Count)
			{
				activeLevelManager--;
			}

            for(int i = 0; i < _levelManagers.Count; ++i)
            {
                Debug.Log("Pos: " + pos+ " Cur i : " + i + " active: " + activeLevelManager);
                SetStayingPosition(_levelManagers[i], i);
                config.Levels[i].levelNumber = i + 1;
                _levelManagers[i].UpdateTitle();
            }

            OnLevelRemoved(pos);
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
        Debug.Log("SaveConfig");
		config.Id = SetUniqueId();
		ConfigLoader.SerializeConfig(config, config.Id.ToString());
        GameManager.Instance.GameConfig = config;
        GameManager.Instance.fader.LoadSceneFading("configChoice");
		ConfigChooser.selectedConfigName = config.Name;
    }

	public void SaveAndReplaceConfig(int configId)
	{
		config.Id = configId;
		ConfigLoader.SerializeConfig(config, configId.ToString());
		GameManager.Instance.GameConfig = config;
		GameManager.Instance.fader.LoadSceneFading("configChoice");
		ConfigChooser.selectedConfigName = config.Name;
	}

    public void Cancel()
    {
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

    void OnLevelAdded(int atIndex)
    {
        if (LevelAdded != null)
            LevelAdded(atIndex); 
    }

    void OnLevelRemoved(int atIndex)
    {
        if (LevelDeleted != null)
            LevelDeleted(atIndex);
    }
}
