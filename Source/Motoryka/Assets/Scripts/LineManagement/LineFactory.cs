using UnityEngine;
using System.Collections;
using System;

public class LineFactory<T> where T : ILine
{
    public GameObject canvas = null;
    public string sortingLayer = "Shapes";

    static private GameObject _linePrefab = null;
    static private string _linePrefabName = "Prefabs/LinePrefab";

    public T Create()
    {
        T newLine = default(T);

        if (isLineLR(typeof(T)))
        {
            if (_linePrefab == null)
                _linePrefab = (GameObject)Resources.Load(_linePrefabName);

            var newLineGameObject = ((GameObject)GameObject.Instantiate(_linePrefab, Vector3.zero, Quaternion.identity)).GetComponent<T>();

            if (canvas != null)
                newLineGameObject.Init(canvas.transform);

            newLine = newLineGameObject;
            newLine.SortingLayer = sortingLayer;
        }

        return newLine;
    }


    public T Create(Vector3 startingVertex)
    {
        T newLine = Create();

        newLine.AddVertex(startingVertex);

        return newLine;
    }

    public T Create(Vector3 startingVertex, Vector3 endingVertex)
    {
        T newLine = Create(startingVertex);

        newLine.AddVertex(endingVertex);

        return newLine;
    }

    public T Create(Vector2 startingVertex)
    {
        T newLine = Create();

        newLine.AddVertex(startingVertex);

        return newLine;
    }

    public T Create(Vector2 startingVertex, Vector2 endingVertex)
    {
        T newLine = Create(startingVertex);

        newLine.AddVertex(endingVertex);

        return newLine;
    }

    private bool isLineLR(Type t)
    {
        return t == typeof(LineLR);
    }

}
