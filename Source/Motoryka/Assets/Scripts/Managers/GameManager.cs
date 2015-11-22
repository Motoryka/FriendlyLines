using UnityEngine;
using System.Collections;
using LineManagement;
using LineManagement.GLLines;
using System.Collections.Generic;
using System.IO;

public class GameManager : Singleton<GameManager>, IInitable {
    public Fader fader;
    public AudioSource backMusic;
    public AudioSource titleMusic;
    public AudioSource levelFinishedSound;
    public AudioSource gameFinishedSound;

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

    int _currentLevel;
    public Config _config = null;
    bool initialized = false;

    string sceneName = "level";
    string finishSceneName = "end";
    string titleSceneName = "title";
    string configChoiceSceneName = "configChoice";
	string resultSceneName = "result";

	public List<LevelResult> ResultsList;

	ShapeElement _previousShapeVertices = null;

	public Config GameConfig {
		get{
			return _config;
		}
		set{
			this._config = value;
		}
	}

	// Use this for initialization
    public void Initialize()
    {
        /* We want this to persist through game life */
        DontDestroyOnLoad(this);

		try
		{
			if(!Directory.Exists(Application.persistentDataPath + "/configs"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/configs");
			}
		}
		catch(IOException e)
		{
			Debug.Log(e.Message);
		}

        _currentLevel = 1;

        if(_config == null)
		    _config = ConfigFactory.CreateEasyLevel();
        
        fader = GetComponent<Fader>();

        if(fader == null)
        {
            fader = gameObject.AddComponent<Fader>();
        }
		
        if (_config == null)
        {
            _config = ConfigFactory.CreateHardLevel();
        }

		this.ResultsList = new List<LevelResult>();

        AudioSource[] sounds = GetComponents<AudioSource>();
        backMusic = sounds[0];
        titleMusic = sounds[1];
        gameFinishedSound = sounds[2];
        levelFinishedSound = sounds[3];

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
        {
            levelFinishedSound.Play();
            fader.LoadSceneFadingAfterTime(sceneName, new WaitForSeconds(4f));
        }
        else
        {
            gameFinishedSound.Play();
            fader.LoadSceneFadingAfterTime(finishSceneName, new WaitForSeconds(4f));

			fader.LoadSceneFadingAfterTime(resultSceneName, new WaitForSeconds(7f));
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

    public void StartGame()
    {
        titleMusic.Stop();
        fader.LoadSceneFading(sceneName);
    }

    public void GoToConfig()
    {
		fader.LoadSceneFading(configChoiceSceneName);
    }

    public void RestartGame()
    {
        Initialize();
    }
}
