using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;
using LineManagement.GLLines;

public enum Shape
{
    StraightLine,
	HorizontalLine,
	VerticalLine,
	DiagonalLine,
	CurvedLine,
    Triangle,
    Circle,
	Ellipse,
	Square,
	Rectangle
}

public class ShapeGenerator : MonoBehaviour {

	private LineFactory lf;

	private float screenWidth; // width in px
	private float screenHeight; // height in px
	private float screenRatio; // width / height [px]
	private float gameUnitsVertical; // amount of game units in vertical dimension
	private float gameUnitsHorizontal; // amount of game units in horizontal dimension
	private float screenMargin; // % 
	private float gameUnitsVerticalMargin;
	private float gameUnitsHorizontalMargin;
	private float gameUnitsHorizontalInActiveArea;
	private float gameUnitsVerticalInActiveArea;
    private delegate ILine CreateFunc();

    private Dictionary<Shape, CreateFunc> shapeMap;

    public Color color;
    public float size;

	private List<List<Vector2>> BezierCurves;

	// Use this for initialization
    void Start() 
	{
		this.screenWidth = (float)Screen.width;
		this.screenHeight = (float)Screen.height;
		this.screenRatio = this.screenWidth / this.screenHeight;
		this.gameUnitsVertical = 2 * Camera.main.orthographicSize;
		this.gameUnitsHorizontal = this.gameUnitsVertical * this.screenRatio;
		this.screenMargin = 0.1f;
		this.gameUnitsVerticalMargin = this.gameUnitsVertical * this.screenMargin;
		this.gameUnitsHorizontalMargin = this.gameUnitsHorizontal * this.screenMargin;
		this.gameUnitsVerticalInActiveArea = (this.gameUnitsVertical / 2) - (this.gameUnitsVerticalMargin);
		this.gameUnitsHorizontalInActiveArea = (this.gameUnitsHorizontal / 2) - (this.gameUnitsHorizontalMargin);

        lf = new LineFactory();

		BezierCurves = new List<List<Vector2>>
		{
			new List<Vector2> { new Vector2(-3, -2), new Vector2(-5, 2), new Vector2(-2, 3), new Vector2(0, 1) },
			new List<Vector2> { new Vector2(-5, 2), new Vector2(-3, -2), new Vector2(-2, 3), new Vector2(0, 1) },
			new List<Vector2> { new Vector2(-7, -2), new Vector2(-2, 4), new Vector2(-2, 4), new Vector2(7, -2) },
			new List<Vector2> { new Vector2(4, 3), new Vector2(-5, 3), new Vector2(-4, -1), new Vector2(4, -2) },
			new List<Vector2> { new Vector2(-6, -2), new Vector2(-2, 3), new Vector2(-0, -3), new Vector2(7, 2) },
			new List<Vector2> { new Vector2(-2, 3), new Vector2(-5, -2), new Vector2(5, -2), new Vector2(2, 3) },
			new List<Vector2> { new Vector2(-2, 3), new Vector2(-2, -3), new Vector2(-1, -3), new Vector2(6, 2) },
			new List<Vector2> { new Vector2(-7, 0), new Vector2(-2, 1), new Vector2(1, -2), new Vector2(5, 1) },
			new List<Vector2> { new Vector2(-5, 2), new Vector2(-3, -3), new Vector2(0, 3), new Vector2(3, 2) },
			new List<Vector2> { new Vector2(-5, -2), new Vector2(-2, -2), new Vector2(0, 2), new Vector2(5, 0) },
			new List<Vector2> { new Vector2(-2, 3), new Vector2(1, 1), new Vector2(-2, -1), new Vector2(-1, -3) },
			new List<Vector2> { new Vector2(-4, -1), new Vector2(-2, 3), new Vector2(5, -1), new Vector2(5, -1) },
		};

        shapeMap = new Dictionary<Shape, CreateFunc>
        {
            { Shape.StraightLine, CreateStraightLine },
			{ Shape.HorizontalLine, CreateHorizontalLine },
			{ Shape.VerticalLine, CreateVerticalLine },
			{ Shape.DiagonalLine, CreateDiagonalLine },
			{ Shape.CurvedLine, CreateCurvedLine },
            { Shape.Triangle, CreateTriangle },
			{ Shape.Circle, CreateCircle },
			{ Shape.Ellipse, CreateEllipse },
			{ Shape.Square, CreateSquare },
			{ Shape.Rectangle, CreateRectangle }
		};
	}

    public ILine CreateShape(Shape shape)
    {
        return shapeMap[shape]();
    }

