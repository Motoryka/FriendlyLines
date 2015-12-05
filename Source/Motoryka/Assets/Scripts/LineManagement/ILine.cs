using System.Collections.Generic;

using UnityEngine;

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
		float GetSize();
        void AddVertex(Vector2 pos);
        void AddVertex(Vector3 pos);
        List<Vector3> GetVertices();
        List<Vector2> GetVertices2();
        void SetVertice(int vertice, Vector2 v);
		void CollapseToPoint(Vector2 v, float inTime);

        void Delete();
    }
}