using LineManagement.GLLines;

using UnityEngine;

public class ShapeElement
{
	private Line shape;
    private Line startPoint;
	private Shape type;

	public ShapeElement () 
    {
	}

    public ShapeElement(Line shape, Line startPoint, Shape type)
    {
		this.shape = shape;
		this.startPoint = startPoint;
		this.type = type;
	}

    public Line Shape
    {
		get 
        {
			return shape;
		}
		set 
        {
			shape = value;
		}
	}

    public Line StartPoint
    {
		get 
        {
			return startPoint;
		}
		set 
        {
			startPoint = value;
		}
	}

	public Shape Type
	{
		get 
        {
			return type;
		}
		set 
        {
			type = value;
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

	public static string GetShapeName(Shape s)
	{
		switch(s)
		{
		case global::Shape.HorizontalLine:
			return "Linia pozioma";
			break;
		case global::Shape.VerticalLine:
			return "Linia pionowa";
			break;
		case global::Shape.DiagonalLine:
			return "Linia ukośna";
			break;
		case global::Shape.CurvedLine:
			return "Linia krzywa";
			break;
		case global::Shape.Triangle:
			return "Trójkąt";
			break;
		case global::Shape.Circle:
			return "Okrąg";
			break;
		case global::Shape.Ellipse:
			return "Elipsa";
			break;
		case global::Shape.Square:
			return "Kwadrat";
			break;
		case global::Shape.Rectangle:
			return "Prostokąt";
			break;
		default:
			return "Nieznany";
			break;
		}
	}
}
