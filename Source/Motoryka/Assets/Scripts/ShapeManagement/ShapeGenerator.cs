using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;
using LineManagement.GLLines;
public enum Shape
{
    StraightLine,
    Triangle,
    CurvedLine
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
		this.gameUnitsVerticalInActiveArea = (this.gameUnitsVertical / 2) - (this.gameUnitsVerticalMargin / 2);
		this.gameUnitsHorizontalInActiveArea = (this.gameUnitsHorizontal / 2) - (this.gameUnitsHorizontalMargin / 2);

        lf = new LineFactory();

        shapeMap = new Dictionary<Shape, CreateFunc>
        {
            { Shape.StraightLine, CreateStraightLine },
            { Shape.Triangle, CreateTriangle },
            { Shape.CurvedLine, CreateCurvedLine }
        };

		#region used for testing generators

//		this.color = new Color(255f,0,0);
//		this.size = 0.10f;
//
//		var line = this.CreateStraightLine ();

		//for(int i = 0; i < 100; i++)
			//this.CreateTriangle ();

//		this.color = new Color(0,255f,0);
//		this.size = 0.20f;
//
//		var curvedLine = this.CreateCurvedLine ();
		/*
		Vector2 p0 = new Vector2 (-3, -2);
		Vector2 p1 = new Vector2 (-5, 2);
		Vector2 p2 = new Vector2 (-2, 3);
		Vector2 p3 = new Vector2 (0, 1);
		var line = lf.Create (p0);
		line.AddVertex (p3);
		line.SetColor(new Color(255f,0,0));
		line.SetSize (0.10f);*/

		#endregion
	}

    public ILine CreateShape(Shape shape)
    {
        return shapeMap[shape]();
    }

    public ILine CreateShape(List<Vector2> shape)
    {
        var line = this.lf.Create(shape);
        line.SetColor(this.color);
        line.SetSize(this.size);

        return line;
    }

	private Vector2 GetRandomPointFromActiveArea()
	{
		float x = Random.Range (-this.gameUnitsHorizontalInActiveArea, this.gameUnitsHorizontalInActiveArea);
		float y = Random.Range (-this.gameUnitsVerticalInActiveArea, this.gameUnitsVerticalInActiveArea);
		return new Vector2 (x, y);
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

	public ILine CreateTriangle()
	{
		float minLineLength = 0.5f * (this.gameUnitsVertical - this.gameUnitsVerticalMargin); // 50% of active generating area height

		// create random start point vector
		Vector2 startPoint = GetRandomPointFromActiveArea ();
		// make a traingle start line point
		var triangle = this.lf.Create (startPoint);

		// create second point
		Vector2 secondPoint = GetRandomPointFromActiveArea ();
		var firstLineLength = Vector2.Distance (startPoint, secondPoint);
		while (firstLineLength < minLineLength) {
			secondPoint = GetRandomPointFromActiveArea ();
			firstLineLength = Vector2.Distance (startPoint, secondPoint);
		}
		// add first line to triangle
		triangle.AddVertex (secondPoint);

		// create last point (line must be 0.7 times longer that first line but not more that 1.4 times, min distance from start point is 0.5 times longer than first line length) 
		Vector2 lastPoint = GetRandomPointFromActiveArea ();
		var secondLineLength = Vector2.Distance (secondPoint, lastPoint);
		while ((secondLineLength < firstLineLength * 0.7f) && (secondLineLength > firstLineLength * 1.4f) && (Vector2.Distance (startPoint, lastPoint) < (firstLineLength * 0.5))) {
			lastPoint = GetRandomPointFromActiveArea ();
			secondLineLength = Vector2.Distance (secondPoint, lastPoint);
		} 
		// add second line and connect first & last points
		triangle.AddVertex (lastPoint);
		triangle.AddVertex (startPoint);

		// set color and size of traingle's lines
		triangle.SetColor(this.color);
		triangle.SetSize (this.size);

		return triangle;
	}

	private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float u = 1.0f - t;
		float tt = t*t;
		float uu = u*u;
		float uuu = uu * u;
		float ttt = tt * t;
		
		Vector2 p = uuu * p0; //first term
		p += 3.0f * uu * t * p1; //second term
		p += 3.0f * u * tt * p2; //third term
		p += ttt * p3; //fourth term
		
		return p;
	}

	public ILine CreateCurvedLine()
	{
		// start point
		Vector2 p0 = new Vector2 (-3, -2);
		// first control point
		Vector2 p1 = new Vector2 (-5, 2);
		// second control point
		Vector2 p2 = new Vector2 (-2, 3);
		// end point
		Vector2 p3 = new Vector2 (0, 1);

		Vector2 q0 = CalculateBezierPoint(0, p1, p0, p2, p3); // starting point 
		Vector2 q1; // point of current incrementation
		float t; // time point of current incrementation [0, 1]
		int increments = 30; // amount of segments needed to draw curved line

		var curvedLine = this.lf.Create (q0);

		for(int i = 1; i <= increments; i++)
		{
			t = i / (float) increments;
			q1 = CalculateBezierPoint(t, p1, p0, p2, p3);
			curvedLine.AddVertex(q1);
		}

		// set color and size of traingle's lines
		curvedLine.SetColor(this.color);
		curvedLine.SetSize (this.size);

		return curvedLine;
	}
}
