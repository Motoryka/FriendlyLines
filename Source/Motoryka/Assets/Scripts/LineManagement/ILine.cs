using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LineManagement
{
    public interface ILine
    {
        string SortingLayer { get; set; }
        void Init(Transform canvas);
        int VertexCount { get; }
        Color Color { get; }
        void SetColor(Color color);
        void SetSize(float size);
        void AddVertex(Vector2 pos);
        void AddVertex(Vector3 pos);
        List<Vector3> GetVertices();
        List<Vector2> GetVertices2();
        void SetVertice(int vertice, Vector2 v);
    }
}