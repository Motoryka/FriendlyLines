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

using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{

    public GameObject LevelChooserDropdown;
    public GameObject ShapeText;
    public GameObject LineStrokeText;
    public GameObject LineColorText;
    public GameObject ShapeColorText;
    public GameObject LevelResultText;
    public GameObject LevelResultIncorrectText;
    public GameObject DrawTimeoutText;
    public GameObject ResultText;
    public GameObject ResultIncorrectText;
    public GameObject StartPointImg;
    public Sprite checkYes;
    public Sprite checkNo;

    private Result finalResult;

    public void EndButtonClick()
    {
		GameManager.Instance.fader.FinishGame("title", new WaitForSeconds(0f));
    }

	// Use this for initialization
	void Start()
    {
        for(int i = 1; i <= GameManager.Instance.GameConfig.NrOfLevels; i++)
        {
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
	void Update()
    {
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
		DrawTimeoutText.GetComponent<Text>().text = GameManager.Instance.GameConfig.WaitingTime.ToString() + " s";
        ResultText.GetComponent<Text>().text = this.finalResult.shapeCovering + " %";
        ResultIncorrectText.GetComponent<Text>().text = this.finalResult.errorRange + " %";
        if (GameManager.Instance.GameConfig.DrawStartPoint)
        {
            StartPointImg.GetComponent<Image>().sprite = checkYes;
        }
        else
        {
            StartPointImg.GetComponent<Image>().sprite = checkNo;
        }
    }
}
