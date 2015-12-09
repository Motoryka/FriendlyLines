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

using LineManagement.GLLines;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public LineDrawer lineDrawer;
    public Camera cam;

    public GameObject shadow;
    public Animator pausePanel;

    public EventSystem EventSystemManager;

    public delegate void PressHandler(Vector3 where);
    public PressHandler press;

    public delegate void ReleaseHandler();
    public ReleaseHandler release;

    public delegate void MoveHandler(Vector3 vector);
    public MoveHandler move;

    public delegate void InputHandling();

    InputHandling handleInput;
    private bool drawingEnabled = true;

    private bool isPaused = false;

	// Use this for initialization
	void Start() 
    {
        Debug.Log("Adding to press");
        press += StartDrawing;
        release += StopDrawing;
        move += Move;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        handleInput += _MouseInputHandler;

#elif UNITY_ANDROID

        handleInput += _TouchInputHandler;

#endif
    }
	
	// Update is called once per frame
	void Update()
    {
        if (isPaused)
            return;
        if (handleInput != null)
            handleInput();
	}

#if UNITY_ANDROID

    private void _TouchInputHandler()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystemManager.IsPointerOverGameObject())
                    return;

                if (SceneManager.Instance.IsStartCorrect(cam.ScreenToWorldPoint(touch.position)))
                {
                    press(cam.ScreenToWorldPoint(touch.position));
                }
            }
            else if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) && lineDrawer.IsDrawing)
            {
                if(lineDrawer.IsDrawing)
                    release();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                move(cam.ScreenToWorldPoint(touch.position));
            }
        }
    }

#endif

#if UNITY_EDITOR || UNITY_STANDALONE

    private void _MouseInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystemManager.IsPointerOverGameObject())
                return;
            if (press != null)
            {
                if (SceneManager.Instance.IsStartCorrect(cam.ScreenToWorldPoint(Input.mousePosition)))
                {
                    press(cam.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            else
            {
                Debug.Log("press is null!");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (lineDrawer.IsDrawing)
            {
                release();
            }
        }

        if (_mouseMoved())
        {
            move(cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }

#endif

    bool _mouseMoved()
    {
        return Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f;
    }

    void Move(Vector3 pos)
    {
        lineDrawer.Draw(pos);
    }
    void StartDrawing(Vector3 where)
    {
        Debug.Log("Input: starting drawing");

		if (drawingEnabled && SceneManager.Instance.IsStartCorrect(where))
        {
            Debug.Log("Input: starting drawing");
            lineDrawer.StartDrawing();

            lineDrawer.Draw(where);
        }
    }

    void StopDrawing()
    {
        if (drawingEnabled && lineDrawer.IsDrawing)
        {
            Debug.Log("Input: stopping drawing");
            lineDrawer.StopDrawing();
        }
    }

    bool IsFinished()
    {
        return SceneManager.Instance.IsFinished();
    }

    public void PauseGameClicked()
    {
        isPaused = !isPaused;
        shadow.SetActive(isPaused);
        pausePanel.SetBool("IsPaused", isPaused);
        SceneManager.Instance.SendMessage("PauseGame");
    }

    public void BackGame(Button sender)
    {
        Debug.Log("Game back");
        sender.interactable = false;
        SceneManager.Instance.SendMessage("BackGame");
    }

    public void NextLevel(Button sender)
    {
        Debug.Log("Next Level");
        sender.interactable = false;
        SceneManager.Instance.SendMessage("NextLevel");
    }
}
