using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using LineManagement;

public class SceneManager : BaseLvlManager<SceneManager>
{
    ShapeGenerator sGen;
    ILine shape;
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

        shape = sGen.CreateShape(GameManager.Instance.GetCurrentShape());
        drewThisRound = false;
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