	private Vector2 GetRandomPointFromActiveArea()
	{
		float x = Random.Range (-this.gameUnitsHorizontalInActiveArea, this.gameUnitsHorizontalInActiveArea);
		float y = Random.Range (-this.gameUnitsVerticalInActiveArea, this.gameUnitsVerticalInActiveArea);
		return new Vector2 (x, y);
	}

	private Vector2 GetRandomPointOnXAxisFromActiveArea(float x)
	{
		float y = Random.Range (-this.gameUnitsVerticalInActiveArea, this.gameUnitsVerticalInActiveArea);
		return new Vector2 (x, y);
	}

	private Vector2 GetRandomPointOnYAxisFromActiveArea(float y)
	{
		float x = Random.Range (-this.gameUnitsHorizontalInActiveArea+2, this.gameUnitsHorizontalInActiveArea-2);
		return new Vector2 (x, y);
	}

	public ILine CreateHorizontalLine()
	{
		Vector2 startPoint = GetRandomPointOnYAxisFromActiveArea(0);
		while(startPoint.x < 2 && startPoint.x > -2)
		{
			startPoint = GetRandomPointOnYAxisFromActiveArea(0);
		}
		var line = this.lf.Create (startPoint);
		
		// add end point symetric to start point 
		line.AddVertex (new Vector2 (-startPoint.x, -startPoint.y));
		
		line.SetColor(this.color);
		line.SetSize (this.size);
		
		return line;
	}

	public ILine CreateVerticalLine()
	{
		Vector2 startPoint = GetRandomPointOnXAxisFromActiveArea(0);
		while(startPoint.y < 1 && startPoint.y > -1)
		{
			startPoint = GetRandomPointOnXAxisFromActiveArea(0);
		}
		var line = this.lf.Create (startPoint);
		
		// add end point symetric to start point 
		line.AddVertex (new Vector2 (-startPoint.x, -startPoint.y));
		
		line.SetColor(this.color);
		line.SetSize (this.size);
		
		return line;
	}

	public ILine CreateStraightLine()
	{
		Vector2 startPoint = GetRandomPointFromActiveArea ();
		var line = this.lf.Create (startPoint);

		// add end point symetric to start point 
		line.AddVertex (new Vector2 (-startPoint.x, -startPoint.y));

		line.SetColor(this.color);
		line.SetSize (this.size);

		return line;
	}

	public ILine CreateDiagonalLine()
	{
		Vector2 startPoint = GetRandomPointFromActiveArea ();
		while( (startPoint.x > -2 && startPoint.x < 2) || (startPoint.y < 1 && startPoint.y > -1) )
		{
			startPoint = GetRandomPointFromActiveArea ();
		}
		var line = this.lf.Create (startPoint);
		
		// add end point symetric to start point 
		line.AddVertex (new Vector2 (-startPoint.x, -startPoint.y));
		
		line.SetColor(this.color);
		line.SetSize (this.size);
		
		return line;
	}

	public ILine CreateCircle()
	{
		float radius = Random.Range (1f, 4f);
		Vector2 startPoint = new Vector2(0, radius);
		var circle = this.lf.Create (startPoint);

		for(float theta = 0; theta < 2*Mathf.PI; theta += 2*Mathf.PI / 100)
		{
			var x = radius * Mathf.Sin (theta);
			var y = radius * Mathf.Cos (theta);
			circle.AddVertex(new Vector2(x, y));
		}

		circle.AddVertex(new Vector2(0, radius));

		circle.SetColor(this.color);
		circle.SetSize (this.size);

		return circle;
	}

	public ILine CreateEllipse()
	{
		float radius = Random.Range (1f, 4f);

		float xFactor = 1f, yFactor = 1f;
		// 50% chance for squashing x or y coordinate to reduce its height
		if(Random.value < 0.5f){
			xFactor = 0.5f;
		}
		else{
			yFactor = 0.5f;
		}

		Vector2 startPoint = new Vector2(0, yFactor * radius);
		var ellipse = this.lf.Create (startPoint);

		for(float theta = 0; theta <= 2*Mathf.PI; theta += 2*Mathf.PI / 100)
		{
			var x = xFactor * radius * Mathf.Sin (theta);
			var y = yFactor * radius * Mathf.Cos (theta);
			ellipse.AddVertex(new Vector2(x, y));
		}

		ellipse.AddVertex(new Vector2(0, yFactor * radius));
		
		ellipse.SetColor(this.color);
		ellipse.SetSize (this.size);
		
		return ellipse;
	}

