using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using LineManagement;

public class SceneManager : BaseLvlManager<SceneManager> {
    ShapeGenerator sGen;
    ILine shape;
    ILine userLine;
    PathAnalyser analizer;

    public override void Init()
    {
        sGen = GetComponent<ShapeGenerator>();
        analizer = new PathAnalyser();
    }

	// Use this for initialization
    protected override void PreStart()
    {
        shape = sGen.CreateShape(GameManager.Instance.GetCurrentShape());
    }

    public void RegisterUserLine(ILine line)
    {
        userLine = line;
    }

    public bool IsFinished()
    {
        if(userLine != null)
            return analizer.IsFinished(shape, userLine);
        return false;
    }

}
