using UnityEngine;
using System.Collections;

public interface ILineDrawer  {

    void SetColor(Color color);
    void StartDrawing();
    void Draw(Vector2 position);
    void StopDrawing();
    void SetSize(float size);
    void SetFrequency(float f);
    void DrawLine(Vector2 a, Vector2 b);
    void DrawLine(Vector2[] vertices);

}
