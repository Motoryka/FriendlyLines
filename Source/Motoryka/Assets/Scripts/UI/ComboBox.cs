using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboBox : MonoBehaviour {
    public string defaultText = "Choose";

    [SerializeField]
    public List<string> options;

    delegate void ClickOutsideAction(ComboBox sender = null, GameObject panel = null);
    static event ClickOutsideAction OnClickOutside;

    public delegate void OptionChangeAction(GameObject sender, string newValue);
    public event OptionChangeAction OnOptionChange;

    int _selectedOption = 0;
    RectTransform _panel;
    Text _text;
    bool _ellapsed = false;
    RectTransform _rectTransform;

    GameObject _itemPrefab;

    public static ComboBox Create()
    {
        var go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ComboBox/Button"));
        go.name = "Combo Box";
        go.transform.parent = GameObject.FindObjectOfType<Canvas>().transform;

        var panel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ComboBox/Panel"));
        panel.transform.SetParent(go.transform);
        panel.name = "Panel";
        panel.hideFlags = HideFlags.HideInHierarchy; 

        new List<Transform>(go.GetComponentsInChildren<Transform>()).ForEach(child => { 
            if( child != go.transform )
                child.hideFlags = HideFlags.HideInHierarchy; 
        });

        var cb = go.AddComponent<ComboBox>();

        cb.ReloadDefaultText();

        return cb;
    }

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
        ReloadOptions();
        ReloadDefaultText();
	}

    public void ReloadDefaultText()
    {
        LoadGameObjects();
        _text.text = defaultText;
    }

    public void LoadGameObjects()
    {
        if (_panel == null)
            _panel = transform.FindChild("Panel").GetComponent<RectTransform>();
        if (_itemPrefab == null)
            _itemPrefab = (GameObject)Resources.Load("Prefabs/ComboBox/ComboBoxItem");
        if (_text == null)
            _text = transform.FindChild("Text").GetComponent<Text>();
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

    }

    public void ReloadOptions()
    {
        LoadGameObjects();
        var children = new List<GameObject>();
        foreach (Transform t in _panel)
        {
            children.Add(t.gameObject);
        }

        children.ForEach(child => Destroy(child));

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
            pos.y = -(item.sizeDelta.y / 2) - (i * item.sizeDelta.y);
            pos.x = 0;
            height = item.sizeDelta.y;
            //item.localPosition = pos;

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

        //_panel.localPosition = new Vector3(0, -_panel.sizeDelta.y / 2f, _panel.position.z);
        _panel.offsetMax = new Vector2(0, 0);
        _panel.offsetMin = new Vector2(0, (-size));

        _panel.gameObject.SetActive(false);

        Button b = GetComponent<Button>();

        b.onClick.RemoveAllListeners();
        OnClickOutside = null;

        b.onClick.AddListener(() => ToggleEllapse());
        OnClickOutside += Collapse;
        
    }
	
	// Update is called once per frame
	void Update () {
	    if(_panel.gameObject.activeSelf && IfClickedOutside(_panel))
        {
            if (OnClickOutside != null)
                OnClickOutside(this, _panel.gameObject);
        }
	}

    void ToggleEllapse()
    {
        _panel.gameObject.SetActive(_ellapsed = !_ellapsed);
    }

    void Collapse(ComboBox sender, GameObject panel)
    {
        Debug.Log(sender.gameObject.name + ": Collapse");
        panel.gameObject.SetActive(false);
        sender._ellapsed = false;
    }

    bool IfClickedOutside(RectTransform panel)
    {
        if (Input.GetMouseButton(0) && _ellapsed)
        {

            Rect rect = rectTransformToRect(_panel);

            var mousePoint = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

            if (!rect.Contains(mousePoint))
            {
                rect = rectTransformToRect(_rectTransform);

                if (!rect.Contains(mousePoint))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void changeText(string text)
    {
        if (OnOptionChange != null)
            OnOptionChange(gameObject, text);

        _text.text = text;
        Collapse(this, _panel.gameObject);
    }

    Rect rectTransformToRect(RectTransform rt)
    {
        Vector3[] worldCorners = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        rt.GetWorldCorners(worldCorners);

        Rect rect = new Rect(worldCorners[0].x, Screen.height - worldCorners[1].y, worldCorners[2].x - worldCorners[0].x, worldCorners[1].y - worldCorners[0].y);
        return rect;
    }
}
