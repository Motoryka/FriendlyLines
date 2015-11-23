﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using LineManagement;
using LineManagement.GLLines;

public class SceneManager : BaseLvlManager<SceneManager>
{
    ShapeGenerator sGen;
    List<ILine> userLines;
    PathAnalyser analizer;
    
    public List<RuntimeAnimatorController> animations;

    public InputHandler inputHandler;

    public LineDrawer lineDrawer;

    public float collapsingTime = 0.3f;

    bool drewThisRound = false;

    float idleSeconds = 2f;

    IEnumerator RestartingCoroutine;

    public override void Init()
    {
        if (sGen != null)
            sGen = GetComponent<ShapeGenerator>();
        else
        {
            sGen = gameObject.AddComponent<ShapeGenerator>();
        }
        analizer = new PathAnalyser();

        userLines = new List<ILine>() ;
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

        this.sGen.color = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].shapeColor.Color;
        this.sGen.size = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].shapeStroke;

        this.lineDrawer.color = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].brushColor.Color;
        this.lineDrawer.size = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].brushStroke;

		this.sGen.drawStartPoint = GameManager.Instance._config.DrawStartPoint;

        if (prevVertices == null)
        {
            shape = sGen.CreateShape(GameManager.Instance.GetCurrentShape());
        }
        else
        {

            shape = prevVertices;//sGen.CreateShape(prevVertices);
        }

        drewThisRound = false;

        //this.shape = this.sGen.CreateShape(this.sGen.CollapseShape(shape));
    }

    protected override void PreFinish()
    {
        var result = analizer.GetResult(shape.Shape, userLines);
        Debug.Log("Prefinish");
        Debug.Log("Wynik: " + result + " %");
        GameManager.Instance.ResultsList.Add(new LevelResult { levelNumber = GameManager.Instance.CurrentLevel, result = (int)result });
		lineDrawer.StopDrawing();

        shape.Shape.CollapseToPoint(Vector2.zero, collapsingTime);
        shape.StartPoint.CollapseToPoint(Vector2.zero, collapsingTime);
        userLines.ForEach( line => line.CollapseToPoint(Vector2.zero, collapsingTime) );

        StartCoroutine(FinishAfterTime(collapsingTime));
    }

    IEnumerator FinishAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        CurrentPhase = LevelPhase.Finished;
    }

    protected override void PostFinish()
    {
        userLines.ForEach( line => line.Delete() );
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animations[Random.Range(0, animations.Count)];
        animator.SetTrigger("finished");
        Debug.Log("Animation trigger is set");
    }



    public void RegisterUserLine(ILine line)
    {
        userLines.Add(line);
    }

    public bool IsFinished()
    {
        if (userLines.Count > 0)
            return analizer.IsFinished(shape.Shape, userLines);
        return false;
    }

	public bool IsStartCorrect(Vector3 where) 
	{
		return analizer.IsStartCorrect (where, shape, userLines);
	}

    public void OnStopDraw()
    {
        if (drewThisRound)
		{
            Debug.Log("Wynik: " + analizer.GetResult(shape.Shape, userLines) + " %");
            if (IsFinished())
            {
                //CurrentPhase = LevelPhase.Prefinished;
                StartCoroutine(PreFinishAfterTime(GameManager.Instance.GameConfig.WaitingTime));
            }
            else
            {
                //RestartLevel();
                RestartingCoroutine = RestartAfterIdleTime(3f);
                StartCoroutine(RestartingCoroutine);
            }
        }
    }

    public void OnMove(Vector3 pos)
    {
        if (inputHandler.lineDrawer.IsDrawing && IsFinished())
        {
            StartCoroutine(PreFinishAfterTime(1f));
        }
    }

    public void OnStartDraw(Vector3 pos)
    {
        drewThisRound = true;
        RegisterUserLine(inputHandler.lineDrawer.CurrentLine);
        if(RestartingCoroutine != null)
        {
            StopCoroutine(RestartingCoroutine);
        }
    }

    IEnumerator PreFinishAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CurrentPhase = LevelPhase.Prefinished;
    }

    IEnumerator RestartAfterIdleTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        RestartLevel();
    }
}
