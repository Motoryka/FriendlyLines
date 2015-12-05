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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIConfigManager : MonoBehaviour {

	public void ToggleComboBoxOnClick(GameObject panel){
		if (panel.activeSelf) {
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
