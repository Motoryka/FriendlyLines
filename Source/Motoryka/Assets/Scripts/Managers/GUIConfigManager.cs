using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIConfigManager : MonoBehaviour {

	public void ToggleComboBoxOnClick(GameObject panel){
		if (panel.active) {
			panel.SetActive (false);
		} else
			panel.SetActive (true);
	}

	public void ChangeSelectedItemOnClick(Text clickedText, Text selectedText){
		GameObject textField = GameObject.Find ("lol");
		clickedText = textField.GetComponent<Text> ();
		selectedText.text = clickedText.text;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
