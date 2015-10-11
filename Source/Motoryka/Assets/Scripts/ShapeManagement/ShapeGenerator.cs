using UnityEngine;
using System.Collections;

public class ShapeGenerator {

	private LineFactory<LineLR> lf;

	float screenWidth; // width in px
	float screenHeight; // height in px
	float screenRatio; // width / height [px]
	float gameUnitsVertical; // amount of game units in vertical dimension
	float gameUnitsHorizontal; // amount of game units in horizontal dimension
	float screenMargin; // % 
	float gameUnitsVerticalMargin;
	float gameUnitsHorizontalMargin;
	float gameUnitsHorizontalInActiveArea;
	float gameUnitsVerticalInActiveArea;

	// Use this for initialization
	public void Start () {
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

		lf = new LineFactory<LineLR> ();

		//this.CreateStraightLine ();

		//for(int i = 0; i < 3; i++)
			//this.CreateTriangle ();
	}

	private Vector2 GetRandomPointFromActiveArea()
	{
		float x = Random.Range (-this.gameUnitsHorizontalInActiveArea, this.gameUnitsHorizontalInActiveArea);
		float y = Random.Range (-this.gameUnitsVerticalInActiveArea, this.gameUnitsVerticalInActiveArea);
		return new Vector2 (x, y);
	}

	public void CreateStraightLine()
	{
		float minLineLength = 0.5f * (this.gameUnitsVertical - this.gameUnitsVerticalMargin); // % of active generating area height

		Vector2 startPoint = GetRandomPointFromActiveArea ();
		var line = this.lf.Create (startPoint);

		Debug.Log ("x: " + startPoint.x + ", y: " + startPoint.y);

		/*Vector2 endPoint = GetRandomPointFromActiveArea ();
		while (Vector2.Distance(startPoint, endPoint) < minLineLength) {
			endPoint = GetRandomPointFromActiveArea();
		}*/

		// add end point symetric to start point 
		line.AddVertex (new Vector2 (-startPoint.x, -startPoint.y));

		line.SetColor(new Color(255f,0,0));
		line.SetSize (0.10f);
	}

	public void CreateTriangle()
	{
		float minLineLength = 0.5f * (this.gameUnitsVertical - this.gameUnitsVerticalMargin);

		// create random start point vector
		Vector2 startPoint = GetRandomPointFromActiveArea ();
		// make a traingle start line point
		var triangle = this.lf.Create (startPoint);
		// set color and size of traingle's lines
		triangle.SetColor(new Color(255f,0,0));
		triangle.SetSize (0.10f);

		// create second point
		Vector2 secondPoint = GetRandomPointFromActiveArea ();
		var firstLineLength = Vector2.Distance (startPoint, secondPoint);
		while (firstLineLength < minLineLength) {
			secondPoint = GetRandomPointFromActiveArea ();
		}
		// add first line to triangle
		triangle.AddVertex (secondPoint);

		// create last point (line must be 0.7 times longer that first line but not more that 1.4 times, min distance from start point is 0.5 times longer than first line length) 
		Vector2 lastPoint = GetRandomPointFromActiveArea ();
		var secondLineLength = Vector2.Distance (secondPoint, lastPoint);
		while ((secondLineLength < firstLineLength * 0.7f) && (secondLineLength > firstLineLength * 1.4f) && (Vector2.Distance (startPoint, lastPoint) < (firstLineLength * 0.5))) {
			lastPoint = GetRandomPointFromActiveArea ();
		} 
		// add second line and connect first & last points
		triangle.AddVertex (lastPoint);
		triangle.AddVertex (startPoint);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
