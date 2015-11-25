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

    public Config Copy()
    {
        var config = new Config();

        config.Id = Id;
        config.Name = Name;
        config.CreationDate = CreationDate;
        config.DrawStartPoint = DrawStartPoint;
        config.NrOfLevels = NrOfLevels;
        config.Levels = new List<LevelConfig>();
        config.WaitingTime = WaitingTime;

        foreach (LevelConfig lc in Levels)
        {
            config.Levels.Add(lc.Copy());
        }

        return config;
    }
}