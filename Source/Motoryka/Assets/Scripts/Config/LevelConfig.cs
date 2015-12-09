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

public class LevelConfig 
{
	public int levelNumber { get; set; }

	public Shape shape { get; set; }

	public float lineStroke { get; set; }

	public PastelColor shapeColor { get; set; }

	public PastelColor brushColor { get; set; }

    public LevelConfig Copy()
    {
        var config = new LevelConfig
                         {
                             levelNumber = this.levelNumber,
                             shapeColor = this.shapeColor,
                             lineStroke = this.lineStroke,
                             brushColor = this.brushColor,
                             shape = this.shape
                         };

        return config;
    }
}
