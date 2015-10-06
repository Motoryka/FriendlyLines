using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class PathAnalyser : IAnalyser {
		
	public PathAnalyser () {}

	public bool IsFinished(List<Vector3> checkpoints, Line line) {

		if (line == null)
			return false;

		bool isChecked = false;

		foreach (Vector3 checkpoint in checkpoints) {

			foreach(Vector3 point in line.GetVerticles()) {

				float x = Mathf.Abs(point.x - checkpoint.x);
				float y = Mathf.Abs(point.y - checkpoint.y);

				if (x < 0.1f && y < 0.1f) {
					isChecked = true;
					break;
				}
			}

			if (!isChecked) {
				return false;
			}
			isChecked = false;
		}

		return true;
	}
}
