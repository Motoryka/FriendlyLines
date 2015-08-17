using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
    public LineDrawer lineDrawer;
    public Camera cam;

    public delegate void PressHandler();
    public PressHandler press;

    public delegate void ReleaseHandler();
    public ReleaseHandler release;

    public delegate void MoveHandler(Vector3 vector);
    public MoveHandler move;

    public delegate void InputHandling();

    InputHandling handleInput;

	// Use this for initialization
	void Start () {
        press += lineDrawer.StartDrawing;
        release += lineDrawer.StopDrawing;
        move += lineDrawer.Draw;
	
#if UNITY_ANDROID

        handleInput += _TouchInputHandler;

#elif UNITY_EDITOR || UNITY_STANDALONE

        handleInput += _MouseInputHandler;

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
                press();
            else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                release();
            else if (touch.phase == TouchPhase.Moved)
                move(cam.ScreenToWorldPoint(touch.position));

        }
    }

#endif

#if UNITY_EDITOR || UNITY_STANDALONE

    private void _MouseInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
            press();

        if (Input.GetMouseButtonUp(0))
            release();

        if (_mouseMoved())
            move(cam.ScreenToWorldPoint(Input.mousePosition));
    }

#endif

    bool _mouseMoved()
    {
        return Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f;
    }

}
