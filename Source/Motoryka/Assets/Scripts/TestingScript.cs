using UnityEngine;
using System.Collections;

public class TestingScript : MonoBehaviour {
    public LineDrawer drawer;
    public Camera cam;
    public Color color;

	// Use this for initialization
	void Start () {
        drawer.SetSize(0.2f);
        drawer.SetColor(color);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Graphics.DrawProcedural(MeshTopology.Quads())
	    if(Input.GetMouseButtonDown(0))
        {
            drawer.StartDrawing();
        }

        if(Input.GetMouseButtonUp(0))
        {
            drawer.StopDrawing();
        }

        if(drawer.IsDrawing)
        {
            drawer.Draw(cam.ScreenToWorldPoint(Input.mousePosition));
        }
	}
}
