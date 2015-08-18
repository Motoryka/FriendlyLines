using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LoadLevelAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Application.LoadLevel("ExampleLevel");
    }
}
