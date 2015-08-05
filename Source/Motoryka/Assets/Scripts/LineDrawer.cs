using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour, ILineDrawer {
    public float frequency = 0.1f;
    public Color color = Color.white;
    public float size = 1;

    private GameObject _canvas;
    private List<Vector2> _vertices;
    private LineFactory _factory;
    private bool _drawing;

    public LineDrawer()
    {
        _vertices = new List<Vector2>();
        _factory = new LineFactory();
    }

	// Use this for initialization
	void Start () {
	    if(_canvas == null)
        {
            _canvas = new GameObject("Canvas");
        }
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
    }

    public void Draw(Vector2 position)
    {
        throw new System.NotImplementedException();
    }

    public void StopDrawing()
    {
        _drawing = false;
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
}
