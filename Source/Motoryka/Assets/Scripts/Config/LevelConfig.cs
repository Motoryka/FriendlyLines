using UnityEngine;
using System.Collections;

public class LevelConfig 
{
	public int levelNumber { get; set; }
	public Shape shape { get; set; }
	public float shapeStroke { get; set; }
	public float brushStroke { get; set; }
	public PastelColor shapeColor { get; set; }
	public PastelColor brushColor { get; set; }
	public int difficulty { get; set; }
}
