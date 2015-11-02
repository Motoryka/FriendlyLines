using UnityEngine;
using System.Collections;
using NUnit.Framework;

using LineManagement;
using LineManagement.GLLines;

[TestFixture]
public class LineLRTests {

	[Test]
    public void AddVertex()
    {
        ILine line = new Line();
        line.AddVertex(new Vector2(2, 1));

        Assert.AreEqual(line.VertexCount, 1);
    }

    [Test]
    public void SetColor()
    {
        ILine line = new Line();
        Color c = Color.red;

        line.SetColor(c);

        Assert.AreEqual(line.Color, c);
    }
}
