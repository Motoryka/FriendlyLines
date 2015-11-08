using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;

public class ConfigFactory : MonoBehaviour {

	public static Config CreateEasyLevel()
	{
		Config config = new Config { 
			Id = Random.Range(0, 1000), 
			Name = "Easy Level",
			CreationDate = DateTime.Now, 
			NrOfLevels = 2 ,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.HorizontalLine, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.VerticalLine, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 }
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
			NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.CurvedLine, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.DiagonalLine, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 }
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
            NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.Circle, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.Square, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.Triangle, shapeStroke = 1f, brushStroke = 1f, shapeColor = Color.blue, brushColor = Color.cyan, difficulty = 2 }
			}
		};
		return config;
	}
}
