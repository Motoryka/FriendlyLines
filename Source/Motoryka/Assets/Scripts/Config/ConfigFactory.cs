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
			Id = 1, 
			Name = "Easy Level",
			CreationDate = DateTime.Now, 
			DrawStartPoint = true,
			NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = PastelColorFactory.Blue, brushColor = PastelColorFactory.LightBlue, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.HorizontalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = PastelColorFactory.Yellow, brushColor = PastelColorFactory.LightRed, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.DiagonalLine, shapeStroke = LineStroke.Thick, brushStroke = LineStroke.Thick, shapeColor = PastelColorFactory.Green, brushColor = PastelColorFactory.Gray, difficulty = 2 }
			}
		};
		return config;
	}

	public static Config CreateMediumLevel()
	{
		Config config = new Config { 
			Id = 2, 
			Name = "Medium Level",
			CreationDate = DateTime.Now,
			DrawStartPoint = true,
			NrOfLevels = 9,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.VerticalLine, shapeStroke = LineStroke.VeryThick, brushStroke = LineStroke.VeryThick, shapeColor = PastelColorFactory.LightYellow, brushColor = PastelColorFactory.LightBlue, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.HorizontalLine, shapeStroke = LineStroke.Thick, brushStroke = LineStroke.Thick, shapeColor = PastelColorFactory.DarkBlue, brushColor = PastelColorFactory.LightPink, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.DiagonalLine, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = PastelColorFactory.Purple, brushColor = PastelColorFactory.LightGreen, difficulty = 2 },
				new LevelConfig { levelNumber = 4, shape = Shape.CurvedLine, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Green, brushColor = PastelColorFactory.DarkGray, difficulty = 2 },
				new LevelConfig { levelNumber = 5, shape = Shape.Circle, shapeStroke = LineStroke.VeryThin, brushStroke = LineStroke.VeryThin, shapeColor = PastelColorFactory.Blue, brushColor = PastelColorFactory.LightRed, difficulty = 2 },
				new LevelConfig { levelNumber = 6, shape = Shape.Ellipse, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Mint, brushColor = PastelColorFactory.Black, difficulty = 2 },
				new LevelConfig { levelNumber = 7, shape = Shape.Triangle, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Orange, brushColor = PastelColorFactory.Yellow, difficulty = 2 },
				new LevelConfig { levelNumber = 8, shape = Shape.Square, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.LightBlue, brushColor = PastelColorFactory.LightRed, difficulty = 2 },
				new LevelConfig { levelNumber = 9, shape = Shape.Rectangle, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Purple, brushColor = PastelColorFactory.LightGreen, difficulty = 2 }
			}
		};
		return config;
	}

	public static Config CreateHardLevel()
	{
		Config config = new Config { 
			Id = 3, 
			Name = "Hard Level",
			CreationDate = DateTime.Now,
			DrawStartPoint = false,
            NrOfLevels = 3,
			Levels = new List<LevelConfig> {
				new LevelConfig { levelNumber = 1, shape = Shape.Triangle, shapeStroke = LineStroke.Medium, brushStroke = LineStroke.Medium, shapeColor = PastelColorFactory.Pink, brushColor = PastelColorFactory.Yellow, difficulty = 2 },
				new LevelConfig { levelNumber = 2, shape = Shape.Square, shapeStroke = LineStroke.Thin, brushStroke = LineStroke.Thin, shapeColor = PastelColorFactory.Mint, brushColor = PastelColorFactory.Orange, difficulty = 2 },
				new LevelConfig { levelNumber = 3, shape = Shape.Rectangle, shapeStroke = LineStroke.VeryThin, brushStroke = LineStroke.VeryThin, shapeColor = PastelColorFactory.LightBlue, brushColor = PastelColorFactory.Orange, difficulty = 2 }
			}
		};
		return config;
	}
}
