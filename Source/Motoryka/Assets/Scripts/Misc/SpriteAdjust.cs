/**********************************************************************
Copyright (C) 2015  Jeżyna Domańska

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
using UnityEngine.UI;

public class SpriteAdjust : MonoBehaviour
{
	public Button optionButton;

	// Use this for initialization
	void Start() 
    {
		SetBackground ();
	}

	void SetBackground()
	{
		SpriteRenderer renderer=GetComponent<SpriteRenderer>();
	    if (renderer == null)
	    {
	        return;
	    }
		
		transform.localScale=new Vector3(1,1,1);
		
		float szerSprite=renderer.sprite.bounds.size.x;
		float wysSprite=renderer.sprite.bounds.size.y;
		
		float wysWorld=Camera.main.orthographicSize*2f;
		float szerWorld=wysWorld/Screen.height*Screen.width;
		
		Vector3 xWidth = transform.localScale;
		xWidth.x=szerWorld / szerSprite;
		transform.localScale=xWidth;
		Vector3 yHeight = transform.localScale;
		yHeight.y=wysWorld / wysSprite;
		transform.localScale=yHeight;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
