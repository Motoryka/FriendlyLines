using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour, ILineDrawer {
    public float frequency = 0.1f;
    public Color color = Color.red;
    public float size = 1;
    public GameObject canvas;
    public string sortingLayer;

    private List<Vector3> _vertices;
    private LineFactory<LineLR> _factory;
    public bool _drawing;
    private List<ILine> _lines;
    private ILine _currentLine;
    private Vector3 _lastVertex;

    public LineDrawer()
    {
        _vertices = new List<Vector3>();
        _factory = new LineFactory<LineLR>();
        _lines = new List<ILine>();
        _currentLine = null;
    }

	// Use this for initialization
	void Start () {
        if (canvas == null)
        {
            canvas = new GameObject("Canvas");
            _factory.canvas = canvas;
        }

        if (sortingLayer == "")
        {
            sortingLayer = "Lines";
        }

        _factory.sortingLayer = sortingLayer;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void StartDrawing()
    {
        _drawing = true;
        _currentLine = _factory.Create();
        _lines.Add(_currentLine);

        _currentLine.SetColor(color);
        _currentLine.SetSize(size);
        _vertices = new List<Vector3>();
    }

    public void Draw(Vector3 position)
    {
        if(!_drawing)
        {
            return;
        }

        if(_vertices.Count != 0 && Vector3.Distance(_lastVertex, position) < (1f/frequency))
        {
            return;
        }

        _currentLine.AddVertex(position);
        _vertices.Add(position);
        _lastVertex = position;
    }

    public void StopDrawing()
    {
        _drawing = false;
        _currentLine = null;
    }

    public void SetSize(float size)
    {
        this.size = size;
    }

    public void SetFrequency(float f)
    {
        this.frequency = f;
    }

    public void DrawLine(Vector2 a, Vector2 b)
    {
        throw new System.NotImplementedException();
    }

    public void DrawLine(Vector2[] vertices)
    {
        throw new System.NotImplementedException();
    }

    public bool IsDrawing
    {
        get { return this._drawing; }
    }

    public ILine CurrentLine
    {
        get
        {
            return _currentLine;
        }
    }
}
