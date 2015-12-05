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
    public class LineDrawer<T> : MonoBehaviour, ILineDrawer where T : ILine
    {
        public float frequency = 0.1f;
        public Color color = Color.red;
        public float size = 1;
        public GameObject canvas;
        public string sortingLayer;

        private List<Vector3> _vertices;
        private LineFactory<T> _factory;
        public bool _drawing;
        private List<ILine> _lines;
        private ILine _currentLine;
        private Vector3 _lastVertex;

        public bool BlockDrawing { get; set; }

        public LineDrawer()
        {
            _vertices = new List<Vector3>();
            _factory = new LineFactory<T>();
            _lines = new List<ILine>();
            _currentLine = null;
            BlockDrawing = false;
        }

        // Use this for initialization
        void Start()
        {
            if (canvas == null)
            {
                canvas = new GameObject("Canvas");
            }

            _factory.canvas = canvas;

            if (sortingLayer == "")
            {
                sortingLayer = "Lines";
            }

            _factory.sortingLayer = sortingLayer;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void StartDrawing()
        {
            if (BlockDrawing)
                return;
            Debug.Log("LineDrawer: starting drawing");
            _drawing = true;
            _currentLine = _factory.Create();
            _lines.Add(_currentLine);

            _currentLine.SetColor(color);
            _currentLine.SetSize(size);
            _vertices = new List<Vector3>();
        }

        public void Draw(Vector3 position)
        {
            if (!_drawing)
            {
                return;
            }

            if (_vertices.Count != 0 && Vector3.Distance(_lastVertex, position) < (1f / frequency))
            {
                return;
            }

            _currentLine.AddVertex(position);
            _vertices.Add(position);
            _lastVertex = position;
        }

        public void StopDrawing()
        {
            Debug.Log("LineDrawer: stoping drawing");
            _drawing = false;
            _currentLine = null;
        }

        public void SetSize(float size)
        {
            this.size = size;
        }

		public float GetSize() {
			return this.size;
		}

        public void SetFrequency(float f)
        {
            this.frequency = f;
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            throw new System.NotImplementedException();
        }

        public void DrawLine(Vector2[] vertices)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDrawing
        {
            get { return this._drawing; }
        }

        public ILine CurrentLine
        {
            get
            {
                return _currentLine;
            }
        }
    }

}