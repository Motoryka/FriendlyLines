using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineLR : ILine
{
    private LineComponent _lineComponent;
    private Vector3 _defaultPosition;
    private List<Vector3> _vertices;
    private float _size;
    private Color _color;

    private GameObject _linePrefab;
    static private string _linePrefabName = "LinePrefab";

    public LineLR(GameObject parent)
    {
        _vertices = new List<Vector3>();
        _size = 1;
        _color = Color.white;
        _linePrefab = (GameObject)Resources.Load(_linePrefabName);

        GameObject newLine = GameObject.Instantiate(_linePrefab, Vector3.zero, Quaternion.identity) as GameObject;

        if (parent != null)
        {
            newLine.transform.parent = parent.transform;
        }

        _lineComponent = newLine.GetComponent<LineComponent>();
        _lineComponent.SetColor(_color);
        _lineComponent.SetSize(_size);
    }

    public void AddVertex(Vector2 pos)
    {
        AddVertex(new Vector3(pos.x, pos.y, _defaultPosition.z));
    }

    public void AddVertex(Vector3 pos)
    {
        pos.z = _defaultPosition.z;
        _vertices.Add(pos);

        _lineComponent.AddVertex(pos);
    }
    public void SetSize(float size)
    {
        _size = size;

        _lineComponent.SetSize(size);
    }

    public int VertexCount
    {
        get
        {
            return _vertices.Count;
        }
    }

    public void SetColor(Color color)
    {
        _color = color;

        _lineComponent.SetColor(_color);
    }
}
