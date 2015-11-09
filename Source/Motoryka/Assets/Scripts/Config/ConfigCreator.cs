using UnityEngine;
using System.Collections;

public class ConfigCreator : MonoBehaviour {

	public T GetUIElement<T>(string name)
	{
		var go = GameObject.Find (name);
		var element = go.GetComponent<T>();
		return (T) element;
	}

	// Use this for initialization
	void Start () {
		var shapeText = GetUIElement<ComboBox>("ShapeText");
		int shape = shapeText.SelectedOption;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
