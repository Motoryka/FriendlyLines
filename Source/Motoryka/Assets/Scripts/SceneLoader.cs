using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class SceneLoader : MonoBehaviour {

	public Text LevelNoLabel;
	private int NoOfLevels;
	private Config config;

	// For tests to check OnClick button events
	public void OnLevelButtonCLick()
	{
		int levelNumber;
		int.TryParse (this.LevelNoLabel.text, out levelNumber);
		// if there's not the last level
		if (levelNumber < this.NoOfLevels) {
			levelNumber++;
			this.LevelNoLabel.text = levelNumber.ToString ();
		//if there's the last level, save config to file
		} else if (levelNumber == this.NoOfLevels) {
			ConfigLoader.SerializeConfig(this.config, "config");
		}
	}

	public void ChangeToScene(int sceneIndex){
		Application.LoadLevel (sceneIndex);
	}

	public void ChangeToScene(string sceneName){
		Application.LoadLevel (sceneName);
	}

	public void CloseApp()
	{
		Application.Quit ();
	}

	// Use this for initialization
	void Start () {
		//this.LevelNoLabel = gameObject.GetComponent ("LevelNo") as Text;
		this.LevelNoLabel.text = "0";

		// create level from factory
		//this.config = ConfigFactory.CreateHardLevel ();

		// deserialize (load) config on start from file
		this.config = ConfigLoader.DeserializeConfig ("config.xml");
		this.NoOfLevels = config.NrOfLevels;
	}
}
