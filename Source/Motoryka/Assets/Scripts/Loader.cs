using UnityEngine;

public class Loader : MonoBehaviour {
    public GameObject gameManagerPrefab;

	// Use this for initialization
	void Start () {
	    if(this.gameManagerPrefab && !GameManager.IsInstantiated)
        {
            GameObject.Instantiate(this.gameManagerPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
