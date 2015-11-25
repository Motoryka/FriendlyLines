using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LineManagement;
using LineManagement.GLLines;

public enum Shape
{
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

public static class ShapeConverter
{
    static Dictionary<Shape, string> shapeString = new Dictionary<Shape, string>
    {
        {Shape.HorizontalLine, "Linia pozioma"},
        {Shape.VerticalLine, "Linia pionowa"},
        {Shape.DiagonalLine, "Linia ukośna"},
        {Shape.CurvedLine, "Linia krzywa"},
        {Shape.Triangle, "Trójkąt"},
        {Shape.Circle, "Okrąg"},
        {Shape.Ellipse, "Elipsa"},
        {Shape.Square, "Kwadrat"},
        {Shape.Rectangle, "Prostokąt"}
    };

    static Dictionary<string, Shape> stringShape;

    static ShapeConverter()
    {
        stringShape = new Dictionary<string, Shape>();
        foreach(Shape s in shapeString.Keys)
        {
            stringShape.Add(shapeString[s], s);
        }
    }

    public static Shape stringToShape(string s)
    {
        return stringShape[s];
    }

    public static string shapeToString(Shape s)
    {
        return shapeString[s];
    }


    public static IEnumerable GetShapeStringArray()
    {
        return stringShape.Keys;
    }
}

public static class LineStroke
{
    public const float VeryThin = 0.6f;

    public const float Thin = 0.8f;

    public const float Medium = 1.0f;

    public const float Thick = 1.2f;

    public const float VeryThick = 1.4f;

    public static int FloatToInt(float f)
    {
        return _ftoi[f];
    }

    public static float IntToFloat(int i)
    {
        return _itof[i];
    }

    static Dictionary<float, int> _ftoi = new Dictionary<float, int> { { VeryThin, 0 }, { Thin, 1 }, { Medium, 2 }, { Thick, 3 }, { VeryThick, 4 } };
    static Dictionary<int, float> _itof = new Dictionary<int, float> { { 0, VeryThin }, { 1, Thin }, { 2, Medium }, { 3, Thick }, { 4, VeryThick } };

	public static string FloatToStroke(float f)
	{
		if(f > 0.55f && f < 0.65f) return "Bardzo cienka";
		if(f > 0.75 && f < 0.85) return "Cienka";
		if(f > 0.95 && f < 0.105) return "Średnia";
		if(f > 0.115 && f < 0.125) return "Gruba";
		if(f > 0.135 && f < 0.145) return "Bardzo gruba";
		return "Nieznana";
	}
}

public class ShapeGenerator : MonoBehaviour
{
    private LineFactory lf;

    private PastelColorFactory pcf;

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
    private delegate ShapeElement CreateFunc();

    private Dictionary<Shape, CreateFunc> shapeMap;

    public Color pointColor { get; set; }
    public Color color { get; set; }
    public float size { get; set; }
    public bool drawStartPoint = false;

    private List<List<Vector2>> BezierCurves;

    // Use this for initialization
    void Awake()
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

        pcf = new PastelColorFactory();

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

    public ShapeElement CreateShape(Shape shape)
    {
        return shapeMap[shape]();
    }

    public ShapeElement CreateShape(ShapeElement shape)
    {
        var line = this.lf.Create(shape.Shape.GetVertices2());
        line.SetColor(shape.Shape.Color);
        line.SetSize(this.size);

        var startPoint = this.lf.Create(shape.Shape.GetVertices2()[0]);
        startPoint.SetColor(shape.StartPoint.Color);
        startPoint.SetSize(this.size);

        return shape;
    }

    private Vector2 GetRandomPointFromActiveArea()
    {
        float x = Random.Range(-this.gameUnitsHorizontalInActiveArea, this.gameUnitsHorizontalInActiveArea);
        float y = Random.Range(-this.gameUnitsVerticalInActiveArea, this.gameUnitsVerticalInActiveArea);
        return new Vector2(x, y);
    }

