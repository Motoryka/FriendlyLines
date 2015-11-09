using UnityEngine;
using System.Collections;

public class ConfigCreator : MonoBehaviour {

	public T GetUIElement<T>(string name)
	{
		var go = GameObject.Find (name);
		var element = go.GetComponent<T>();
		return (T) element;
	}
    Config config;

	// Use this for initialization
	void Start () {
		var shapeText = GetUIElement<ComboBox>("ShapeText");
		int shape = shapeText.SelectedOption;
        config = ConfigFactory.CreateEasyLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddLevel()
    {
        var level = new LevelConfig { levelNumber = config.NrOfLevels + 1, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 };
        config.Levels.Add(level);
    }
}
