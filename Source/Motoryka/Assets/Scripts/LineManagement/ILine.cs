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