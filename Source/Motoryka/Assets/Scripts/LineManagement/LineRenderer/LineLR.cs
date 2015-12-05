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

namespace LineManagement.LineRendererLines
{
    using System;

    public class Line : MonoBehaviour, ILine
    {
        public string SortingLayer
        {
            get
            {
                return _sortingLayer;
            }
            set
            {
                _sortingLayer = value;
            }
        }

        private string _sortingLayer;
        private string _lrSortingLayer;
        private UnityEngine.LineRenderer _lineRenderer;
        private Vector3 _defaultPosition;
        private List<Vector3> _vertices;
        private int _lrVerticesCount;
        private float _size;
        private float _lrSize;
        private Color _color;
        private Color _lrColor;

        public Line()
        {
            _vertices = new List<Vector3>();
            _lrVerticesCount = 0;
            _size = 1;
            _lrSize = 1;
            _color = _lrColor = Color.white;
            _defaultPosition = new Vector3(0f, 0f, -1f);
            _lrSortingLayer = "";
        }

		public void CollapseToPoint(Vector2 v, float inTime)
		{
			throw new NotImplementedException();
		}

        // Use this for initialization
        void Start()
        {
            _lineRenderer = GetComponent<UnityEngine.LineRenderer>();

            _lineRenderer.SetColors(_color, _color);
            _lineRenderer.SetWidth(_size, _size);
        }

        // Update is called once per frame
        void Update()
        {
            while (_lrVerticesCount < _vertices.Count)
            {
                _lineRenderer.SetVertexCount(_lrVerticesCount + 1);
                _lineRenderer.SetPosition(_lrVerticesCount, _vertices[_lrVerticesCount]);

                _lrVerticesCount++;
            }

            if (_size != _lrSize)
            {
                _lrSize = _size;
                _lineRenderer.SetWidth(_lrSize, _lrSize);
            }

            if (_lrColor != _color)
            {
                _lrColor = _color;
                _lineRenderer.SetColors(_color, _color);
            }

            if (_lrSortingLayer != _sortingLayer)
            {
                _lrSortingLayer = _sortingLayer;
                _lineRenderer.sortingLayerName = _lrSortingLayer;
            }
        }

        public void SetVertice(int vertice, Vector2 v)
        {
            try
            {
                this._vertices[vertice] = v;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("Index out of range exception in SetVertice: " + e.Message);
            }
        }

        public void AddVertex(Vector2 pos)
        {
            AddVertex(new Vector3(pos.x, pos.y, _defaultPosition.z));
        }

        public void AddVertex(Vector3 pos)
        {
            pos.z = _defaultPosition.z;
            _vertices.Add(pos);
        }
        public void SetSize(float size)
        {
            _size = size;
        }
		public float GetSize() {
			return _size;
		}

        public int VertexCount
        {
            get
            {
                return _vertices.Count;
            }
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void Init(Transform canvas)
        {
            transform.parent = canvas;
        }


        public Color Color
        {
            get { return this._color; }
        }

        public List<Vector3> GetVertices()
        {
            return _vertices;
        }


        public List<Vector2> GetVertices2()
        {
            var l = new List<Vector2>();

            foreach (var v in _vertices)
            {
                l.Add(new Vector2(v.x, v.y));
            }

            return l;
        }


        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}