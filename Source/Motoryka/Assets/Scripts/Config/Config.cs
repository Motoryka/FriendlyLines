using System;
using System.Collections.Generic;

//Config test class
public class Config
{
    public Config()
    {
        this.CreationDate = DateTime.Now;
        this.Levels = new List<LevelConfig>();
    }

	public int Id { get; set; }

	public string Name { get; set; }

	public DateTime CreationDate { get; set; }

	public bool DrawStartPoint { get; set; }

	public int NrOfLevels { get; set; }

	public List<LevelConfig> Levels { get; set; }

    public float WaitingTime { get; set; }
    
    public Config Copy()
    {
        var config = new Config
                         {
                             Id = this.Id,
                             Name = this.Name,
                             CreationDate = this.CreationDate,
                             DrawStartPoint = this.DrawStartPoint,
                             NrOfLevels = this.NrOfLevels,
                             Levels = new List<LevelConfig>(),
                             WaitingTime = this.WaitingTime
                         };

        foreach (LevelConfig lc in this.Levels)
        {
            config.Levels.Add(lc.Copy());
        }

        return config;
    }
}