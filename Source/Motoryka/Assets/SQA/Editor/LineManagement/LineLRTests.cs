using UnityEngine;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class LineLRTests {

	[Test]
    public void AddVertex()
    {
        ILine line = new LineLR();
        line.AddVertex(new Vector2(2, 1));

        Assert.AreEqual(line.VertexCount, 1);
    }

    [Test]
    public void SetColor()
    {
        ILine line = new LineLR();
        Color c = Color.red;

        line.SetColor(c);

        Assert.AreEqual(line.Color, c);
    }
}
