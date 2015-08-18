using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
    public GameObject gameManagerPrefab;

	// Use this for initialization
	void Start () {

	    if(gameManagerPrefab)
        {
            GameObject.Instantiate(gameManagerPrefab);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
