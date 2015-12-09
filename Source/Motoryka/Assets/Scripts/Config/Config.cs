/**********************************************************************
Copyright (C) 2015  Mateusz Nojek

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

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