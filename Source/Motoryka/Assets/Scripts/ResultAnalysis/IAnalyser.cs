using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;

public interface IAnalyser {
	bool IsFinished(ILine generatedLine, ILine userLine);
	float GetResult (ILine generatedLine, ILine userLine);
	bool IsStartCorrect(Vector3 point, ILine generatedLine);
}
