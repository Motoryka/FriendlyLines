using UnityEngine;

namespace LineManagement
{
    public interface ILineDrawer
    {
        void SetColor(Color color);
        void StartDrawing();
        void Draw(Vector3 position);
        void StopDrawing();
        void SetSize(float size);
        void SetFrequency(float f);
        void DrawLine(Vector2 a, Vector2 b);
        void DrawLine(Vector2[] vertices);

        ILine CurrentLine { get; }

        bool IsDrawing { get; }
    }
}
