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
				new LevelConfig { levelNumber = 1, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = PastelColorFactory.Blue.Color, brushColor = PastelColorFactory.LightBlue.Color, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = PastelColorFactory.Yellow.Color, brushColor = PastelColorFactory.LightRed.Color, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.DiagonalLine, shapeStroke = LineStroke.Thick, brushStroke = LineStroke.Thick, shapeColor = PastelColorFactory.Green.Color, brushColor = PastelColorFactory.Gray.Color, difficulty = 2 }
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
				new LevelConfig { levelNumber = 2, shape = Shape.Circle, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = PastelColorFactory.DarkBlue.Color, brushColor = PastelColorFactory.LightPink.Color, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.Ellipse, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Purple.Color, brushColor = PastelColorFactory.LightGreen.Color, difficulty = 2 }
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
				new LevelConfig { levelNumber = 1, shape = Shape.Triangle, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = PastelColorFactory.Pink.Color, brushColor = PastelColorFactory.Yellow.Color, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.Square, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Mint.Color, brushColor = PastelColorFactory.Orange.Color, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.Rectangle, shapeStroke = LineStroke.VeryThin, brushStroke = LineStroke.VeryThin, shapeColor = PastelColorFactory.LightBlue.Color, brushColor = PastelColorFactory.Orange.Color, difficulty = 2 }
			}
		};
		return config;
	}
}
