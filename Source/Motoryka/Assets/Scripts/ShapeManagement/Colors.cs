using UnityEngine;
using System;

public class Colors {
	private Color lineColor;
	private Color pointColor;

	public Colors(Color line, Color point) {
		this.lineColor = line;
		this.pointColor = point;
	}

	public Color GetLineColor() {
		return lineColor;
	}

	public Color GetPointColor() {
		return pointColor;
	}
}