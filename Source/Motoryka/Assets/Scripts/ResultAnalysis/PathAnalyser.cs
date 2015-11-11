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
		{AccuracyLevel.Easy, 2.5f},
		{AccuracyLevel.Medium, 2f},
		{AccuracyLevel.Hard, 1.5f}
	};
	
	private AccuracyLevel level = AccuracyLevel.Easy;
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
		
		//List<Vector2> listG = FillVertexes (generatedLine.GetVertices2().ToArray ());
		
		foreach (Vector2 checkpoint in generatedLine.GetVertices2()) {
			
			foreach(Vector2 point in userLine.GetVertices2()) {
				
				float _distance = Vector2.Distance(point, checkpoint);
				
				if (_distance < levelMap[level]*generatedLine.GetSize()) {
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
		/*Vector2[] listG = generatedLine.GetVertices2().ToArray ();
		Vector2[] listU = userLine.GetVertices2().ToArray ();
		
		if (listG.Length == 0 || listU.Length == 0)
			return false;
		
		if (Vector2.Distance(listG[0], listU[0]) < _finalPointsError &&
		    Vector2.Distance(listG[listG.Length-1], listU[listU.Length-1]) < _finalPointsError) {
			
			return true;
		}*/
		int size = userLine.GetVertices2 ().Count;

		if (size == 0)
			return false;

		float distance = Vector2.Distance (userLine.GetVertices2 () [0], userLine.GetVertices2 () [size - 1]);
		return distance < generatedLine.GetSize()*2;
	}
	
	public float GetResult (ILine generatedLine, ILine userLine) {
		float covUser = GetUserLineCovering (userLine, generatedLine);
		
		float covGen = GetGenLineCovering (generatedLine, userLine);

		Debug.Log ("user: " + covUser + " gen: " + covGen);
		return (covUser+covGen)/2;
	}
	
	/*
	 * Jaki procent linii narysowanej lezy na tej wygenerowanej.
	 */
	private float GetUserLineCovering(ILine userLine, ILine generatedLine) {
		Vector2[] listG = generatedLine.GetVertices2().ToArray ();
		int correctPoints = 0;
		int wrongPoints = 0;

		foreach(Vector2 point in userLine.GetVertices2()) {
			float min = GetMinDistance (listG, point);
			if (min < generatedLine.GetSize()/2*levelMap[level]) {
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
	
	/*
	 * Jaki procent wygenerowanych wierzcholkow zostal pokryty.
	 */
	private float GetGenLineCovering (ILine generatedLine, ILine userLine) {
		List<Vector2> listG = FillVertexes (generatedLine.GetVertices2().ToArray ());
		int correctCheckpoints = 0;
		int wrongCheckpoints = 0;

		foreach (Vector2 checkpoint in listG) {
			bool correct = false;
			
			foreach(Vector2 point in userLine.GetVertices2()) {
				
				float _distance = Vector2.Distance(point, checkpoint);
				
				if (_distance < generatedLine.GetSize()*levelMap[level]) {
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

	/**
	 * Uzupelnianie brakujacych wierzcholkow - gdy wygenerowane wierzcholki leza daleko od siebie.
	 */
	private List<Vector2> FillVertexes (Vector2[] listG) {
		List<Vector2> result = new List<Vector2> ();

		for (int i = 0; i < listG.Length; i++) {

			result.Add(listG[i]);

			if (i < listG.Length - 1) {
				result.AddRange (Fill (listG[i], listG[i+1]));
			}
		}

		return result;
	}

	private List<Vector2> Fill (Vector2 first, Vector2 second) {
		List<Vector2> result = new List<Vector2> ();

		if (Vector2.Distance (first, second) < 0.5f) {
			return result;
		}

		Vector2 extraVector = new Vector2 (
			(first.x + second.x) / 2, 
			(first.y + second.y) / 2
		);
		result.Add (extraVector);

		result.AddRange (Fill(first, extraVector));
		result.AddRange (Fill(extraVector, second));

		return result;
	}

	/**
	 * Get minimum distance from point to line.
	 */
	private float GetMinDistance (Vector2[] listG, Vector2 point) {
		float min = 100;
		
		for (int i = 0; i < listG.Length - 1; i++) {
			//(x2 - x1)(y - y1) = (y2 - y1)(x - x1)
			//u  = ((x2 - x1)(x - x1) + (y2 - y1)(y - y1)) / (x2 - x1)^2 + (y2 - y1)^2
			
			Vector2 p1 = listG [i];
			Vector2 p2 = listG [i + 1];
			
			float U = ((p2.x - p1.x)*(point.x - p1.x) + (p2.y - p1.y)*(point.y - p1.y))/ 
				(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow(p2.y - p1.y, 2));
			
			//P3 - punkt przeciecia prostej
			//x3 = x1 + u(x2 - x1)
			//y3 = y1 + u(y2 - y1)
			float x3 = p1.x + U*(p2.x - p1.x);
			float y3 = p1.y + U*(p2.y - p1.y);
			
			Vector2 p3 = new Vector2(x3, y3);
			
			float score = 0;
			
			if (U <= 0) {
				score = Vector2.Distance(p1, point);
			}else if (U > 0 && U < 1) {
				score = Vector2.Distance(p3, point);
			}else {
				score = Vector2.Distance(p2, point);
			}
			
			if (min > score) {
				min = score;
			}
		}

		return min;
	}
	
	/**
	 * Whether start point is correct - used for lines.
	 */
	public bool IsStartCorrect(Vector3 point, ILine generatedLine) {
		Vector2 vec = new Vector2 (point.x, point.y);
		if (Vector2.Distance (vec, generatedLine.GetVertices2()[0]) < generatedLine.GetSize()) {
			return true;
		}
		
		return false;
	}
	
	/**
	 * Whether start point is correct - used for circles.
	 */
	public bool IsStartCorrect2(Vector3 point, ILine generatedLine) {		
		Vector2[] listG = generatedLine.GetVertices2().ToArray ();
		Vector2 vec = new Vector2 (point.x, point.y);
		
		return GetMinDistance (listG, vec) < generatedLine.GetSize();
	}
}
