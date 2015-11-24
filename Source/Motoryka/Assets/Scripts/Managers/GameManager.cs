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
    public AudioSource restartSound;

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
        restartSound = sounds[4];

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

    private void _finishedLevel(bool skipWaiting = false)
    {
        Debug.Log("Level " + CurrentLevel + " finished.");
        _previousShapeVertices = null;

        _currentLevel++;

        float time = 4f;

        if (skipWaiting)
            time = 0f;

        if (CurrentLevel <= _config.NrOfLevels)
            fader.LoadSceneFadingAfterTime(sceneName, new WaitForSeconds(time));
        else
        {
            fader.LoadSceneFadingAfterTime(finishSceneName, new WaitForSeconds(time));

            fader.LoadSceneFadingAfterTime(resultSceneName, new WaitForSeconds(time + 3f));
        }
    }

    public void FinishedLevel()
    {
        _finishedLevel();
    }

    public void StartedLevel()
    {
        Debug.Log("Level " + CurrentLevel + " started.");
    }

    public void RestartLevel(ShapeElement currentShape)
    {
        restartSound.Play();

		_previousShapeVertices = currentShape;

        Debug.Log("Level " + CurrentLevel + " restarted.");

        fader.LoadSceneFadingAfterTime(sceneName, new WaitForSeconds(1f));
    }

    public void BackGame()
    {
        fader.FinishGame(titleSceneName, null);
    }

    public void NextLevel()
    {
        _finishedLevel(true);
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