	public ILine CreateTriangle()
	{
		float minLineLength = 0.6f * (this.gameUnitsVertical - this.gameUnitsVerticalMargin); // 60% of active generating area height
		
		// create random start point vector
		Vector2 A = GetRandomPointFromActiveArea ();

		// create second point
		Vector2 B = GetRandomPointFromActiveArea ();
		var firstLineLength = Vector2.Distance (A, B);
		while (firstLineLength < minLineLength) {
			B = GetRandomPointFromActiveArea ();
			firstLineLength = Vector2.Distance (A, B);
		}

		Vector2 C = GetRandomPointFromActiveArea ();
		Vector2 AB = (B - A).normalized;
		Vector2 AC = (C - A).normalized;
		float angleInA = Mathf.Acos (Vector2.Dot (AB, AC)) * 180 / Mathf.PI;

		var secondLineLength = Vector2.Distance (B, C);

		while (((secondLineLength < firstLineLength * 0.7f) && (secondLineLength > firstLineLength * 1.4f)) || (angleInA < 20f || angleInA > 100f)) {
			C = GetRandomPointFromActiveArea ();
			secondLineLength = Vector2.Distance (B, C);
			AC = (C - A).normalized;
			angleInA = Mathf.Acos (Vector2.Dot (AB, AC)) * 180 / Mathf.PI;
		} 
		// point of the center of a triangle
		var translation = new Vector2( (A.x + B.x + C.x) / 3, (A.y + B.y + C.y) / 3);
		// translate triangle to the center of a screen
		A -= translation;
		B -= translation;
		C -= translation;

		var triangle = this.lf.Create (A);
		triangle.AddVertex (B);
		triangle.AddVertex (C);
		triangle.AddVertex (A);
		
		// set color and size of traingle's lines
		triangle.SetColor(this.color);
		triangle.SetSize (this.size);
		
		return triangle;
	}

	public ILine CreateSquare()
	{
		Vector2 A = GetRandomPointFromActiveArea();
		float pointTranslation =  Vector2.Distance(A, new Vector2(0f, 0f)); // distance of the point from the center of the screen
		while(pointTranslation < 1 || pointTranslation > this.gameUnitsVerticalInActiveArea){
			A = GetRandomPointFromActiveArea();
			pointTranslation =  Vector2.Distance(A, new Vector2(0f, 0f));
		}
		var square = this.lf.Create(A);
		square.AddVertex(new Vector2(A.y, -A.x));
		square.AddVertex(new Vector2(-A.x, -A.y));
		square.AddVertex(new Vector2(-A.y, A.x));
		square.AddVertex(A);

		square.SetColor(this.color);
		square.SetSize (this.size);

		return square;
	}

	public ILine CreateRectangle()
	{
		Vector2 A = GetRandomPointFromActiveArea();
		while((A.x < 1 && A.x > -1) || (A.y < 1 && A.y > -1) || A.x > 6 || A.x < -6){
			A = GetRandomPointFromActiveArea();
		}
		var rectangle = this.lf.Create(A);
		rectangle.AddVertex(new Vector2(-A.x, A.y));
		rectangle.AddVertex(new Vector2(-A.x, -A.y));
		rectangle.AddVertex(new Vector2(A.x, -A.y));
		rectangle.AddVertex(A);
		
		rectangle.SetColor(this.color);
		rectangle.SetSize (this.size);
		
		return rectangle;
	}

	private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float u = 1.0f - t;
		float u2 = u*u;
		float u3 = u2 * u;
		float t2 = t*t;
		float t3 = t2 * t;
		
		Vector2 p = (u3 * p0) + 3.0f * (u2 * t * p1) + 3.0f * (u * t2 * p2) + (t3 * p3);
		
		return p;
	}

	public ILine CreateCurvedLine()
	{
		List<Vector2> bezierCurve = this.BezierCurves[Random.Range(0, this.BezierCurves.Count)];

		Vector2 q0 = CalculateBezierPoint(0, bezierCurve[0], bezierCurve[1], bezierCurve[2], bezierCurve[3]); // starting point 
		Vector2 q1; // point of current incrementation
		float t; // time point of current incrementation [0, 1]
		int increments = 30; // amount of segments needed to draw curved line

		var curvedLine = this.lf.Create (q0);

		for(int i = 1; i <= increments; i++)
		{
			t = i / (float) increments;
			q1 = CalculateBezierPoint(t, bezierCurve[0], bezierCurve[1], bezierCurve[2], bezierCurve[3]);
			curvedLine.AddVertex(q1);
		}

		// set color and size of triangle's lines
		curvedLine.SetColor(this.color);
		curvedLine.SetSize (this.size);

		return curvedLine;
	}
}
