using UnityEngine;
using System.Collections;

public class GameManagerTemp : MonoBehaviour {
    public TitleScript titleSceneScript;

	// Use this for initialization
	void Start () {
        titleSceneScript = GameObject.Find("SceneManager").GetComponent<TitleScript>();
        titleSceneScript.SendMessage("LoadLevelAfterTime", 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
