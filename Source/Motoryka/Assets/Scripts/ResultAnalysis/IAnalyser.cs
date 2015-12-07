/**********************************************************************
Copyright (C) 2015  Magdalena Foks

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


using System.Collections.Generic;

using LineManagement;

using UnityEngine;

public interface IAnalyser
{
	bool IsFinished(ILine generatedLine, List<ILine> userLines);
    Result GetResult(ILine generatedLine, List<ILine> userLines);
    bool IsStartCorrect(Vector3 point, ShapeElement shape, List<ILine> userLines, bool isStartDisplayed = true);

	void SetIsStartDisplayed(bool val);
	void SetAccuracyLevel(AccuracyLevel lvl);
}
