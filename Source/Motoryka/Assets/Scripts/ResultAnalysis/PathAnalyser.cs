using System.Collections.Generic;

using LineManagement;

using UnityEngine;

public enum AccuracyLevel 
{
	Easy,
	Medium,
	Hard
}

public struct LevelResult 
{
    public int levelNumber;
    public Result result;
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

	public PathAnalyser ()
    {
		shapeMap = new Dictionary<Shape, IsStartCorrectFunc>
		{
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

	public PathAnalyser (AccuracyLevel level)
    {
		this.level = level;
	}

	public void SetIsStartDisplayed(bool val)
    {
		this.IsStartDisplayed = val;
	}

	public void SetAccuracyLevel(AccuracyLevel lvl)
    {
		this.level = lvl;
	}
	
	public bool IsFinished(ILine generatedLine, List<ILine> userLines)
    {
	    if (userLines == null || generatedLine == null || userLines.Count == 0)
	    {
	        return false;
	    }
		
		bool isChecked = false;
		
		List<Vector2> listG = FillVertexes (generatedLine.GetVertices2().ToArray ());

		foreach (Vector2 checkpoint in listG) 
        {
			
			foreach (ILine line in userLines) 
            {
				isChecked = IsPointCovered (checkpoint, line);

                if (isChecked)
                {
                    break;
                }
			}
			
			if (!isChecked) 
            {
				return false;
			}

			isChecked = false;
		}

        foreach (ILine line in userLines)
        {
            foreach (Vector2 point in line.GetVertices2())
            {
                isChecked = IsPointCovered(point, generatedLine);

                if (!isChecked)
                    break;
            }

            if (!isChecked)
            {
                return false;
            }

            isChecked = false;
        }
		
		return true;
	}

	/**
	 * Whether the vertex - checkpoint - is covered by user line.
	 */
	private bool IsPointCovered (Vector2 checkpoint, ILine userLine, bool result = false)
	{
        float accuracy = levelMap[level];

        if (result)
            accuracy /= 2;

		float _distance = GetMinDistance (userLine.GetVertices2 ().ToArray (), checkpoint);

        if (_distance < accuracy * userLine.GetSize())
        {
			return true;
		}

		return false;
	}

	/**
	 * Returns a result of one level.
	 */
    public Result GetResult(ILine generatedLine, List<ILine> userLines)
    {
        float covUserError = GetUserLineErrorCovering(generatedLine, userLines);
		
		float covGen = GetGenLineCovering (generatedLine, userLines);

        Result result = new Result((int)covGen, (int)covUserError);
        Debug.Log("err: " + covUserError + " gen: " + covGen);
        return result;
	}
	
	/*
	 * Jaki procent linii narysowanej lezy poza ta wygenerowana.
	 */
	private float GetUserLineErrorCovering(ILine generatedLine, List<ILine> userLines)
    {
		List<Vector2> listG = FillVertexes (generatedLine.GetVertices2().ToArray());
		int correctPoints = 0;
		int wrongPoints = 0;

		foreach (ILine line in userLines) 
        {
            List<Vector2> listU = FillVertexes(line.GetVertices2().ToArray());
			foreach (Vector2 point in listU) 
            {
				float min = GetMinDistance (listG.ToArray(), point);
				if (min < generatedLine.GetSize () / 2 * levelMap [level]) 
                {
					correctPoints++;
				} 
                else 
                {
					wrongPoints++;
				}
			}
		}

		if (correctPoints + wrongPoints != 0) 
        {
            return (wrongPoints * 100) / (correctPoints + wrongPoints);
		}
		
		return 0;
	}
	
	/*
	 * Jaki procent wygenerowanych wierzcholkow zostal pokryty.
	 */
	private float GetGenLineCovering (ILine generatedLine, List<ILine> userLines)
    {
		List<Vector2> listG = FillVertexes(generatedLine.GetVertices2().ToArray());
		int correctCheckpoints = 0;
		int wrongCheckpoints = 0;

		foreach (Vector2 checkpoint in listG) 
        {
			bool correct = false;
			
			foreach (ILine line in userLines) 
            {
				correct = IsPointCovered(checkpoint, line, true);
				
				if (correct) 
                {
					break;
				}
			}
			
			if (correct) 
            {
				correctCheckpoints++;
			}
            else 
            {
				wrongCheckpoints++;
			}
		}
		
		if (correctCheckpoints + wrongCheckpoints != 0) 
        {
			return (100 * correctCheckpoints) / (correctCheckpoints + wrongCheckpoints);
		}
		
		return 0;
	}

	/**
	 * Uzupelnianie brakujacych wierzcholkow - gdy wygenerowane wierzcholki leza daleko od siebie.
	 */
	private List<Vector2> FillVertexes (Vector2[] listG) 
    {
		List<Vector2> result = new List<Vector2>();

		for (int i = 0; i < listG.Length; i++) 
        {
			result.Add(listG[i]);

			if (i < listG.Length - 1) 
            {
				result.AddRange (Fill (listG[i], listG[i+1]));
			}
		}

		return result;
	}

    /**
     * Zwraca liste wierzcholkow wygenerowanych pomiedzy podanymi wierzcholkami.
     */
	private List<Vector2> Fill (Vector2 first, Vector2 second)
    {
		List<Vector2> result = new List<Vector2>();

		if (Vector2.Distance(first, second) < 0.1f)
        {
			return result;
		}

		Vector2 extraVector = new Vector2(
			(first.x + second.x) / 2, 
			(first.y + second.y) / 2
            );

		while(Vector2.Distance(extraVector, first) > 0.1f) 
        {
			result.Add(extraVector);

			extraVector.Set(
					(first.x + extraVector.x) / 2, 
					(first.y + extraVector.y) / 2
                    );
		}

		while(Vector2.Distance(extraVector, second) > 0.1f)
        {
			result.Add(extraVector);
			
			extraVector.Set(
				(second.x + extraVector.x) / 2, 
				(second.y + extraVector.y) / 2
				);
		}

		return result;
	}

	/**
	 * Get minimum distance from point to line.
	 */
	private float GetMinDistance (Vector2[] listG, Vector2 point)
    {
        if (listG.Length == 1)
        {
            return Vector2.Distance(point, listG[0]);
        }

		float min = 100;
		
		for (int i = 0; i < listG.Length - 1; i++) 
        {
			//(x2 - x1)(y - y1) = (y2 - y1)(x - x1)
			//u  = ((x2 - x1)(x - x1) + (y2 - y1)(y - y1)) / (x2 - x1)^2 + (y2 - y1)^2
			
			Vector2 p1 = listG [i];
			Vector2 p2 = listG [i + 1];
			
			float U = ((p2.x - p1.x)*(point.x - p1.x) + (p2.y - p1.y)*(point.y - p1.y)) /
				(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow(p2.y - p1.y, 2));
			
			//P3 - punkt przeciecia prostej
			//x3 = x1 + u(x2 - x1)
			//y3 = y1 + u(y2 - y1)
			float x3 = p1.x + U*(p2.x - p1.x);
			float y3 = p1.y + U*(p2.y - p1.y);
			
			Vector2 p3 = new Vector2(x3, y3);
			
			float score = 0;
			
			if (U <= 0) 
            {
				score = Vector2.Distance(p1, point);
			}
            else if (U > 0 && U < 1) 
            {
				score = Vector2.Distance(p3, point);
			}
            else 
            {
				score = Vector2.Distance(p2, point);
			}
			
			if (min > score) 
            {
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
			return IsStartEqShapes (point, userLines);
        }

        if (IsStartDisplayed)
        {
            return IsStartEqPoint (vec, shape.Shape);
        }

		return shapeMap[shape.Type](vec, shape.Shape);
	}

	/**
	 * Whether start point is correct - for lines.
	 */
	private bool IsStartEqPoint(Vector2 point, ILine line, bool endingPoint = false) {
        Vector2 examinedVertex;
        List<Vector2> vertices = line.GetVertices2();

        if (!endingPoint)
        {
            examinedVertex = vertices[0];
        }
        else
        {
            examinedVertex = vertices[vertices.Count - 1];
        }

        if (Vector2.Distance(point, examinedVertex) < line.GetSize() * levelMap[level])
        {
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

        if (listG.Length == 1)
            return Vector2.Distance(listG[0], point) < line.GetSize() * levelMap[level];
		
		return GetMinDistance (listG, point) < line.GetSize()*levelMap[level];
	}

    /**
     * Whether start point is touching previous lines. 
     */
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
