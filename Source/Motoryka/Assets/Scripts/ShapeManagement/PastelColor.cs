using System.Collections.Generic;

using UnityEngine;

public class PastelColor
{
    public string Name { get; set; }

    public Color Color { get; set; }

	public PastelColor()
    {
	}

    public PastelColor(string name, Color color)
    {
        this.Name = name;
        this.Color = color;
    }
}

public class PastelColorFactory
{
    private static List<PastelColor> colorList;

    static PastelColorFactory()
    {
        colorList = new List<PastelColor>();
        LightRed = AddColorToList("Jasny czerwony", new Color(1f, 0.702f, 0.702f));
        LightGreen = AddColorToList("Jasny zielony", new Color(0.6f, 1f, 0.608f));
        LightBlue = AddColorToList("Jasny niebieski", new Color(0.702f, 0.898f, 1f));
        LightYellow = AddColorToList("Jasny żółty", new Color(1f, 1f, 0.6f));
        LightPink = AddColorToList("Jasny różowy", new Color(0.871f, 0.647f, 0.643f));
        Red = AddColorToList("Czerwony", new Color(1f, 0.412f, 0.38f));
        Green = AddColorToList("Zielony", new Color(0.467f, 0.867f, 0.467f));
        Blue = AddColorToList("Niebieski", new Color(0.682f, 0.776f, 0.812f));
        Yellow = AddColorToList("Żółty", new Color(0.92f, 0.92f, 0.588f));
        Pink = AddColorToList("Różowy", new Color(1f, 0.82f, 0.863f));
        Orange = AddColorToList("Pomarańczowy", new Color(1f, 0.702f, 0.278f));
        Purple = AddColorToList("Fioletowy", new Color(0.392f, 0.078f, 0.392f));
        Mint = AddColorToList("Miętowy", new Color(0.6f, 1f, 0.8f));
        Gray = AddColorToList("Szary", new Color(0.812f, 0.812f, 0.769f));
        Black = AddColorToList("Czarny", new Color(0f, 0f, 0f));
        DarkRed = AddColorToList("Ciemny czerwony", new Color(0.761f, 0.231f, 0.133f));
        DarkGreen = AddColorToList("Ciemny zielony", new Color(0.012f, 0.753f, 0.235f));
        DarkBlue = AddColorToList("Ciemny niebieski", new Color(0.467f, 0.62f, 0.796f));
        DarkGray = AddColorToList("Ciemny szary", new Color(0.4f, 0.4f, 0.4f));
    }

    public static PastelColor RandomColor 
    { 
		get 
        {
			PastelColor color;
			do{	color = colorList[Random.Range(0, colorList.Count - 1)]; }
			while(color.Name == "Czarny");

			return color;
		} 
	}

	public static PastelColor RandomColorWithExclude(PastelColor exclude)
    {
		PastelColor color;
		do{	color = colorList[Random.Range(0, colorList.Count - 1)]; }
		while(color.Name == "Czarny" || color == exclude);
		
		return color;
	}

    public static PastelColor LightRed { get; internal set; }
    public static PastelColor LightGreen { get; internal set; }
    public static PastelColor LightBlue { get; internal set; }
    public static PastelColor LightYellow { get; internal set; }
    public static PastelColor LightPink { get; internal set; }
    public static PastelColor Red { get; internal set; }
    public static PastelColor Green { get; internal set; }
    public static PastelColor Blue { get; internal set; }
    public static PastelColor Yellow { get; internal set; }
    public static PastelColor Pink { get; internal set; }
    public static PastelColor Orange { get; internal set; }
    public static PastelColor Purple { get; internal set; }
    public static PastelColor Mint { get; internal set; }
    public static PastelColor Gray { get; internal set; }
    public static PastelColor Black { get; internal set; }
    public static PastelColor DarkRed { get; internal set; }
    public static PastelColor DarkGreen { get; internal set; }
    public static PastelColor DarkBlue { get; internal set; }
    public static PastelColor DarkGray { get; internal set; }

    private static PastelColor AddColorToList(string name, Color color)
    {
        var pastelColor = new PastelColor(name, color);
        colorList.Add(pastelColor);
        return pastelColor;
    }

    public static List<PastelColor> ColorList { get { return colorList; } }

    public static string GetColorName(Color c)
    {
        foreach(PastelColor pc in colorList)
        {
            if (pc.Color == c)
                return pc.Name;
        }

        return "Nieznany";
    }


    public static PastelColor GetColor(string name)
    {
        foreach (PastelColor pc in colorList)
        {
            if (pc.Name == name)
                return pc;
        }

        return Black;
    }
}
