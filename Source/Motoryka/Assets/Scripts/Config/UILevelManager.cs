using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour {

    public Text titleLbl;
    public Slider LineStrokeSlider;
    public Vector3 StayingPoint;
    public Dropdown ShapeDropdown;
    public Dropdown LineColorDropdown;
    public Dropdown ShapeColorDropdown;

    public Button PreviousLevelBtn;
    public Button NextLevelBtn;

    public Button AddPrevLevelBtn;
    public Button AddNextLevelBtn;

    public Button DeleteBtn;

    LevelConfig cfg;

    ConfigCreator creator;

    int myindex;
    int maxindex;

    public float smoothTime = 0.2F;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, StayingPoint, ref velocity, smoothTime);
	}

    public void Init(ConfigCreator ccreator, LevelConfig lcfg, int levelindex, int maxindex)
    {
        creator = ccreator;
        cfg = lcfg;
        myindex = levelindex;
        this.maxindex = maxindex;

        creator.LevelAdded += new ConfigCreator.LevelAddHandler(LevelInserted);
        creator.LevelDeleted += LevelDeleted;

        UpdateTitle();

        LineStrokeSlider.value = (float)LineStroke.FloatToInt(lcfg.lineStroke);
        LineStrokeSlider.onValueChanged.AddListener(f => SetStroke(f));

        ShapeDropdown.options.Clear();
        foreach (string option in ShapeConverter.GetShapeStringArray())
            ShapeDropdown.options.Add(new Dropdown.OptionData(option));
        ShapeDropdown.onValueChanged.AddListener(i => UpdateShape(i));

        int index = 0;
        foreach (var option in ShapeDropdown.options)
        {
            if (option.text == ShapeConverter.shapeToString(lcfg.shape))
            {
                // Strange workaround
                ShapeDropdown.value = index + 1;
                ShapeDropdown.value = index;
                break;
            }

            index++;
        }

        LineColorDropdown.options.Clear();
		//LineColorDropdown.options.Add (new Dropdown.OptionData("Losowy"));
        foreach (PastelColor option in PastelColorFactory.ColorList)
            LineColorDropdown.options.Add(new Dropdown.OptionData(option.Name));
        LineColorDropdown.onValueChanged.AddListener(i => UpdateLineColor(i));

        index = 0;
        foreach (var option in LineColorDropdown.options)
        {
            if (option.text == lcfg.brushColor.Name)
            {
                // Strange workaround
                LineColorDropdown.value = index+1;
                LineColorDropdown.value = index;
                break;
            }

            index++;
        }

        ShapeColorDropdown.options.Clear();
		//ShapeColorDropdown.options.Add(new Dropdown.OptionData("Losowy"));
        foreach (PastelColor option in PastelColorFactory.ColorList)
            ShapeColorDropdown.options.Add(new Dropdown.OptionData(option.Name));
        ShapeColorDropdown.onValueChanged.AddListener(i => UpdateShapeColor(i));

        index = 0;
        foreach (var option in ShapeColorDropdown.options)
        {
            if (option.text == lcfg.shapeColor.Name)
            {
                // Strange workaround
                ShapeColorDropdown.value = index + 1;
                ShapeColorDropdown.value = index;
                break;
            }

            index++;
        }

        StayingPoint = transform.localPosition;

        AddNextLevelBtn.onClick.AddListener(() => creator.SendMessage("AddLevel", myindex+1) );
        AddPrevLevelBtn.onClick.AddListener(() => creator.SendMessage("AddLevel", myindex));

        updateBtns();
    }

    public void UpdateTitle()
    {
        titleLbl.text = "Edytujesz poziom " + cfg.levelNumber + ".";
    }

    private void UpdateLineColor(int i)
    {
		/*if(LineColorDropdown.options[i].text == "Losowy") cfg.brushColor = PastelColorFactory.RandomColor;
		else*/ cfg.brushColor = PastelColorFactory.GetColor(LineColorDropdown.options[i].text);
    }

    private void UpdateShapeColor(int i)
    {
		/*if(ShapeColorDropdown.options[i].text == "Losowy") cfg.shapeColor = PastelColorFactory.RandomColor;
		else*/ cfg.shapeColor = PastelColorFactory.GetColor(ShapeColorDropdown.options[i].text);
    }

    private void UpdateShape(int i)
    {
        cfg.shape = ShapeConverter.stringToShape(ShapeDropdown.options[i].text);
    }

    private void SetBrushColor(GameObject sender, string newValue, int index)
    {
        cfg.brushColor = PastelColorFactory.GetColor(newValue);
    }

    private void SetShapeColor(GameObject sender, string newValue, int index)
    {
        cfg.shapeColor = PastelColorFactory.GetColor(newValue);
    }

    private void SetShape(GameObject sender, string newValue, int index)
    {
        cfg.shape = (Shape)index;
    }

    void SetStroke(float value)
    {
		cfg.lineStroke = LineStroke.IntToFloat((int)value);
    }
	
	public void setKnobSize()
	{
		LineStrokeSlider.animator.SetInteger ("knobSize", (int)LineStrokeSlider.value);
	}

    public void SetActiveLevelNext()
    {
        creator.SendMessage("SetActiveLevelNext");
    }

    public void SetActiveLevelPrevious()
    {
        creator.SendMessage("SetActiveLevelPrevious");
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

    public void LevelInserted(int atIndex)
    {
        maxindex++;
        if (atIndex <= myindex)
        {
            myindex++;
        }

        updateBtns();
    }

    public void LevelDeleted(int atIndex)
    {
        
        maxindex--;

        if (atIndex < myindex)
            myindex--;

        Debug.Log("My idnex:" + myindex + " " + maxindex);

        updateBtns();
    }
    void updateBtns()
    {
        if (maxindex > 0)
            DeleteBtn.interactable = true;
        else
            DeleteBtn.interactable = false;

        if (myindex == 0)
            PreviousLevelBtn.interactable = false;
        else
            PreviousLevelBtn.interactable = true;

        if (myindex == maxindex)
            NextLevelBtn.interactable = false;
        else
            NextLevelBtn.interactable = true;
    }

    public void DeleteThis()
    {
        creator.LevelAdded -= new ConfigCreator.LevelAddHandler(LevelInserted);
        creator.LevelDeleted -= LevelDeleted;

        creator.SendMessage("RemoveLevel", myindex);

        Destroy(gameObject);
    }
}
