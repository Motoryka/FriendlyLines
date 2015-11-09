using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LineManagement;

public class ShapeElement
{
	private ILine shape;
	private ILine startPoint;

	public ShapeElement () {
	}

	public ShapeElement (ILine shape, ILine startPoint) {
		this.shape = shape;
		this.startPoint = startPoint;
	}

	public ILine Shape {
		get {
			return shape;
		}
		set {
			shape = value;
		}
	}

	public ILine StartPoint {
		get {
			return startPoint;
		}
		set {
			startPoint = value;
		}
	}
}
