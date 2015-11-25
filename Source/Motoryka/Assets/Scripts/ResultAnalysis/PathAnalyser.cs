using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;

public enum AccuracyLevel {
	Easy,
	Medium,
	Hard
}

public struct LevelResult {
    public int levelNumber;
    public int result;
}

public class PathAnalyser : MonoBehaviour, IAnalyser
{
	
	private Dictionary<AccuracyLevel, float> levelMap = new Dictionary<AccuracyLevel, float> () 
	{
		{AccuracyLevel.Easy, 2.5f},
		{AccuracyLevel.Medium, 2f},
		{AccuracyLevel.Hard, 1.5f}
	};
	//Czy punkt startowy sie wyswietla
	public bool IsStartDisplayed = true;

	private AccuracyLevel level = AccuracyLevel.Medium;
	private delegate bool IsStartCorrectFunc(Vector2 point, ILine line, bool endingVertex = false);

	private Dictionary<Shape, IsStartCorrectFunc> shapeMap;

	public PathAnalyser () {

		shapeMap = new Dictionary<Shape, IsStartCorrectFunc>
		{
			{ Shape.StraightLine, IsStartEqPoint },
			{ Shape.HorizontalLine, IsStartEqPoint },
			{ Shape.VerticalLine, IsStartEqPoint },
			{ Shape.DiagonalLine, IsStartEqPoint },
			{ Shape.CurvedLine, IsStartEqPoint },
			{ Shape.Triangle, IsStartEqVertexes },
			{ Shape.Circle, IsStartEqShape },
			{ Shape.Ellipse, IsStartEqShape },
			{ Shape.Square, IsStartEqVertexes },
			{ Shape.Rectangle, IsStartEqVertexes }

		};
	}

	public PathAnalyser (AccuracyLevel level) {
		this.level = level;
	}

	public void SetIsStartDisplayed(bool val) {
		this.IsStartDisplayed = val;
	}

	public void SetAccuracyLevel(AccuracyLevel lvl) {
		this.level = lvl;
	}
	
	public bool IsFinished(ILine generatedLine, List<ILine> userLines) {
		
		if (userLines == null || generatedLine == null || userLines.Count == 0)
			return false;
		
		bool isChecked = false;
		
		List<Vector2> listG = FillVertexes (generatedLine.GetVertices2().ToArray ());

		foreach (Vector2 checkpoint in listG) {
			
			foreach (ILine line in userLines) {
				isChecked = IsPointCovered (checkpoint, line);
				
				if (isChecked) 
					break;
			}
			
			if (!isChecked) {
				return false;
			}
			isChecked = false;
		}

		if (GetUserLineCovering (generatedLine, userLines) < 50) return false;
		
		return true;
	}

	/**
	 * Whether the vertex - checkpoint - is covered by user line.
	 */
	private bool IsPointCovered (Vector2 checkpoint, ILine userLine)
	{
		//List<Vector2> listU = FillVertexes (userLine.GetVertices2 ().ToArray());
		float _distance = GetMinDistance (userLine.GetVertices2 ().ToArray (), checkpoint);

		if (_distance < levelMap [level] * userLine.GetSize ()) {
			return true;
		}
		/*foreach (Vector2 point in userLine.GetVertices2()) {
			float _distance = Vector2.Distance (point, checkpoint);
			if (_distance < levelMap [level] * userLine.GetSize ()) {
				isChecked = true;
				break;
			}
		}*/
		return false;
	}

	/**
	 * Returns a result of one level.
	 */
	public float GetResult (ILine generatedLine, List<ILine> userLines) {
		float covUser = GetUserLineCovering (generatedLine, userLines);
		
		float covGen = GetGenLineCovering (generatedLine, userLines);

		Debug.Log ("user: " + covUser + " gen: " + covGen);
		return (covUser+covGen)/2;
	}
	
