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
        List<Vector3> GetVerticles();
        List<Vector2> GetVertices2();
    }
}