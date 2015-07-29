using UnityEngine;
using System.Collections;

public class LineFactory : MonoBehaviour 
{
    static private GameObject _linePrefab;
    static private string _linePrefabName = "LinePrefab";

    static LineFactory()
    {
        _linePrefab = (GameObject)Resources.Load(_linePrefabName);
    }

    public Line Create()
    {
        return ((GameObject)Instantiate(_linePrefab, Vector3.zero, Quaternion.identity)).GetComponent<Line>();
    }

    public Line Create(Vector3 startingVertex)
    {
        Line newLine = Create();

        newLine.AddVertex(startingVertex);

        return newLine;
    }

    public Line Create(Vector3 startingVertex, Vector3 endingVertex)
    {
        Line newLine = Create(startingVertex);

        newLine.AddVertex(endingVertex);

        return newLine;
    }

}
