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

using System.Collections.Generic;

using UnityEngine;

namespace LineManagement.GLLines
{
    using System;

    public class Line : MonoBehaviour, ILine
    {
        float _defaultZ;
        List<Vector2> _triangleVertices;
        List<Vector2> _vertices;
        List<Vector2> _verticesAdded;
        float _thickness;
        float _newThickness;
        Color _color;
        Color _newColor;
        Material _mat;
		bool isCollapsing = false;
		Vector2 collapseTargetPoint;
		List<float> vertexAccellerations;
		List<float> previousVelocities;

        List<Vector2> verticesToCollapse;

        bool shouldRecompute = true;
        bool parentSet = false;

        Transform _parent;

        float _circleDensity = 24f;

        public Line()
        {
            _vertices = new List<Vector2>();
        }

        // Use this for initialization
        void Start()
        {
            
            _color = Color.red;
            _thickness = 1f;
            _defaultZ = 0f;

            recomputeTriangles();
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

		public void CollapseToPoint(Vector2 v, float collapseTime)
		{
			isCollapsing = true;
			collapseTargetPoint = v;

			vertexAccellerations = new List<float>();
			previousVelocities = new List<float>();
            verticesToCollapse = _vertices;

            foreach (Vector2 vertex in verticesToCollapse)
			{
				float a = 2f*(v - vertex).magnitude / (Mathf.Pow(collapseTime,2f));
				vertexAccellerations.Add(a);
				previousVelocities.Add (0f);
			}
		}

        void recomputeTriangles()
        {
            _triangleVertices = new List<Vector2>();
            _verticesAdded = new List<Vector2>();

            foreach (var v in _vertices)
            {
                _addVertex(v);
            }

            shouldRecompute = false;
        }

        void _addVertex(Vector2 v)
        {
            _verticesAdded.Add(v);

            if (_verticesAdded.Count == 1)
            {
                return;
            }
            else if (_verticesAdded.Count == 2)
            {
                Vector2 firstP = _verticesAdded[0];
                Vector2 secondP = _verticesAdded[1];

                Vector2 fline = secondP - firstP;
                Vector2 flineN = new Vector2(-fline.y, fline.x).normalized;

                _triangleVertices.Add(firstP + _thickness * flineN);
                _triangleVertices.Add(firstP - _thickness * flineN);
                _triangleVertices.Add(secondP + _thickness * flineN);

                _triangleVertices.Add(firstP - _thickness * flineN);
                _triangleVertices.Add(secondP + _thickness * flineN);
                _triangleVertices.Add(secondP - _thickness * flineN);
            }
            else
            {
                Vector2 p0 = _verticesAdded[_verticesAdded.Count - 3];
                Vector2 p1 = _verticesAdded[_verticesAdded.Count - 2];
                Vector2 p2 = _verticesAdded[_verticesAdded.Count - 1];

                _triangleVertices.AddRange(_generateRoundTrianglesForLines(p0, p1, p2));
            }

            if (_verticesAdded.Count > 3 && v == _verticesAdded[0])
            {
                Vector2 p0 = _verticesAdded[1];
                Vector2 p1 = _verticesAdded[0];
                Vector2 p2 = _verticesAdded[_verticesAdded.Count - 2];

                _triangleVertices.AddRange(_genJointCircle(p0, p1, p2));
            }
        }

        IEnumerable<Vector2> _generateRoundTrianglesForLines(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            List<Vector2> triangles = new List<Vector2>();

            Vector2 line2 = (p2 - p1).normalized;

            Vector2 n2 = new Vector2(-line2.y, line2.x).normalized;

            triangles.AddRange(_genJointCircle(p0,p1,p2));

            triangles.Add(p1 + _thickness * n2);
            triangles.Add(p1 - _thickness * n2);
            triangles.Add(p2 + _thickness * n2);

            triangles.Add(p1 - _thickness * n2);
            triangles.Add(p2 - _thickness * n2);
            triangles.Add(p2 + _thickness * n2);

            return triangles;
        }

        IEnumerable<Vector2> _genJointCircle(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            List<Vector2> triangles = new List<Vector2>();

            Vector2 line1 = (p1 - p0).normalized;
            Vector2 line2 = (p2 - p1).normalized;

            Vector2 n1 = new Vector2(-line1.y, line1.x).normalized;
            Vector2 n2 = new Vector2(-line2.y, line2.x).normalized;

            float scalar = Vector2.Dot(n1, n2);

            Vector2 r;

            if (Vector2.Dot(n1, line2) < 0)
            {
                r = n1 * _thickness;
            }
            else
            {
                r = -n2 * _thickness;
            }

            float angle = Mathf.Acos(scalar);

            triangles.AddRange(GraphicsProvider.GetCircleTriangles(p1, r, angle, Color.red, _circleDensity, -1f, _defaultZ));

            return triangles;
        }

        Vector2 calculateM(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Vector2 line = p1 - p0;
            Vector2 lineN = new Vector2(-line.y, line.x).normalized;

            Vector2 line2 = p2 - p1;
            //Vector2 line2N = new Vector2(-line2.y, line2.x).normalized;

            Vector2 t = (line.normalized + line2.normalized).normalized;

            Vector2 m = new Vector2(-t.y, t.x).normalized;

            float mLength = _thickness / (Vector2.Dot(m, lineN));

            return mLength * m.normalized;
        }

        // Update is called once per frame
        void Update()
        {
            if (shouldRecompute)
            {
                recomputeTriangles();
            }

            if (!parentSet && _parent)
            {
                transform.parent = _parent;
                parentSet = true;
            }

            UpdateThickness();
            UpdateColor();
            UpdateVertices();

			if(isCollapsing)
			{
				_collapseVertices();
			}
        }

		void _collapseVertices()
		{
            int inDestination = 0;

            for (int i = 0; i < verticesToCollapse.Count; ++i)
			{
                Vector2 line = (collapseTargetPoint - verticesToCollapse[i]);
                float v = previousVelocities[i] + vertexAccellerations[i] * Time.deltaTime;
                Vector2 direction = line.normalized;
                Vector2 newPos = verticesToCollapse[i] + direction * previousVelocities[i] * Time.deltaTime + direction * vertexAccellerations[i] * Mathf.Pow(Time.deltaTime, 2f) / 2;
                previousVelocities[i] = v;

                if ((verticesToCollapse[i] - collapseTargetPoint).magnitude < (newPos - collapseTargetPoint).magnitude)
                {
                    verticesToCollapse[i] = collapseTargetPoint;
                    inDestination++;
                }
                else
                {
                    verticesToCollapse[i] = newPos;
                }
            }

            if (inDestination == verticesToCollapse.Count)
            {
                isCollapsing = false;
            }
                
            recomputeTriangles();
		}

        void UpdateThickness()
        {
            if (_thickness != _newThickness)
            {
                _thickness = _newThickness;
                recomputeTriangles();
            }
        }

        void UpdateColor()
        {
            if (_color != _newColor)
            {
                _color = _newColor;
            }
        }

        void UpdateVertices()
        {
            while (_verticesAdded.Count < _vertices.Count)
            {
                _addVertex(_vertices[_verticesAdded.Count]);
            }
        }

        public void OnRenderObject()
        {
            UpdateThickness();
            UpdateColor();
            
            GraphicsProvider.DrawTriangles(transform, _color, _defaultZ, _triangleVertices);

            if (_verticesAdded.Count > 0 && (_verticesAdded[0] != _verticesAdded[_verticesAdded.Count - 1] || _verticesAdded.Count == 1))
            {
                _drawCircles();
            }
        }

        void _drawCircles()
        {
            if (_verticesAdded.Count == 0)
                return;

            if (_verticesAdded.Count < 2)
            {
                Vector2 p = _verticesAdded[0];
                Vector2 v = new Vector2(0f, _thickness);

                GraphicsProvider.DrawCircle(transform, p, v, 2*Mathf.PI, _color, _circleDensity, 1f, _defaultZ);
            }
            else
            {
                Vector2 p0 = _verticesAdded[0];
                Vector2 p1 = _verticesAdded[1];

                Vector2 line = (p0 - p1).normalized;
                Vector2 n = new Vector2(-line.y, line.x).normalized;

                Vector2 p = p0;
                Vector2 v = n * _thickness;

                GraphicsProvider.DrawCircle(transform, p, v, Mathf.PI, _color, _circleDensity, -1f, _defaultZ);

                p0 = _verticesAdded[_verticesAdded.Count - 1];
                p1 = _verticesAdded[_verticesAdded.Count - 2];

                line = (p0 - p1).normalized;
                n = new Vector2(-line.y, line.x).normalized;

                p = p0;
                v = n * _thickness;

                GraphicsProvider.DrawCircle(transform, p, v, Mathf.PI, _color, _circleDensity, -1f, _defaultZ);
            }
        }

        public string SortingLayer
        {
            get
            {
                return ""; //do smth
            }
            set
            {
                //to smthg
            }
        }

        public void Init(Transform canvas)
        {
            _parent = canvas;
        }

        public int VertexCount
        {
            get { return _vertices.Count; }
        }

        public Color Color
        {
            get { return _color; }
        }

        public void SetColor(Color color)
        {
            _newColor = color;
        }

        public void SetSize(float size)
        {
            _newThickness = size / 2;
        }

		public float GetSize() {
			return _newThickness;
		}

        public void AddVertex(Vector2 pos)
        {
            _vertices.Add(pos);
        }

        public void AddVertex(Vector3 pos)
        {
            AddVertex(new Vector2(pos.x, pos.y));
        }

        public List<Vector3> GetVertices()
        {
            var l = new List<Vector3>();

            foreach (var v in _vertices)
            {
                l.Add(new Vector3(v.x, v.y, _defaultZ));
            }

            return l;
        }

        public List<Vector2> GetVertices2()
        {
            return _vertices;
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }

    static class GraphicsProvider
    {
        static Material _mat;
        static GraphicsProvider()
        {
            _mat = new Material(Shader.Find("Sprites/Default"));
        }

        public static void DrawTriangles(Transform transform, Color color, float z, List<Vector2> triangleVertices)
        {
            _mat.SetPass(0);

            GL.PushMatrix();

            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.TRIANGLES);

            GL.Color(color);

            for (int i = 0; i < triangleVertices.Count - 2; i += 3)
            {
                //GL.Color(nextColor());

                for (int j = 0; j < 3; ++j)
                {
                    Vector2 v = triangleVertices[i + j];

                    GL.Vertex3(v.x, v.y, z);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        static public List<Vector2> GetCircleTriangles(Vector2 p, Vector2 r, float dAngle, Color color, float density, float direction = 1f, float z = 0f)
        {
            var vertices = new List<Vector2>();

            Vector2 prevP = p + r;

            density = dAngle * density / (2 * Mathf.PI);

            for (float angle = 0f; angle <= dAngle; angle += (dAngle / density))
            {
                float a = Mathf.Sign(direction) * (angle + (dAngle / density));

                Vector2 newV = new Vector2(r.x * Mathf.Cos(a) - r.y * Mathf.Sin(a),
                                            r.x * Mathf.Sin(a) + r.y * Mathf.Cos(a));
                Vector2 newP = p + newV;

                vertices.Add(p);
                vertices.Add(prevP);
                vertices.Add(newP);

                prevP = newP;
            }

            return vertices;
        }

        public static void DrawCircle(Transform transform, Vector2 p, Vector2 r, float dAngle, Color color, float density, float direction = 1f, float z = 0f)
        {
            _mat.SetPass(0);
            density = dAngle * density / (2 * Mathf.PI);

            GL.PushMatrix();

            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.TRIANGLES);
            GL.Color(color);

            Vector2 prevP = p + r;


            for (float angle = 0f; angle <= dAngle; angle += (dAngle / density))
            {
                float a = Mathf.Sign(direction) * (angle + (dAngle / density));

                Vector2 newV = new Vector2(r.x * Mathf.Cos(a) - r.y * Mathf.Sin(a),
                                            r.x * Mathf.Sin(a) + r.y * Mathf.Cos(a));
                Vector2 newP = p + newV;

                GL.Vertex3(p.x, p.y, z);
                GL.Vertex3(prevP.x, prevP.y, z);
                GL.Vertex3(newP.x, newP.y, z);

                prevP = newP;
            }
            GL.End();

            GL.PopMatrix();
        }
    }


    public static class CameraExtensions
    {
        public static Bounds OrthographicBounds(this Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }
    }
}