    public ShapeElement CreateHorizontalLine()
    {
        Vector2 startPoint = new Vector2(Random.Range(-this.gameUnitsHorizontalInActiveArea + 2, -2), 0);
        var line = this.lf.Create(startPoint);

        // add end point symetric to start point 
        line.AddVertex(new Vector2(-startPoint.x, -startPoint.y));

        line.SetColor(this.color);
        line.SetSize(this.size);

        var startLine = this.lf.Create(startPoint);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(line, startLine, Shape.HorizontalLine);

        return shape;
    }

    public ShapeElement CreateVerticalLine()
    {
        Vector2 startPoint = new Vector2(0, Random.Range(this.gameUnitsVerticalInActiveArea - 1, 1));

        var line = this.lf.Create(startPoint);

        // add end point symetric to start point 
        line.AddVertex(new Vector2(-startPoint.x, -startPoint.y));

        line.SetColor(this.color);
        line.SetSize(this.size);

        var startLine = this.lf.Create(startPoint);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(line, startLine, Shape.VerticalLine);

        return shape;
    }

    public ShapeElement CreateDiagonalLine()
    {
        Vector2 startPoint;
        do
        {
            startPoint = GetRandomPointFromActiveArea();
        } while (startPoint.x > -2 || startPoint.y < 1);

        var line = this.lf.Create(startPoint);

        // add end point symetric to start point 
        line.AddVertex(new Vector2(-startPoint.x, -startPoint.y));

        line.SetColor(this.color);
        line.SetSize(this.size);

        var startLine = this.lf.Create(startPoint);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(line, startLine, Shape.DiagonalLine);

        return shape;
    }

    public ShapeElement CreateCircle()
    {
        float radius = Random.Range(1f, 4f);
        Vector2 startPoint = new Vector2(0, radius);
        var circle = this.lf.Create(startPoint);

        for (float theta = 0; theta < 2 * Mathf.PI; theta += 2 * Mathf.PI / 100)
        {
            var x = radius * Mathf.Sin(theta);
            var y = radius * Mathf.Cos(theta);
            circle.AddVertex(new Vector2(x, y));
        }

        circle.AddVertex(new Vector2(0, radius));

        circle.SetColor(this.color);
        circle.SetSize(this.size);

        var startLine = this.lf.Create(startPoint);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(circle, startLine, Shape.Circle);

        return shape;
    }

    public ShapeElement CreateEllipse()
    {
        float radius = Random.Range(1f, 4f);

        float xFactor = 1f, yFactor = 1f;
        // 50% chance for squashing x or y coordinate to reduce its height
        if (Random.value < 0.5f)
        {
            xFactor = 0.5f;
        }
        else
        {
            yFactor = 0.5f;
        }

        Vector2 startPoint = new Vector2(0, yFactor * radius);
        var ellipse = this.lf.Create(startPoint);

        for (float theta = 0; theta <= 2 * Mathf.PI; theta += 2 * Mathf.PI / 100)
        {
            var x = xFactor * radius * Mathf.Sin(theta);
            var y = yFactor * radius * Mathf.Cos(theta);
            ellipse.AddVertex(new Vector2(x, y));
        }

        ellipse.AddVertex(new Vector2(0, yFactor * radius));

        ellipse.SetColor(this.color);

        ellipse.SetSize(this.size);

        var startLine = this.lf.Create(startPoint);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(ellipse, startLine, Shape.Ellipse);

        return shape;
    }

    public ShapeElement CreateTriangle()
    {
        float minLineLength = 0.6f * (this.gameUnitsVertical - this.gameUnitsVerticalMargin); // 60% of active generating area height
        // create random start point vector
        Vector2 A = GetRandomPointFromActiveArea();

        // create second point
        Vector2 B;
        float firstLineLength;
        do
        {
            B = GetRandomPointFromActiveArea();
            firstLineLength = Vector2.Distance(A, B);
        } while (firstLineLength < minLineLength);

        Vector2 C, AC, BA, BC;
        Vector2 AB = (B - A).normalized;
        float angleInA;
        float angleInB;
        float secondLineLength;
        do
        {
            C = GetRandomPointFromActiveArea();
            secondLineLength = Vector2.Distance(B, C);
            AC = (C - A).normalized;
            BA = (A - B).normalized;
            BC = (C - B).normalized;
            angleInA = Mathf.Acos(Vector2.Dot(AB, AC)) * 180 / Mathf.PI;
            angleInB = Mathf.Acos(Vector2.Dot(BA, BC)) * 180 / Mathf.PI;
        } while (((secondLineLength < firstLineLength * 0.7f) && (secondLineLength > firstLineLength * 1.4f)) || (angleInA < 25f || angleInA > 100f) || (angleInB < 25f || angleInB > 100f));

        // point of the center of a triangle
        var translation = new Vector2((A.x + B.x + C.x) / 3, (A.y + B.y + C.y) / 3);
        // translate triangle to the center of a screen
        A -= translation;
        B -= translation;
        C -= translation;

        var triangle = this.lf.Create(A);
        triangle.AddVertex(B);
        triangle.AddVertex(C);
        triangle.AddVertex(A);

        // set color and size of traingle's lines
        triangle.SetColor(this.color);
        triangle.SetSize(this.size);

        var startLine = this.lf.Create(A);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(triangle, startLine, Shape.Triangle);

        return shape;
    }

