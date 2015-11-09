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

    bool drewThisRound = false;

    public override void Init()
    {
        sGen = GetComponent<ShapeGenerator>();
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

        List<Vector2> prevVertices = GameManager.Instance.GetPreviousShapeVertices();

        if (prevVertices == null)
        {
            shape = sGen.CreateShape(GameManager.Instance.GetCurrentShape());
        }
        else
        {
            shape = sGen.CreateShape(prevVertices);
        }

        drewThisRound = false;
    }

    protected override void PreFinish()
    {
//        base.PostFinish();
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("finished");
        Debug.Log("Animation trigger is set");
    }

    public void RegisterUserLine(ILine line)
    {
        userLine = line;
    }

    public bool IsFinished()
    {
        if (userLine != null)
            return analizer.IsFinished(shape, userLine);
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
