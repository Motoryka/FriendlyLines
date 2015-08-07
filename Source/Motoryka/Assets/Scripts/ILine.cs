using UnityEngine;
using System.Collections;

public interface ILine {
    void Init(Transform canvas);
    int VertexCount { get; }
    void SetColor(Color color);
    void SetSize(float size);
    void AddVertex(Vector2 pos);
    void AddVertex(Vector3 pos);
}
