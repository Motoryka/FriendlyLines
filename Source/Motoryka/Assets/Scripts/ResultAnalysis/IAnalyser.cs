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
