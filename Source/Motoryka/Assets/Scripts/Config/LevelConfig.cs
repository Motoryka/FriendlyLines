public class LevelConfig 
{
	public int levelNumber { get; set; }

	public Shape shape { get; set; }

	public float lineStroke { get; set; }

	public PastelColor shapeColor { get; set; }

	public PastelColor brushColor { get; set; }

    public LevelConfig Copy()
    {
        var config = new LevelConfig
                         {
                             levelNumber = this.levelNumber,
                             shapeColor = this.shapeColor,
                             lineStroke = this.lineStroke,
                             brushColor = this.brushColor,
                             shape = this.shape
                         };

        return config;
    }
}
