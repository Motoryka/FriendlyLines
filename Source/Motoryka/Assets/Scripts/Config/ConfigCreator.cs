/**********************************************************************
Copyright (C) 2015  Mateusz Nojek

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;

public class ConfigCreator : MonoBehaviour
{
	public delegate void LevelAddHandler(int atIndex);
	public event LevelAddHandler LevelAdded;

    public delegate void LevelDeleteHandler(int atIndex);
    public event LevelDeleteHandler LevelDeleted;

    public GameObject canvas;
    public GameObject uiLevelManagerPrefab;
    public UIConfigManager configManager;

    public Config config;
    private List<UILevelManager> _levelManagers;
    private int activeLevelManager;

    private Vector3 previousPoint = new Vector3(-Screen.width, 0f, 0f);
    private Vector3 nextPoint = new Vector3(Screen.width, 0f, 0f);

	public T GetUIElement<T>(string name)
	{
		var go = GameObject.Find(name);
		var element = go.GetComponent<T>();
		return (T)element;
	}

	// Use this for initialization
	void Start () 
    {
        this.activeLevelManager = 0;
        var gconfig = GameManager.Instance.GameConfig;

        this.config = gconfig.Copy();

        this._levelManagers = new List<UILevelManager>();
        int i = 0;

        this.configManager.Init(this.config);

        foreach (LevelConfig l in this.config.Levels)
        {
            this._levelManagers.Add(InstantiateLevelManager(l, i));
            i++;
        }
	}
	
	// Update is called once per frame
	void Update()
    {
	}

    UILevelManager InstantiateLevelManager(LevelConfig l, int i)
    {
        GameObject o = Instantiate<GameObject>(uiLevelManagerPrefab);
        o.GetComponent<RectTransform>().SetParent(this.canvas.GetComponent<RectTransform>(), false);

        this.SetPosition(o, i);

        UILevelManager mng = o.GetComponent<UILevelManager>();

        mng.Init(this, l, i, this.config.Levels.Count - 1);

        return mng;
    }

    public void SetPosition(GameObject o, int i)
    {
        o.transform.localPosition = this.nextPoint * (i - this.activeLevelManager);
        this.SetStayingPosition(o.GetComponent<UILevelManager>(), i);
    }

    private void SetStayingPosition(UILevelManager o, int i)
    {
        o.GetComponent<UILevelManager>().StayingPoint = this.nextPoint * (i - this.activeLevelManager);
    }

    public void AddLevel(int pos)
    {
		var level = new LevelConfig { levelNumber = pos + 1, shape = Shape.HorizontalLine, lineStroke = LineStroke.Medium, shapeColor = PastelColorFactory.RandomColor };
		level.brushColor = PastelColorFactory.RandomColorWithExclude(level.shapeColor);
        
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
        }

		foreach(var lvl in _levelManagers)
		{
			lvl.UpdateTitle();
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
		if(GameManager.Instance.oldConfig != null)
        {
			GameManager.Instance.GameConfig = GameManager.Instance.oldConfig;
		}
		else
        {
			GameManager.Instance.GameConfig = ConfigFactory.CreateEasyLevel();
		}

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
			while(reader.Read())
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

		ids.Sort((a, b) => a.CompareTo(b));
		int minId = 0;
		while(true)
        {
			minId++;
            if (ids.IndexOf(minId) == -1)
            {
                return minId;
            }
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
