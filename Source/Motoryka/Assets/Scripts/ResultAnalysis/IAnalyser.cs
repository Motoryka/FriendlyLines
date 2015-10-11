using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAnalyser {
	bool IsFinished(ILine generatedLine, ILine userLine);
}
