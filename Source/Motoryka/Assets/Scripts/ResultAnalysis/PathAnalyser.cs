using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;

public enum AccuracyLevel {
	Easy,
	Medium,
	Hard
}

public class PathAnalyser : IAnalyser {
		
	private Dictionary<AccuracyLevel, float> levelMap = new Dictionary<AccuracyLevel, float> () 
	{
		{AccuracyLevel.Easy, 2},
		{AccuracyLevel.Medium, 1.5f},
		{AccuracyLevel.Hard, 1}
	};

	private AccuracyLevel level = AccuracyLevel.Medium;
	private float _acceptedError = 0.4f;
	private float _accuracy = 2f;
	private float _finalPointsError = 0.5f;

	public PathAnalyser () {
	}

	public PathAnalyser (AccuracyLevel level) {
		this.level = level;
	}

	public bool IsFinished(ILine generatedLine, ILine userLine) {

		if (userLine == null || generatedLine == null)
			return false;

		bool isChecked = false;

		foreach (Vector2 checkpoint in generatedLine.GetVertices2()) {

			foreach(Vector2 point in userLine.GetVertices2()) {

				float _distance = Vector2.Distance(point, checkpoint);

				if (_distance < _acceptedError*levelMap[level]) {
					isChecked = true;
					break;
				}
			}

			if (!isChecked) {
				return false;
			}
			isChecked = false;
		}

		int size = generatedLine.GetVertices2 ().Count;

		if (size > 0 && generatedLine.GetVertices2 () [0] == generatedLine.GetVertices2 () [size-1]) {
			return AreFinalPointsCorrect(generatedLine, userLine);
		}

		return true;
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
		Vector2[] listG = generatedLine.GetVertices2().ToArray ();

		float covUser = GetUserLineCovering (userLine, listG);

		float covGen = GetGenLineCovering (generatedLine, userLine);
		
		Debug.Log (covUser + " ...:" + covGen + " == " + (covUser+covGen)/2 + "%");
		return (covUser+covGen)/2;
	}
	
	//jaki procent linii narysowanej lezy na tej wygenerowanej
	private float GetUserLineCovering(ILine userLine, Vector2[] listG) {
		int correctPoints = 0;
		int wrongPoints = 0;
		
		foreach(Vector2 point in userLine.GetVertices2()) {
			float min = 100;
			
			for (int i = 0; i < listG.Length -1; i++) {
				//(x2 - x1)(y - y1) = (y2 - y1)(x - x1)
				Vector2 p1 = listG[i];
				Vector2 p2 = listG[i+1];
				
				float left = (p2.x - p1.x)*(point.y - p1.y);
				float right= (p2.y - p1.y)*(point.x - p1.x);
				
				float distA = Vector2.Distance(p1, point);
				float distB = Vector2.Distance(p2, point);
				float distC = Vector2.Distance(p1, p2);

				float distCheck = Mathf.Abs((distA + distB) - distC);
				
				float score = Mathf.Abs(left - right);
				if (min > score && distCheck < _acceptedError) {
					min = score;
				}
			}
			
			if (min < _accuracy*levelMap[level]) {
				correctPoints++;
			}else {
				wrongPoints++;
			}
		}

		if (correctPoints + wrongPoints != 0) {
			return ((correctPoints*100) / (correctPoints + wrongPoints));
		}
		
		return 0;
	}

	//jaki procent wygenerowanych wierzcholkow zostal pokryty
	private float GetGenLineCovering (ILine generatedLine, ILine userLine) {
		int correctCheckpoints = 0;
		int wrongCheckpoints = 0;
		
		foreach (Vector2 checkpoint in generatedLine.GetVertices2()) {
			bool correct = false;
			
			foreach(Vector2 point in userLine.GetVertices2()) {
				
				float _distance = Vector2.Distance(point, checkpoint);
				
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

	
	public bool IsStartCorrect(Vector3 point, ILine generatedLine) {
		Vector2 vec = new Vector2 (point.x, point.y);
		if (Vector2.Distance (vec, generatedLine.GetVertices2()[0]) < _finalPointsError) {
			return true;
		}

		return false;
	}
}
