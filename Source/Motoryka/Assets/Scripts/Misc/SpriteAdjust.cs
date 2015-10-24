using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteAdjust : MonoBehaviour {

	public Button startButton;
	public Button optionButton;

	// Use this for initialization
	void Start () {
		SetBackground ();
		SetStartButton ();
	}

	void SetBackground()
	{
		SpriteRenderer renderer=GetComponent<SpriteRenderer>();
		if(renderer==null)
			return;
		
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

	void SetStartButton()
	{
		startButton.image.rectTransform.sizeDelta = new Vector2 ((Screen.width / 5)*3 , Screen.height / 2);
		startButton.image.transform.position = new Vector3 (Screen.width / 2, Screen.height / 2 - Screen.height / 10, 0);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
