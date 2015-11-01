using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;
public class PathAnalyser : IAnalyser {
		
	private float _acceptedError = 0.5f;
	private float _accuracy = 2f;
	private float _finalPointsError = 0.5f;

	public PathAnalyser () {}

	public bool IsFinished(ILine generatedLine, ILine userLine) {

		if (userLine == null || generatedLine == null)
			return false;

		bool isChecked = false;

		GetResult (generatedLine, userLine);
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
		
		if (Vector2.Distance(listG[0], listU[0]) < _finalPointsError &&
		    Vector2.Distance(listG[listG.Length-1], listU[listU.Length-1]) < _finalPointsError) {
			
			return true;
		}
		
		return false;
	}
	
	public float GetResult (ILine generatedLine, ILine userLine) {
		Vector3[] listG = generatedLine.GetVerticles ().ToArray ();
		
		float covUser = GetUserLineCovering (userLine, listG);
		
		float covGen = GetGenLineCovering (generatedLine, userLine);
		
		Debug.Log (covUser + " ...:" + covGen + " == " + (covUser+covGen)/2);
		return (covUser+covGen)/2;
	}
	
	private float GetUserLineCovering(ILine userLine, Vector3[] listG) {
		int correctPoints = 0;
		int wrongPoints = 0;
		
		foreach(Vector3 point in userLine.GetVerticles()) {
			
			float min = 100;
			
			for (int i = 0; i < listG.Length -1; i++) {
				//(x2 - x1)(y - y1) = (y2 - y1)(x - x1)
				
				Vector3 p1 = listG[i];
				Vector3 p2 = listG[i+1];
				
				float left = (p2.x - p1.x)*(point.y - p1.y);
				float right= (p2.y - p1.y)*(point.x - p1.x);
				
				float distA = Vector3.Distance(p1, point);
				float distB = Vector3.Distance(p2, point);
				float distC = Vector3.Distance(p1, p2);
				
				//Debug.Log(distA + " + " + distB + " = " + distC);
				float distCheck = Mathf.Abs((distA + distB) - distC);
				
				float score = Mathf.Abs(left - right);
				if (min > score && distCheck < _acceptedError) {
					min = score;
				}
				//Debug.Log (min);
			}
			
			if (min < _accuracy) {
				correctPoints++;
			}else {
				wrongPoints++;
			}
		}
		//Debug.Log (correctPoints + " " + wrongPoints);
		if (correctPoints + wrongPoints != 0) {
			return (correctPoints*100) / (correctPoints + wrongPoints);
		}
		
		return 0;
	}
	
	private float GetGenLineCovering (ILine generatedLine, ILine userLine) {
		int correctCheckpoints = 0;
		int wrongCheckpoints = 0;
		
		foreach (Vector3 checkpoint in generatedLine.GetVerticles()) {
			bool correct = false;
			
			foreach(Vector3 point in userLine.GetVerticles()) {
				
				float _distance = Vector3.Distance(point, checkpoint);
				
				if (_distance < _acceptedError) {
					correct = true;
					break;
				}
			}
			
			if (correct) {
				correctCheckpoints++;
			}else {
				wrongCheckpoints++;
			}
		}
		
		if (correctCheckpoints + wrongCheckpoints != 0) {
			return (100 * correctCheckpoints) / (correctCheckpoints + wrongCheckpoints);
		}
		
		return 0;
	}
}
