using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultScene : MonoBehaviour {

    public GameObject LevelChooserDropdown;
    public GameObject ShapeText;
    public GameObject LineStrokeText;
    public GameObject LineColorText;
    public GameObject ShapeColorText;
    public GameObject LevelResultText;
    public GameObject LevelResultIncorrectText;
    public GameObject StartPointToggle;
    public GameObject DrawTimeoutText;
    public GameObject ResultText;
    public GameObject ResultIncorrectText;

    private Result finalResult;

    public void EndButtonClick()
    {
		GameManager.Instance.fader.FinishGame("title", new WaitForSeconds(0f));
    }

	// Use this for initialization
	void Start () {
        for(int i = 1; i <= GameManager.Instance.GameConfig.NrOfLevels; i++){
            LevelChooserDropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(i.ToString()));
        }
        LevelChooserDropdown.GetComponent<Dropdown>().value = 0;

        this.finalResult = new Result(0,0);
        foreach (var res in GameManager.Instance.ResultsList)
        {
            this.finalResult.shapeCovering += res.result.shapeCovering;
            this.finalResult.errorRange += res.result.errorRange;
        }
        this.finalResult.shapeCovering /= GameManager.Instance.GameConfig.NrOfLevels;
        this.finalResult.errorRange /= GameManager.Instance.GameConfig.NrOfLevels;

		OnLevelChange();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnLevelChange()
    {
        int currentLevelNumber = LevelChooserDropdown.GetComponent<Dropdown>().value + 1;
        LevelConfig currentLevel = GameManager.Instance.GameConfig.Levels.Find(x => x.levelNumber == currentLevelNumber);
		ShapeText.GetComponent<Text>().text = ShapeElement.GetShapeName(currentLevel.shape);
		LineStrokeText.GetComponent<Text>().text = LineStroke.FloatToStroke(currentLevel.lineStroke);
        LineColorText.GetComponent<Text>().text = currentLevel.brushColor.Name;
        ShapeColorText.GetComponent<Text>().text = currentLevel.shapeColor.Name;
		LevelResultText.GetComponent<Text>().text = GameManager.Instance.ResultsList.Find(x => x.levelNumber == currentLevelNumber).result.shapeCovering + " %";
        LevelResultIncorrectText.GetComponent<Text>().text = GameManager.Instance.ResultsList.Find(x => x.levelNumber == currentLevelNumber).result.errorRange + " %";
        StartPointToggle.GetComponent<Toggle>().isOn = GameManager.Instance.GameConfig.DrawStartPoint;
		DrawTimeoutText.GetComponent<Text>().text = GameManager.Instance.GameConfig.WaitingTime.ToString();
        ResultText.GetComponent<Text>().text = this.finalResult.shapeCovering + " %";
        ResultIncorrectText.GetComponent<Text>().text = this.finalResult.errorRange + " %";
    }
}
