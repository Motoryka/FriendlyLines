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

		if (AreFinalPointsCorrect(generatedLine, userLine)) {
			foreach (Vector2 checkpoint in generatedLine.GetVertices2()) {

				foreach(Vector2 point in userLine.GetVertices2()) {

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

		return false;
	}

	private bool AreFinalPointsCorrect (ILine generatedLine, ILine userLine) {
		Vector2[] listG = generatedLine.GetVertices2().ToArray ();
		Vector2[] listU = userLine.GetVertices2().ToArray ();
		
		if (listG.Length == 0 || listU.Length == 0)
			return false;
		
		if (Vector2.Distance(listG[0], listU[0]) < _acceptedError &&
		    Vector2.Distance(listG[listG.Length-1], listU[listU.Length-1]) < _acceptedError) {
			
			return true;
		}
		
		return false;
	}
}
