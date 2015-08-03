using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    public bool drawing = false;
    public Line currentlyDrawing = null;
    public LineFactory lf;
    public Camera camera;
    public Vector3 prevVertex;
	// Use this for initialization
	void Start () {
        lf = new LineFactory();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Graphics.DrawProcedural(MeshTopology.Quads())
	    if(Input.GetMouseButtonDown(0))
        {
            drawing = true;
            if(currentlyDrawing == null)
            {

                prevVertex = camera.ScreenToWorldPoint(Input.mousePosition);
                currentlyDrawing = lf.Create(prevVertex);
                currentlyDrawing.SetSize(0.1f);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            drawing = false;
            currentlyDrawing = null;
        }

        if(drawing && currentlyDrawing != null)
        {
            Vector3 newPos = camera.ScreenToWorldPoint(Input.mousePosition);
            if(Vector3.Distance(newPos, prevVertex) > 0.2)
            {
                currentlyDrawing.AddVertex(newPos);
                prevVertex = newPos;
            }
        }
	}
}
