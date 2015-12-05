using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void GoToConfig()
    {
        GameManager.Instance.GoToConfig();
    }
}
