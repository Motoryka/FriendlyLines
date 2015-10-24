using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;
public class PathAnalyser : IAnalyser {
		
	private float _acceptedError = 0.5f;

	public PathAnalyser () {}

	public bool IsFinished(ILine generatedLine, ILine userLine) {

		if (userLine == null || generatedLine == null)
			return false;

		bool isChecked = false;

		foreach (Vector2 checkpoint in generatedLine.GetVertices2()) {

            foreach (Vector2 point in userLine.GetVertices2())
            {

                float _distance = Vector2.Distance(point, checkpoint);

				if (_distance < _acceptedError) {
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
