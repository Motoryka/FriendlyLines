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
	public bool DrawStartPoint { get; set; }
	public int NrOfLevels { get; set; }
	public List<LevelConfig> Levels { get; set; }

    public float WaitingTime { get; set; }

    public Config()
    {
        CreationDate = DateTime.Now;
        Levels = new List<LevelConfig>();
    }
}