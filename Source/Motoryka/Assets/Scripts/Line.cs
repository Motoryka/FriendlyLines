using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour 
{
    private LineRenderer _lineRenderer;
    private Vector3 _defaultPosition;
    private List<Vector3> _vertices;
    private int _lrVerticesCount;
    private float _size;
    private float _lrSize;
    private Color _color;
    private Color _lrColor;

    public Line()
    {
        _vertices = new List<Vector3>();
        _lrVerticesCount = 0;
        _size = 1;
        _lrSize = 1;
        _color = _lrColor = Color.white;
    }

	// Use this for initialization
	void Start () {
        _lineRenderer = GetComponent<LineRenderer>();
        _defaultPosition = Vector3.zero;

        _lineRenderer.SetColors(_color, _color);
        _lineRenderer.SetWidth(_size, _size);
	}

    void Update()
    {
        while(_lrVerticesCount < _vertices.Count)
        {
            _lineRenderer.SetVertexCount(_lrVerticesCount + 1);
            _lineRenderer.SetPosition(_lrVerticesCount, _vertices[_lrVerticesCount]);

            _lrVerticesCount++;
        }

        if (_size != _lrSize)
        {
            _lrSize = _size;
            _lineRenderer.SetWidth(_lrSize, _lrSize);
        }

        if(_lrColor != _color)
        {
            _lrColor = _color;
            _lineRenderer.SetColors(_color, _color);
        }
    }

    public void AddVertex(Vector2 pos)
    {
        AddVertex(new Vector3(pos.x, pos.y, _defaultPosition.z));
    }

    public void AddVertex(Vector3 pos)
    {
        pos.z = _defaultPosition.z;
        _vertices.Add(pos);
    }
    public void SetSize(float size)
    {
        _size = size;
    }

    public int vertexCount
    {
        get
        {
            return _vertices.Count;
        }
    }

    public void SetColor(Color color)
    {
        _color = color;
    }
}
