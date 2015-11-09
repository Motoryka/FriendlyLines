using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;

public class ConfigFactory 
{

	public static Config CreateEasyLevel()
	{
		Config config = new Config { 
			Id = Random.Range(0, 1000), 
			Name = "Easy Level",
			CreationDate = DateTime.Now, 
			DrawStartPoint = true,
			NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = Color.yellow, brushColor = Color.red, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.DiagonalLine, shapeStroke = LineStroke.Thick, brushStroke = LineStroke.Thick, shapeColor = Color.green, brushColor = Color.gray, difficulty = 2 }
			}
		};
		return config;
	}

	public static Config CreateMediumLevel()
	{
		Config config = new Config { 
			Id = Random.Range(0, 1000), 
			Name = "Medium Level",
			CreationDate = DateTime.Now,
			DrawStartPoint = true,
			NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.CurvedLine, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = Color.magenta, brushColor = Color.black, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.Circle, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.Ellipse, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = Color.yellow, brushColor = Color.green, difficulty = 2 }
			}
		};
		return config;
	}

	public static Config CreateHardLevel()
	{
		Config config = new Config { 
			Id = Random.Range(0, 1000), 
			Name = "Hard Level",
			CreationDate = DateTime.Now,
			DrawStartPoint = false,
            NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.Triangle, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = Color.yellow, brushColor = Color.red, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.Square, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.Rectangle, shapeStroke = LineStroke.VeryThin, brushStroke = LineStroke.VeryThin, shapeColor = Color.gray, brushColor = Color.black, difficulty = 2 }
			}
		};
		return config;
	}
}
