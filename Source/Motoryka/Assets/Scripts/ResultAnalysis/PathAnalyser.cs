using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class PathAnalyser : IAnalyser {
		
	private float _acceptedError = 0.5f;

	public PathAnalyser () {}

	public bool IsFinished(ILine generatedLine, ILine userLine) {

		if (userLine == null || generatedLine == null)
			return false;

		bool isChecked = false;

		if (AreFinalPointsCorrect(generatedLine, userLine)) {
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

		return false;
	}

	private bool AreFinalPointsCorrect (ILine generatedLine, ILine userLine) {
		Vector3[] listG = generatedLine.GetVerticles ().ToArray ();
		Vector3[] listU = userLine.GetVerticles ().ToArray ();
		
		if (listG.Length == 0 || listU.Length == 0)
			return false;
		
		if (Vector3.Distance(listG[0], listU[0]) < _acceptedError &&
		    Vector3.Distance(listG[listG.Length-1], listU[listU.Length-1]) < _acceptedError) {
			
			return true;
		}
		
		return false;
	}
}
