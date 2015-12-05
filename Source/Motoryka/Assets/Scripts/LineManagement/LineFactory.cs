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
using System;
using System.Collections.Generic;

namespace LineManagement
{
    public class LineFactory<T> where T : ILine
    {
        public GameObject canvas = null;
        public string sortingLayer = "Shapes";

        static private GameObject _linePrefab = null;
        static private string _linePrefabName = "Prefabs/LinePrefab";
        static private string _lineGLPrefabName = "Prefabs/LineGLPrefab";

        public T Create()
        {
            T newLine = default(T);

            if (isLineLR(typeof(T)))
            {
                if (_linePrefab == null)
                    _linePrefab = (GameObject)Resources.Load(_linePrefabName);

                /*  var newLineGameObject = ((GameObject)GameObject.Instantiate(_linePrefab, Vector3.zero, Quaternion.identity)).GetComponent<T>();

                  if (canvas != null)
                      newLineGameObject.Init(canvas.transform);

                  newLine = newLineGameObject;
                  newLine.SortingLayer = sortingLayer;*/
            }
            else if (isLineGL(typeof(T)))
            {
                if (_linePrefab == null)
                    _linePrefab = (GameObject)Resources.Load(_lineGLPrefabName);

            }

            var newLineGameObject = ((GameObject)GameObject.Instantiate(_linePrefab, Vector3.zero, Quaternion.identity)).GetComponent<T>();

            if (canvas != null)
                newLineGameObject.Init(canvas.transform);

            newLine = newLineGameObject;
            newLine.SortingLayer = sortingLayer;

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

        public T Create(List<Vector2> vertices)
        {
            if (vertices.Count == 0)
                return Create();

            T newLine = Create(vertices[0]);

            foreach (Vector2 v in vertices)
                newLine.AddVertex(v);

            return newLine;
        }

        private bool isLineLR(Type t)
        {
            return t == typeof(LineManagement.LineRendererLines.Line);
        }

        private bool isLineGL(Type t)
        {
            return t == typeof(LineManagement.GLLines.Line);
        }

    }

}