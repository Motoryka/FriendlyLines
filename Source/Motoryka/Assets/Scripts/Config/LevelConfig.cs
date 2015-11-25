using UnityEngine;
using System.Collections;

public class LevelConfig 
{
	public int levelNumber { get; set; }
	public Shape shape { get; set; }
	public float lineStroke { get; set; }
	public PastelColor shapeColor { get; set; }
	public PastelColor brushColor { get; set; }

    public LevelConfig Copy()
    {
        var config = new LevelConfig();

        config.levelNumber = levelNumber;
        config.shapeColor = shapeColor;
		config.lineStroke = lineStroke;
        config.brushColor = brushColor;
        config.shape = shape;

        return config;
    }
}
