using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using LineManagement;
using LineManagement.GLLines;

public class SceneManager : BaseLvlManager<SceneManager>
{
    ShapeGenerator sGen;
    ILine userLine;
    PathAnalyser analizer;

    public InputHandler inputHandler;

    public LineDrawer lineDrawer;

    bool drewThisRound = false;

    public override void Init()
    {
        if (sGen != null)
            sGen = GetComponent<ShapeGenerator>();
        else
        {
            sGen = gameObject.AddComponent<ShapeGenerator>();
        }
        analizer = new PathAnalyser();
    }

    // Use this for initialization
    protected override void PreStart()
    {
        if (inputHandler == null)
            inputHandler = GameObject.FindObjectOfType<InputHandler>();

        if (inputHandler != null)
        {
            inputHandler.press += OnStartDraw;
            inputHandler.release += OnStopDraw;
            inputHandler.move += OnMove;
        }

        if (this.lineDrawer == null)
        {
            this.lineDrawer = GameObject.FindObjectOfType<LineDrawer>();
        }

        ShapeElement prevVertices = GameManager.Instance.GetPreviousShapeVertices();

        this.sGen.color = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].shapeColor;
        this.sGen.size = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].shapeStroke;

        this.lineDrawer.color = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].brushColor;
        this.lineDrawer.size = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].brushStroke;

        if (prevVertices == null)
        {
            shape = sGen.CreateShape(GameManager.Instance.GetCurrentShape());
        }
        else
        {
            shape = sGen.CreateShape(prevVertices);
        }

        drewThisRound = false;

        //this.shape = this.sGen.CreateShape(this.sGen.CollapseShape(shape));
    }

    protected override void PreFinish()
    {
        Debug.Log("Prefinish");
        float time = 0.4f;
        lineDrawer.StopDrawing();
        shape.Shape.CollapseToPoint(Vector2.zero, time);
        shape.StartPoint.CollapseToPoint(Vector2.zero, time);
        userLine.CollapseToPoint(Vector2.zero, time);
    }

    public void RegisterUserLine(ILine line)
    {
        userLine = line;
    }

    public bool IsFinished()
    {
        if (userLine != null)
            return analizer.IsFinished(shape.Shape, userLine);
        return false;
    }

	public bool IsStartCorrect(Vector3 where) 
	{
		if (where != null)
			return analizer.IsStartCorrect (where, shape.Shape);
		return false;
	}

    public void OnStopDraw()
    {
        if (drewThisRound)
        {
            if (IsFinished())
            {
                CurrentPhase = LevelPhase.Finished;
            }
            else
            {
                RestartLevel();
            }
        }
    }

    public void OnMove(Vector3 pos)
    {
        if (inputHandler.lineDrawer.IsDrawing && IsFinished())
        {
            CurrentPhase = LevelPhase.Finished;
        }
    }

    public void OnStartDraw(Vector3 pos)
    {
        drewThisRound = true;
        RegisterUserLine(inputHandler.lineDrawer.CurrentLine);
    }

}
