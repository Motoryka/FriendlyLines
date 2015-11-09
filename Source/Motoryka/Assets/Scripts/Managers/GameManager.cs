using UnityEngine;
using System.Collections;
using LineManagement;
using LineManagement.GLLines;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>, IInitable {
    public Fader fader;
    public string CurrentScene
    {
        get
        {
            return sceneName;
        }
    }
    public string NextScene
    {
        get
        {
            return sceneName;
        }
    }

    int _currentLevel = 1;
    public Config _config;
    bool initialized = false;

    string sceneName = "level";
    string finishSceneName = "end";
    string titleSceneName = "title";

	ShapeElement _previousShapeVertices = null;

	// Use this for initialization
    public void Initialize()
    {
        /* We want this to persist through game life */
        DontDestroyOnLoad(this);

		Config conf = ConfigFactory.CreateHardLevel();
		ConfigLoader.SerializeConfig(conf, "config");
        _config = ConfigLoader.DeserializeConfig("config");
        
        fader = GetComponent<Fader>();

        if(fader == null)
        {
            fader = gameObject.AddComponent<Fader>();
        }

        if (_config == null)
        {
            _config = ConfigFactory.CreateHardLevel();
        }

        Debug.Log("Game started");
        initialized = true;
    }

    void Start()
    {
        if (!initialized)
            Initialize();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit(); 
	}

    public Shape GetCurrentShape()
    {
		return _config.Levels[CurrentLevel - 1].shape;
    }

    public ShapeElement GetPreviousShapeVertices()
    {
        return _previousShapeVertices;
    }

    public void FinishedLevel()
    {
        Debug.Log("Level " + CurrentLevel + " finished.");
        _previousShapeVertices = null;

        _currentLevel++;

        if (CurrentLevel <= _config.NrOfLevels)
            fader.LoadSceneFadingAfterTime(sceneName, new WaitForSeconds(5f));
        else
        {
            fader.LoadSceneFadingAfterTime(finishSceneName, new WaitForSeconds(1f));

            fader.FinishGame(titleSceneName, new WaitForSeconds(5f));
        }
    }

    public void StartedLevel()
    {
        Debug.Log("Level " + CurrentLevel + " started.");
    }

    public void RestartLevel(ShapeElement currentShape)
    {
		_previousShapeVertices = currentShape;

        Debug.Log("Level " + CurrentLevel + " restarted.");

        fader.LoadSceneFadingAfterTime(sceneName, new WaitForSeconds(1f));
    }

    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
    }
}
