using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboBox : MonoBehaviour {
    public string defaultText = "Choose";
    [SerializeField]
    public List<string> options;

    public delegate void ClickOutsideAction();
    public static event ClickOutsideAction OnClickOutside;

    int _selectedOption = 0;
    RectTransform _panel;
    Text _text;
    bool _ellapsed = false;

    GameObject _itemPrefab;


    public int SelectedOption
    {
        get { return _selectedOption; }
        set
        {
            if (options != null && value > options.Count)
            {
                _selectedOption = options.Count;

                if (_text != null)
                {
                    _text.text = options[_selectedOption - 1].ToString();
                }
                else
                {
                    _text = transform.FindChild("Text").GetComponent<Text>();
                    if (_text != null)
                    {
                        _text.text = options[_selectedOption - 1].ToString();
                    }
                }
            }
        }
    }



    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
        _panel = transform.FindChild("Panel").GetComponent<RectTransform>();
        _itemPrefab = (GameObject)Resources.Load("Prefabs/ComboBox/ComboBoxItem");
        _text = transform.FindChild("Text").GetComponent<Text>();

        int i = 0;
        float size = 0;
        foreach (object o in options)
        {
            RectTransform item = Instantiate(_itemPrefab).GetComponent<RectTransform>();
            float height;

            item.parent = _panel;
            item.anchorMin = new Vector2(0f, 1f);
            item.anchorMax = new Vector2(1f, 1f);

            Vector3 pos = item.position;
            pos.y = -( item.sizeDelta.y / 2 ) - ( i * item.sizeDelta.y );
            pos.x = 0;
            height = item.sizeDelta.y;
            //item.localPosition = pos;

            Debug.Log((item.sizeDelta.y));


            item.offsetMax = new Vector2(0, (-i * height));
            item.offsetMin = new Vector2(0, (-(i + 1) * height));

            Text t = item.FindChild("Text").GetComponent<Text>();
            t.text = o.ToString();
            item.GetComponent<Button>().onClick.AddListener(() => changeText(t.text));

            i++;
            size += height;
        }

        _panel.anchorMin = new Vector2(0f, 0f);
        _panel.anchorMax = new Vector2(1f, 0f);

        _panel.sizeDelta = new Vector2(_panel.sizeDelta.x, size);

        Debug.Log("size" + size + _panel.sizeDelta + (-_panel.sizeDelta.y / 2));

        //_panel.localPosition = new Vector3(0, -_panel.sizeDelta.y / 2f, _panel.position.z);
        _panel.offsetMax = new Vector2(0, 0);
        _panel.offsetMin = new Vector2(0, (-size));

        _panel.gameObject.SetActive(false);

        Button b = GetComponent<Button>();
        b.onClick.AddListener( () => ToggleEllapse());
        OnClickOutside += Collapse;


	}
	
	// Update is called once per frame
	void Update () {
	    if(_panel.gameObject.activeSelf && IfClickedOutside(_panel))
        {
            OnClickOutside();
        }
	}

    void ToggleEllapse()
    {
        Debug.Log("Toggle" + _ellapsed);
        _panel.gameObject.SetActive(_ellapsed = !_ellapsed);
    }

    void Collapse()
    {
        Debug.Log("Collapse");
        _panel.gameObject.SetActive(false);
        _ellapsed = false;
    }

    bool IfClickedOutside(RectTransform panel)
    {
        if (Input.GetMouseButton(0) && _ellapsed &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel,
                Input.mousePosition,
                Camera.main))
        {
           
           // return true;
        }
        return false;
    }

    void changeText(string text)
    {
        Debug.Log("text: " + text);
        _text.text = text;
        Collapse();
    }
}
