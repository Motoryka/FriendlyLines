using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//Config test class
public class Config
{
	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime CreationDate { get; set; }
	public int NrOfLevels { get; set; }
	public List<LevelConfig> Levels { get; set; }
}