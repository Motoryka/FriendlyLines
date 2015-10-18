using UnityEngine;
using System.Collections;

public class SpriteAdjust : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
