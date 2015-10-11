using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class SceneLoader : MonoBehaviour {

	public Text LevelNoLabel;
	private int NoOfLevels;

	// For tests to check OnClick button events
	public void OnLevelButtonCLick()
	{
		int levelNumber;
		int.TryParse (this.LevelNoLabel.text, out levelNumber);
		if (levelNumber < this.NoOfLevels) {
			levelNumber++;
			this.LevelNoLabel.text = levelNumber.ToString ();
		} else if (levelNumber == this.NoOfLevels) {
			this.LevelNoLabel.text = ConfigLoader.SaveConfig();
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

		Config config = ConfigFactory.CreateHardLevel ();
		this.NoOfLevels = config.NrOfLevels;
	}
}
