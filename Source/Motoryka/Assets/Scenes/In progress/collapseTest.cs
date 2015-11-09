using UnityEngine;
using System.Collections;
using LineManagement;

public class collapseTest : MonoBehaviour {
    public ShapeGenerator sg;
    public ShapeElement se;
    public float timeToCollapse = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenTriangle()
    {
        Debug.Log("Creating shape");
        sg.color = Color.red;
        sg.size = 1f;
        se = sg.CreateSquare();
    }

    public void deleteShape()
    {
        se.Shape.Delete();
        se.StartPoint.Delete();
        se = null;
    }

    public void CollapseShape()
    {
        se.Shape.CollapseToPoint(Vector2.zero, timeToCollapse);
        se.StartPoint.CollapseToPoint(Vector2.zero, timeToCollapse);
    }
}
