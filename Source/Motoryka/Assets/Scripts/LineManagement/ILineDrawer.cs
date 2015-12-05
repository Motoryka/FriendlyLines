/**********************************************************************
Copyright (C) 2015  Wojciech Nadurski

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

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
