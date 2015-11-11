using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour {

    public Text titleLbl;
    public ComboBox ShapeTypeCB;
    public Slider ShapeWidthSlider;
    public Slider BrushWidthSlider;
    public ComboBox ShapeColorCB;
    public ComboBox BrushColorCB;
    public Toggle StartPointToggle;
    public Vector3 StayingPoint;

    LevelConfig cfg;

    ConfigCreator creator;
    float movingVelocity = 1000f;

    public float smoothTime = 0.2F;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, StayingPoint, ref velocity, smoothTime);
	}

    public void Init(ConfigCreator ccreator, LevelConfig lcfg)
    {
        creator = ccreator;
        cfg = lcfg;

        titleLbl.text = "Edytujesz poziom " + cfg.levelNumber + ".";

        ShapeTypeCB.SetChoice((int)cfg.shape);
        ShapeWidthSlider.value = (float)LineStroke.FloatToInt(lcfg.shapeStroke);
        BrushWidthSlider.value = (float)LineStroke.FloatToInt(cfg.brushStroke);
        ShapeColorCB.SetChoice(PastelColorFactory.GetColorName(cfg.shapeColor));
        BrushColorCB.SetChoice(PastelColorFactory.GetColorName(cfg.brushColor));
        StartPointToggle.isOn = true;
        StartPointToggle.enabled = false;

        ShapeTypeCB.OnOptionChange += SetShape;
        ShapeColorCB.OnOptionChange += SetShapeColor;
        BrushColorCB.OnOptionChange += SetBrushColor;

        StayingPoint = transform.localPosition;
    }

    private void SetBrushColor(GameObject sender, string newValue, int index)
    {
        cfg.brushColor = PastelColorFactory.GetColor(newValue).Color;
    }

    private void SetShapeColor(GameObject sender, string newValue, int index)
    {
        cfg.shapeColor = PastelColorFactory.GetColor(newValue).Color;
    }

    private void SetShape(GameObject sender, string newValue, int index)
    {
        cfg.shape = (Shape)index;
    }

    public void SetShapeStroke()
    {
        cfg.shapeStroke = LineStroke.IntToFloat((int)ShapeWidthSlider.value);
    }

    public void SetBrushStroke()
    {
        cfg.brushStroke = LineStroke.IntToFloat((int)BrushWidthSlider.value);
    }

    public void SetActiveLevelNext()
    {
        creator.SendMessage("SetActiveLevelNext");
    }

    public void SetActiveLevelPrevious()
    {
        creator.SendMessage("SetActiveLevelPrevious");
    }

    public void SaveConfig()
    {
        creator.SendMessage("SaveConfig");
    }

    public void MoveToPoint(Vector3 point)
    {

        StayingPoint = point;
        //StartCoroutine(MoveToPointCrt(point));
    }

    IEnumerator MoveToPointCrt(Vector3 point)
    {
        Debug.Log("Moving to point: " + point);
        velocity = Vector3.zero;
        while (transform.localPosition != point)
        {
            //transform.localPosition = Vector3.MoveTowards(transform.localPosition, point, movingVelocity * Time.deltaTime);

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, point, ref velocity, smoothTime);
            
            yield return new WaitForEndOfFrame();
        }
    }
}
