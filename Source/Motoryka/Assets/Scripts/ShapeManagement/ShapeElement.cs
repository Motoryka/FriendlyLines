using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;
using LineManagement.GLLines;

public class ShapeElement
{
	private Line shape;
    private Line startPoint;

	public ShapeElement () {
	}

    public ShapeElement(Line shape, Line startPoint)
    {
		this.shape = shape;
		this.startPoint = startPoint;
	}

    public Line Shape
    {
		get {
			return shape;
		}
		set {
			shape = value;
		}
	}

    public Line StartPoint
    {
		get {
			return startPoint;
		}
		set {
			startPoint = value;
		}
	}

    public void Preserve()
    {
        GameObject.DontDestroyOnLoad(shape);
        GameObject.DontDestroyOnLoad(startPoint);
    }

    public void DontPreserve()
    {
        GameObject.DestroyObject(shape);
        GameObject.DestroyObject(startPoint);
    }
}