    public ShapeElement CreateSquare()
    {
        Vector2 A;
        float pointTranslation; // distance of the point from the center of the screen
        do
        {
            A = GetRandomPointFromActiveArea();
            pointTranslation = Vector2.Distance(A, new Vector2(0f, 0f));
        } while (A.y < 1 || pointTranslation < 1 || pointTranslation > this.gameUnitsVerticalInActiveArea);

        var square = this.lf.Create(A);
        square.AddVertex(new Vector2(A.y, -A.x));
        square.AddVertex(new Vector2(-A.x, -A.y));
        square.AddVertex(new Vector2(-A.y, A.x));
        square.AddVertex(A);

        square.SetColor(this.color);
        square.SetSize(this.size);

        var startLine = this.lf.Create(A);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(square, startLine, Shape.Square);

        return shape;
    }

    public ShapeElement CreateRectangle()
    {
        Vector2 A;
        do
        {
            A = GetRandomPointFromActiveArea();
        } while ((A.x < 1 && A.x > -1) || (A.y < 1 && A.y > -1) || A.x > 6 || A.x < -6 || (Mathf.Abs(A.x) < Mathf.Abs(A.y) * 1.2f && Mathf.Abs(A.x) > Mathf.Abs(A.y) * 0.8f));

        var rectangle = this.lf.Create(A);
        rectangle.AddVertex(new Vector2(-A.x, A.y));
        rectangle.AddVertex(new Vector2(-A.x, -A.y));
        rectangle.AddVertex(new Vector2(A.x, -A.y));
        rectangle.AddVertex(A);

        rectangle.SetColor(this.color);
        rectangle.SetSize(this.size);;

        var startLine = this.lf.Create(A);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(rectangle, startLine, Shape.Rectangle);

        return shape;
    }

    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1.0f - t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t2 = t * t;
        float t3 = t2 * t;

        Vector2 p = (u3 * p0) + 3.0f * (u2 * t * p1) + 3.0f * (u * t2 * p2) + (t3 * p3);

        return p;
    }

    public ShapeElement CreateCurvedLine()
    {
        List<Vector2> bezierCurve = this.BezierCurves[Random.Range(0, this.BezierCurves.Count)];

        Vector2 q0 = CalculateBezierPoint(0, bezierCurve[0], bezierCurve[1], bezierCurve[2], bezierCurve[3]); // starting point 
        Vector2 q1; // point of current incrementation
        float t; // time point of current incrementation [0, 1]
        int increments = 30; // amount of segments needed to draw curved line

        var curvedLine = this.lf.Create(q0);

        for (int i = 1; i <= increments; i++)
        {
            t = i / (float)increments;
            q1 = CalculateBezierPoint(t, bezierCurve[0], bezierCurve[1], bezierCurve[2], bezierCurve[3]);
            curvedLine.AddVertex(q1);
        }

        // set color and size of triangle's lines
        curvedLine.SetColor(this.color);
        curvedLine.SetSize(this.size);

        var startLine = this.lf.Create(q0);
        if (drawStartPoint)
        {
            startLine.SetColor(PastelColorFactory.Red.Color);
            startLine.SetSize(this.size);
        }

        ShapeElement shape = new ShapeElement(curvedLine, startLine, Shape.CurvedLine);

        return shape;
    }
}
