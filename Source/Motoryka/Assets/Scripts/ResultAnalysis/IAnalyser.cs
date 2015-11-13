using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;

public interface IAnalyser {
	bool IsFinished(ILine generatedLine, List<ILine> userLines);
	float GetResult (ILine generatedLine, List<ILine> userLines);
	bool IsStartCorrect(Vector3 point, ShapeElement shape);
	void SetIsStartDisplayed(bool val);
}
