using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PanelComboBox : MonoBehaviour, IDeselectHandler
{
    public void OnDeselect(BaseEventData data)
    {
        Debug.Log("Deselected");
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
