using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class PathAnalyser : IAnalyser {
		
	private float _acceptedError = 0.15f;

	public PathAnalyser () {}

	public bool IsFinished(ILine generatedLine, ILine userLine) {

		if (userLine == null || generatedLine == null)
			return false;

		bool isChecked = false;

		foreach (Vector3 checkpoint in generatedLine.GetVerticles()) {

			foreach(Vector3 point in userLine.GetVerticles()) {

				float _distance = Vector3.Distance(point, checkpoint);

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
