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

using UnityEngine;
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

    private LevelConfig cfg;

    private ConfigCreator creator;

    private int myindex;
    private int maxindex;

    public float smoothTime = 0.2F;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start() 
    {
	}
	
	// Update is called once per frame
	void Update() 
    {
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
        {
            ShapeDropdown.options.Add(new Dropdown.OptionData(option));
        }
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
        foreach (PastelColor option in PastelColorFactory.ColorList)
        {
            LineColorDropdown.options.Add(new Dropdown.OptionData(option.Name));
        }
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
        {
            ShapeColorDropdown.options.Add(new Dropdown.OptionData(option.Name));
        }
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
        titleLbl.text = "Edytujesz poziom " + cfg.levelNumber + " / " + creator.config.NrOfLevels;
    }

    private void UpdateLineColor(int i)
    {
		cfg.brushColor = PastelColorFactory.GetColor(LineColorDropdown.options[i].text);
    }

    private void UpdateShapeColor(int i)
    {
		cfg.shapeColor = PastelColorFactory.GetColor(ShapeColorDropdown.options[i].text);
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
    }

    IEnumerator MoveToPointCrt(Vector3 point)
    {
        Debug.Log("Moving to point: " + point);
        velocity = Vector3.zero;
        while (transform.localPosition != point)
        {
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
