using UnityEngine;
using System.Collections;

public interface ILine {
    int vertexCount { get; set; }
    void SetColor(Color color);
    void SetSize(float size);
    void AddVertex(Vector2 pos);
}
