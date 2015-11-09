using UnityEngine;
using System.Collections;
using LineManagement.GLLines;

public class InputHandler : MonoBehaviour {
    public LineDrawer lineDrawer;
    public Camera cam;

    public delegate void PressHandler(Vector3 where);
    public PressHandler press;

    public delegate void ReleaseHandler();
    public ReleaseHandler release;

    public delegate void MoveHandler(Vector3 vector);
    public MoveHandler move;

    public delegate void InputHandling();

    InputHandling handleInput;
    bool _isLine = false;

	// Use this for initialization
	void Start () {
        Debug.Log("Adding to press");
        press += StartDrawing;
        release += StopDrawing;
        move += Move;

#if UNITY_EDITOR

        handleInput += _MouseInputHandler;

#elif UNITY_ANDROID || UNITY_STANDALONE

        handleInput += _TouchInputHandler;

#endif

    }
	
	// Update is called once per frame
	void Update () {
        
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
                move(cam.ScreenToWorldPoint(touch.position));

        }
    }

#endif

#if UNITY_EDITOR || UNITY_STANDALONE

    private void _MouseInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("There was a press");
            if (press != null)
            {
                if (SceneManager.Instance.IsStartCorrect(cam.ScreenToWorldPoint(Input.mousePosition)))
                {
                    press(cam.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            else
                Debug.Log("press is null!");
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("There was a release");
            if (lineDrawer.IsDrawing)
                release();
        }

        if (_mouseMoved())
            move(cam.ScreenToWorldPoint(Input.mousePosition));
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

		if (!_isLine && SceneManager.Instance.IsStartCorrect(where))
        {
            Debug.Log("Input: starting drawing");
            lineDrawer.StartDrawing();

            lineDrawer.Draw(where);
        }
    }

    void StopDrawing()
    {
        if (!_isLine && lineDrawer.IsDrawing)
        {
            Debug.Log("Input: stopping drawing");
            lineDrawer.StopDrawing();

            _isLine = true;
        }
    }

    bool IsFinished()
    {
        return SceneManager.Instance.IsFinished();
    }
}
