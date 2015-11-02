using UnityEngine;
using System;

public class Colors {
	private Color lineColor;
	private Color pointColor;

	public Colors(Color line, Color point) {
		this.lineColor = line;
		this.pointColor = point;
	}

	public Color Line {
		get {
			return lineColor;
		}
		set {
			lineColor = value;
		}
	}
	
	public Color StartPoint {
		get {
			return pointColor;
		}
		set {
			pointColor = value;
		}
	}
}