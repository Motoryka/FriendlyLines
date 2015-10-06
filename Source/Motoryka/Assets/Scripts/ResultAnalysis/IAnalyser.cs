using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAnalyser {
	bool IsFinished(List<Vector3> checkpoints, Line line);
}
