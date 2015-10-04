using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
    public Fader fader;

	// Use this for initialization
	void Start () {
        /* We want this to persist through game life */
        DontDestroyOnLoad(this);

        Debug.Log("Game started");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
