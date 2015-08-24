using UnityEngine;
using System.Collections;
using System.IO;

public class ConfigLoader : MonoBehaviour {

	public void SaveConfig()
	{
		StreamWriter fileWriter = null;

		string fileName = Application.persistentDataPath + "/" + "GraMotoryka" + ".txt"; 
		fileWriter = File.CreateText(fileName); 
		fileWriter.WriteLine("Hello world"); 
		fileWriter.Close();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
