/**********************************************************************
Copyright (C) 2015  Wojciech Nadurski

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

using System.Collections;
using System.Collections.Generic;

using LineManagement;
using LineManagement.GLLines;

using UnityEngine;
using UnityEngine.UI;

public class SceneManager : BaseLvlManager<SceneManager>
{
    ShapeGenerator sGen;
    List<ILine> userLines;
    PathAnalyser analizer;
    
    public List<RuntimeAnimatorController> animations;

    public InputHandler inputHandler;

    public LineDrawer lineDrawer;

    public float collapsingTime = 0.3f;
    public Button pauseButton;
	public Text levelNumberText;

    bool drewThisRound = false;

    float idleSeconds = 2f;

    IEnumerator RestartingCoroutine;
    private bool isFinishing;

    bool isWaitingForRestart = false;
    float waitingSum = 0;
    bool isPaused = false;

    public override void Init()
    {
        isFinishing = false;
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
        this.sGen.size = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].lineStroke;

        this.lineDrawer.color = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].brushColor.Color;
        this.lineDrawer.size = GameManager.Instance._config.Levels[GameManager.Instance.CurrentLevel - 1].lineStroke;

		this.sGen.drawStartPoint = GameManager.Instance._config.DrawStartPoint;

        if (prevVertices == null)
        {
            shape = sGen.CreateShape(GameManager.Instance.GetCurrentShape());
        }
        else
		{
            shape = prevVertices;
        }

		this.levelNumberText.text = "Poziom " + GameManager.Instance.CurrentLevel + " / " + GameManager.Instance._config.NrOfLevels;

        drewThisRound = false;
    }

    protected override void PreFinish()
    {
        lineDrawer.StopDrawing();
        lineDrawer.BlockDrawing = true;
        var result = analizer.GetResult(shape.Shape, userLines);
        Debug.Log("Prefinish");
        Debug.Log("Wynik: " + result.shapeCovering + " % " + result.errorRange + " %");
        GameManager.Instance.ResultsList.Add(new LevelResult { levelNumber = GameManager.Instance.CurrentLevel, result = result });

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
        return analizer.IsStartCorrect(where, shape, userLines, GameManager.Instance._config.DrawStartPoint);
	}

    public void OnStopDraw()
    {
        if (drewThisRound)
		{
            Debug.Log("Wynik: " + analizer.GetResult(shape.Shape, userLines).shapeCovering + " %" + analizer.GetResult(shape.Shape, userLines).errorRange + " %");
            if (IsFinished())
            {
                if (!isFinishing)
                    GameManager.Instance.levelFinishedSound.Play();
                pauseButton.interactable = false;

                isFinishing = true;
                StartCoroutine(PreFinishAfterTime(2f));
            }
            else
            {
                RestartingCoroutine = RestartAfterIdleTime(GameManager.Instance.GameConfig.WaitingTime);
                StartCoroutine(RestartingCoroutine);
            }
        }
    }

    public void OnMove(Vector3 pos)
    {
        if (inputHandler.lineDrawer.IsDrawing && IsFinished())
        {
            if (!isFinishing)
            {
                GameManager.Instance.levelFinishedSound.Play();
            }
            pauseButton.interactable = false;
            isFinishing = true;
            StartCoroutine(PreFinishAfterTime(1f));
        }
    }

    public void OnStartDraw(Vector3 pos)
    {
        if (lineDrawer.BlockDrawing)
            return;
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
        waitingSum = 0;
        int iterations = (int)(seconds / 0.1f);

        for (int i = 0; i < iterations; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            waitingSum += 0.1f;
        }
        pauseButton.interactable = false;
        RestartLevel();
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            if (RestartingCoroutine != null)
            {
                StopCoroutine(RestartingCoroutine);
            }
        }
        else
        {
            if (waitingSum > 0f)
            {
                RestartingCoroutine = RestartAfterIdleTime(GameManager.Instance.GameConfig.WaitingTime - waitingSum +(0.1f));
                StartCoroutine(RestartingCoroutine);
            }
        }
    }

    public void BackGame()
    {
        GameManager.Instance.SendMessage("BackGame");
    }

    public void NextLevel()
    {
        if (!isFinishing)
        {
            lineDrawer.StopDrawing();

            Result result = new Result(0,0);
            Debug.Log("Wynik: " + result.shapeCovering + " %");
            GameManager.Instance.ResultsList.Add(new LevelResult { levelNumber = GameManager.Instance.CurrentLevel, result = result });

            GameManager.Instance.NextLevel();
        }
    }
}