	/*
	 * Jaki procent linii narysowanej lezy na tej wygenerowanej.
	 */
	private float GetUserLineCovering(ILine generatedLine, List<ILine> userLines) {
		Vector2[] listG = generatedLine.GetVertices2().ToArray ();
		int correctPoints = 0;
		int wrongPoints = 0;

		foreach (ILine line in userLines) {
			foreach (Vector2 point in line.GetVertices2()) {
				float min = GetMinDistance (listG, point);
				if (min < generatedLine.GetSize () / 2 * levelMap [level]) {
					correctPoints++;
				} else {
					wrongPoints++;
				}
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
	private float GetGenLineCovering (ILine generatedLine, List<ILine> userLines) {
		List<Vector2> listG = FillVertexes (generatedLine.GetVertices2().ToArray ());
		int correctCheckpoints = 0;
		int wrongCheckpoints = 0;

		foreach (Vector2 checkpoint in listG) {
			bool correct = false;
			
			foreach (ILine line in userLines) {
				correct = IsPointCovered(checkpoint, line);
				
				if (correct) {
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

		if (Vector2.Distance (first, second) < 0.1f) {
			return result;
		}

		Vector2 extraVector = new Vector2 (
			(first.x + second.x) / 2, 
			(first.y + second.y) / 2
		);

		while(Vector2.Distance(extraVector, first) > 0.1f) {

			result.Add (extraVector);

			extraVector.Set(
					(first.x + extraVector.x) / 2, 
					(first.y + extraVector.y) / 2
			);

		}

		while(Vector2.Distance(extraVector, second) > 0.1f) {
			
			result.Add (extraVector);
			
			extraVector.Set(
				(second.x + extraVector.x) / 2, 
				(second.y + extraVector.y) / 2
				);
			
		}
		//result.AddRange (Fill(first, extraVector));
		//result.AddRange (Fill(extraVector, second));

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
	 * Whether start point is correct.
	 */
    public bool IsStartCorrect(Vector3 point, ShapeElement shape, List<ILine> userLines, bool isStartDisplayed = true) 
    {
        Vector2 vec = new Vector2(point.x, point.y);

        this.IsStartDisplayed = isStartDisplayed;

        if(userLines.Count > 0)
        {
			//return IsStartEqPoint(vec, userLines[userLines.Count-1], true);
			return IsStartEqShapes (point, userLines);
        }

		if (IsStartDisplayed)
			return IsStartEqPoint (vec, shape.Shape);

		return shapeMap[shape.Type](vec, shape.Shape);
	}

	/**
	 * Whether start point is correct - for lines.
	 */
	private bool IsStartEqPoint(Vector2 point, ILine line, bool endingPoint = false) {
        Debug.Log("HERE");
        Vector2 examinedVertex;
        List<Vector2> vertices = line.GetVertices2();

        if (!endingPoint)
        {
            examinedVertex = vertices[0];
            Debug.Log("HERE2");
        }
        else
        {
            examinedVertex = vertices[vertices.Count - 1];
            Debug.Log("HERE3");
        }

        if (Vector2.Distance(point, examinedVertex) < line.GetSize() * levelMap[level])
        {
            Debug.Log("HERE4");
            return true;
        }
		
		return false;
	}

	/**
	 * Whether start point is correct - for triangles, rectangles.
	 */
	private bool IsStartEqVertexes(Vector2 point, ILine line, bool endingVertex = false) {
		foreach (Vector2 vertex in line.GetVertices2()) {
			if (Vector2.Distance (point, vertex) < line.GetSize()*levelMap[level]) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Whether start point is correct - for circles, ellipses.
	 */
    private bool IsStartEqShape(Vector2 point, ILine line, bool endingVertex = false)
    {
		Vector2[] listG = line.GetVertices2().ToArray ();
		
		return GetMinDistance (listG, point) < line.GetSize()*levelMap[level];
	}

	private bool IsStartEqShapes(Vector2 point, List<ILine> userLines) {
		foreach (ILine uLine in userLines) {
			bool result = IsStartEqShape (point, uLine);
			
			if (result) {
				return true;
			}
		}
		return false;
	}
}
